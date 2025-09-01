using Newtonsoft.Json;

namespace MCS.UI.Controls
{
    public class AutoCompleteDataSource
    {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "parameters")]
        public object[] Parameters { get; set; }
    }
}
