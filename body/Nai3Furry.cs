using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoNai3Tools.body.BodyTools;

namespace AutoNai3Tools.body {
    class Nai3Furry : NovalAIBase {
        public Nai3Furry(Dictionary<string, object> kwargs) : base(kwargs) {
            this.model = BodyTools.GetEnumDescription(BodyTools.Model.Nai3_Furry);
            this.parameters.extra_noise_seed = 0;
            this.parameters.strength = 0;
            this.parameters.noise = 0;
        }
    }
}
