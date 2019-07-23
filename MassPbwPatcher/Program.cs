using LibRpws.Apps;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MassPbwPatcher
{
    class Program
    {
        /// <summary>
        /// The input from the app database you'd like to use (this one no longer works)
        /// </summary>
        public const string SOURCE_URL = "https://pebble-appstore.romanport.com/api/raw/stream/?offset=0";

        /// <summary>
        /// Where you'd like to redirect timeline requests to
        /// </summary>
        public const string PATCHED_ENDPOINT = "blue.api.get-rpws.com";

        /// <summary>
        /// Where you'd like to place output files. Use {id} as a placeholder for the app ID
        /// </summary>
        public const string OUTPUT_PATH = @"C:\Users\Roman\Documents\PatchedPbws\{id}.pbw";

        /// <summary>
        /// Where you will place the output JSON file containing converted apps.
        /// </summary>
        public const string OUTPUT_DB = @"C:\Users\Roman\Documents\PatchedPbws\db.json";

        static void Main(string[] args)
        {
            Console.WriteLine("Downloading app data...");
            //Download all apps.
            string next = SOURCE_URL;
            JsonSerializerSettings sett = new JsonSerializerSettings();
            List<PebbleApp> apps = new List<PebbleApp>();
            List<string> validIds = new List<string>();
            while (next != null)
            {
                var page = RunAppHttpApi(sett, next);
                next = page.next;
                //add apps
                apps.AddRange(page.data);
            }
            //Now, patch all.
            Console.WriteLine("Patching PBWs...");
            int done = 0;
            int failed = 0;
            Parallel.For(0, apps.Count, (int i) =>
            {
                PebbleApp app = apps[i];
                bool ok = PatchPbw(app, PATCHED_ENDPOINT, OUTPUT_PATH.Replace("{id}", app.id));
                if (ok)
                    validIds.Add(app.id);
                else
                    failed++;
                done++;
                Console.Title = "RPWS PBW Patcher - " + done.ToString() + " done - " + failed.ToString() + " failed - " + ((int)(((float)done / (float)apps.Count) * 100)).ToString() + "% finished";
            });
            Console.WriteLine("Complete. Saving database.");
            string db = JsonConvert.SerializeObject(validIds.ToArray());
            File.WriteAllText(OUTPUT_DB, db);
            Console.WriteLine("All finished.");
            Console.ReadLine();
        }

        public static PebbleApiLocalCachePage RunAppHttpApi(JsonSerializerSettings sett, string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                //Some patch I had to use early on. Ignore.
                //string app = reader.ReadToEnd().Replace("%rootUrl%files", "https://assets-static-1.romanport.com/pebble_appstore_static_originals").Replace("%rootUrl%/files", "https://assets-static-1.romanport.com/pebble_appstore_static_originals");

                return JsonConvert.DeserializeObject<PebbleApiLocalCachePage>(reader.ReadToEnd(), sett);
            }
        }

        public static bool PatchPbw(PebbleApp app, string timelineDomain, string saveLocation)
        {
            try
            {
                PebbleApp_Release release = app.latest_release.GetRelease();
                //Download the PBW file from the server.
                using (FileStream pbw = new FileStream(saveLocation, FileMode.Create))
                {
                    //Download and copy to this stream.
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(release.pbw_file);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    {
                        stream.CopyTo(pbw);
                    }
                    //Rewind to the beginning.
                    pbw.Position = 0;
                    //Open the ZIP file.
                    using (var zip = new ZipArchive(pbw, ZipArchiveMode.Update, true))
                    {
                        //Check if the "pebble-js-app.js" file exists.
                        var js = zip.GetEntry("pebble-js-app.js");
                        //Work on it if it exists.
                        if (js != null)
                        {
                            //Open.
                            using (var jsFile = js.Open())
                            {
                                //Read in the javascript.
                                byte[] buf = new byte[jsFile.Length];
                                jsFile.Read(buf, 0, buf.Length);
                                string jsData = Encoding.UTF8.GetString(buf);
                                //Do find and replace.
                                jsData = jsData.Replace("timeline-api.getpebble.com", timelineDomain);
                                //Write this back.
                                buf = Encoding.UTF8.GetBytes(jsData);
                                jsFile.Position = 0;
                                jsFile.SetLength(buf.Length);
                                jsFile.Write(buf, 0, buf.Length);
                                //Close and save the file.
                            }
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("FATAL ERROR WHILE PATCHING " + app.id + " (" + app.title + ") - " + ex.Message +" @ "+ex.StackTrace+"\r\n\r\n");
                File.Delete(saveLocation);
                return false;
            }
        }
    }

    public class PebbleApiLocalCachePage
    {
        public PebbleApp[] data;
        public string next;
    }

    public class PebbleApiLocalCachePageGeneric
    {
        public PebbleApiLocalCachePageGenericObject[] data;
    }

    public class PebbleApiLocalCachePageGenericObject
    {
        public string id;
        public string uuid;
    }
}
