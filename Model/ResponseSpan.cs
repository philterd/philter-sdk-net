using Newtonsoft.Json;

namespace Philter.Model
{
    public class ResponseSpan
    {

        [JsonProperty("characterStart")]
        public int CharacterStart { get; set; }

        [JsonProperty("characterEnd")]
        public int CharacterEnd { get; set; }

        [JsonProperty("filterType")]
        public string FilterType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("documentId")]
        public string DocumentId { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }

        [JsonProperty("replacement")]
        public string Replacement { get; set; }

    }
}