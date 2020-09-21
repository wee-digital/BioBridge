using System;
using Newtonsoft.Json;

namespace BioBridge
{
    class CameraData
    {
        [JsonProperty(PropertyName = "Status")]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "TimeIn")]
        public String TimeIn { get; set; }

        [JsonProperty(PropertyName = "IsReal")]
        public Boolean IsReal { get; set; }

        [JsonProperty(PropertyName = "FaceColor")]
        public String FaceColor { get; set; }

    }

    class FingerprintData
    {

        [JsonProperty(PropertyName = "Status")]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "TimeIn")]
        public String TimeIn { get; set; }

        [JsonProperty(PropertyName = "DPI")]
        public Int16 DPI { get; set; }

        [JsonProperty(PropertyName = "Rate")]
        public Int16 Rate { get; set; }

        [JsonProperty(PropertyName = "Data")]
        public String Data { get; set; }

        [JsonProperty(PropertyName = "TemplateData")]
        public String TemplateData { get; set; }

       
    }

    class SocketData
    {
        [JsonProperty(PropertyName = "Type")]
        public Int16 Type { get; set; }

        [JsonProperty(PropertyName = "CameraData", NullValueHandling = NullValueHandling.Ignore)]
        public CameraData CameraData { get; set; }

        [JsonProperty(PropertyName = "FpData", NullValueHandling = NullValueHandling.Ignore)]
        public FingerprintData FingerprintData { get; set; }
    }
}
