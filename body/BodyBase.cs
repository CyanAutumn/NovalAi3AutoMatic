using System.Collections.Generic;

namespace AutoNai3Tools.body {
    public class BodyBase {
        public virtual string prompt { get; set; }
        public BodyBase(Dictionary<string, object> kwargs) { }

        public string ToJson() {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, new Newtonsoft.Json.JsonSerializerSettings {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
            });
        }
    }
}