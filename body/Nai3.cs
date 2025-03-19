using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.body {
    class Nai3 : NovalAIBase {
        public Nai3(Dictionary<string, object> kwargs) : base(kwargs) {
            this.model = BodyTools.GetEnumDescription(BodyTools.Model.Nai3);
            this.parameters.sm = false;
            this.parameters.sm_dyn = false;
            this.parameters.extra_noise_seed = 0;
            this.parameters.strength = 0;
            this.parameters.noise = 0;
        }
    }
}