using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibRpws.Apps
{
    [Serializable]
    public class PebbleApp_Capabilities
    {
        public int is_timeline_enabled { get; set; }
        public int is_configurable { get; set; }
    }

    [Serializable]
    public class PebbleApp_RPWS_Dev
    {
        public PebbleApp_Release[] release_history { get; set; }
        public bool changes_pending { get; set; }
    }

    [Serializable]
    public class PebbleApp
    {

        public string author { get; set; }
        //Capabilities

        public string category_id { get; set; }

        public string category_name { get; set; }

        public string category_color { get; set; }

        public PebbleApp_ChangelogItem[] changelog { get; set; }

        public PebbleApp_Companions companions { get; set; }

        public PebbleApp_Compatibility compatibility { get; set; }

        public string created_at { get; set; }

        public string description { get; set; }

        public string developer_id { get; set; }

        public PebbleApp_HeaderImg[] header_images { get; set; }

        public int hearts { get; set; }

        //public PebbleAppOutput_IconImages icon_image {get; set;}

        public string id { get; set; }

        public PebbleAppOutput_Releases latest_release { get; set; }

        public PebbleApp_Links links { get; set; }

        public PebbleAppOutput_ListImages list_image { get; set; }

        public string published_date { get; set; }

        public PebbleAppOutput_ScreenshotImages screenshot_images { get; set; } //To convert

        public string source { get; set; }

        public string title { get; set; }

        public string type { get; set; }

        public string uuid { get; set; }

        public string website { get; set; }

        public PebbleAppOutput_IconImages icon_image { get; set; }

        /* New */

        public AppMeta meta { get; set; }

        public PebbleApp_Capabilities capabilities { get; set; }

        public int isOriginal { get; set; }
        public int isPublished { get; set; }
        public int isTimelineKnownUnsupported { get; set; }
    }

    [Serializable]
    public class PebbleAppOutput_ScreenshotImages
    {

        public PebbleApp_ScreenshotImg[] aplite { get; set; }

        public PebbleApp_ScreenshotImg[] basalt { get; set; }

        public PebbleApp_ScreenshotImg[] chalk { get; set; }

        public PebbleApp_ScreenshotImg[] diorite { get; set; }

        public PebbleApp_ScreenshotImg[] emery { get; set; }
    }

    [Serializable]
    public class PebbleAppOutput_Releases
    {

        public PebbleApp_Release aplite { get; set; }

        public PebbleApp_Release basalt { get; set; }

        public PebbleApp_Release chalk { get; set; }

        public PebbleApp_Release diorite { get; set; }

        public PebbleApp_Release emery { get; set; }

        public PebbleApp_Release GetRelease()
        {
            PebbleApp_Release release = basalt;
            if (release == null)
                release = diorite;
            if (release == null)
                release = emery;
            if (release == null)
                release = chalk;
            if (release == null)
                release = aplite;
            if (release == null)
                throw new Exception("No app releases found.");
            return release;
        }
    }

    [Serializable]
    public class PebbleAppOutput_IconImages
    {

        public PebbleApp_IconImg aplite { get; set; }

        public PebbleApp_IconImg basalt { get; set; }

        public PebbleApp_IconImg chalk { get; set; }

        public PebbleApp_IconImg diorite { get; set; }

        public PebbleApp_IconImg emery { get; set; }
    }

    [Serializable]
    public class PebbleAppOutput_ListImages
    {

        public PebbleApp_ListImg aplite { get; set; }

        public PebbleApp_ListImg basalt { get; set; }

        public PebbleApp_ListImg chalk { get; set; }

        public PebbleApp_ListImg diorite { get; set; }

        public PebbleApp_ListImg emery { get; set; }
    }

    [Serializable]
    public class PebbleApp_ChangelogItem
    {

        public string version { get; set; }

        public string published_date { get; set; }

        public string release_notes { get; set; }
    }

    [Serializable]
    public class PebbleApp_Companions
    {

        public CompanionPlatform ios { get; set; }

        public CompanionPlatform android { get; set; }

    }

    [Serializable]
    public class CompanionPlatform
    {
        public string id { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool required { get; set; }
        public string pebblekit_version { get; set; }
    }

    [Serializable]
    public class PebbleApp_Compatibility
    {

        public PebbleApp_Compatibility_Phone ios { get; set; }

        public PebbleApp_Compatibility_Phone android { get; set; }

        public PebbleApp_Compatibility_Hardware emery { get; set; }

        public PebbleApp_Compatibility_Hardware diorite { get; set; }

        public PebbleApp_Compatibility_Hardware chalk { get; set; }

        public PebbleApp_Compatibility_Hardware basalt { get; set; }

        public PebbleApp_Compatibility_Hardware aplite { get; set; }
    }

    [Serializable]
    public class PebbleApp_Compatibility_Hardware
    {

        public PebbleApp_Compatibility_Hardware_Firmware firmware { get; set; }

        public bool supported { get; set; }
    }

    [Serializable]
    public class PebbleApp_Compatibility_Hardware_Firmware
    {

        public int major { get; set; }
    }

    [Serializable]
    public class PebbleApp_Compatibility_Phone
    {

        public bool supported { get; set; }
    }

    [Serializable]
    public class PebbleApp_HeaderImg
    {

        public string orig { get; set; }
    }

    [Serializable]
    public class PebbleApp_IconImg
    {
        [JsonProperty("48x48")]
        public string _48x48 { get; set; }
    }

    [Serializable]
    public class PebbleApp_ListImg
    {
        [JsonProperty("144x144")]
        public string _144x144 { get; set; }
    }

    [Serializable]
    public class PebbleApp_ScreenshotImg
    {
        [JsonProperty("144x168")]
        public string _144x168 { get; set; }
        [JsonProperty("180x180")]
        public string _180x180 { get; set; }

        public string GetUrl()
        {
            var url = _144x168;
            if (url == null)
                url = _180x180;
            return url.Replace("%rootUrl%files/", "https://assets-static-1.romanport.com/pebble_appstore_static_originals/");
        }
    }

    [Serializable]
    public class PebbleApp_Release
    {

        public string id { get; set; }

        public float js_version { get; set; }

        public string pbw_file { get; set; }

        public string published_date { get; set; }

        public string release_notes { get; set; }

        public string version { get; set; }
    }

    [Serializable]
    public class PebbleApp_Links
    {

        public string add { get; set; }

        public string remove { get; set; }

        public string add_heart { get; set; }

        public string remove_heart { get; set; }

        public string add_flag { get; set; }

        public string remove_flag { get; set; }

        public string share { get; set; }
    }

    [Serializable]
    public enum WatchHardware
    {
        aplite, //OG Pebble / Pebble Steel
        basalt, //Pebble Time / Pebble Time Steel
        chalk, //Pebble Time Round
        diorite, //Pebble 2
        emery //Pebble Time 2
    }

    [Serializable]
    public class AppMeta
    {
        public string header { get; set; }
        public string struct_version { get; set; }
        public string sdk_version { get; set; }
        public string app_version { get; set; }
        public int size { get; set; }
        public int offset { get; set; }
        public int crc { get; set; }
        public string appname { get; set; }
        public string companyname { get; set; }
        public long icon_resource_id { get; set; }
        public long symbol_table_address { get; set; }
        public long pebble_process_info_flags { get; set; }
        public long relocation_list { get; set; }
        public string uuid {get; set; }

        //Even newer
        public bool isTimelineEnabled { get; set; }
    }

    [Serializable]
    public class PebbleAppDbStorage
    {
        public int _id { get; set; }
        public PebbleApp app { get; set; }
        public int dbVersion { get; set; }
        public PebbleApp_RPWS_Dev dev { get; set; }
        public bool deleted { get; set; }
    }
}
