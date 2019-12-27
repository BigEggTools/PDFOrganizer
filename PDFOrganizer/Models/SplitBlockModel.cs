using Newtonsoft.Json;

namespace BigEgg.PDFOrganizer.Models
{
    [JsonObject("block")]
    public class SplitBlockModel
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("start", Required = Required.Always)]
        public int StartPage { get; set; }

        [JsonProperty("end", Required = Required.Always)]
        public int EndPage { get; set; }
    }
}
