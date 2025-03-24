using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.body {
    class Nai4Full : NovalAIBase {
        public List<string> characterPrompts { get; set; }

        public Nai4Full(Dictionary<string, object> kwargs) : base(kwargs) {
            this.model = BodyTools.GetEnumDescription(BodyTools.Model.Nai4_Full);
            this.parameters.autoSmea = false;
            this.parameters.prefer_brownian = true;
            this.parameters.use_coords = false;
            this.parameters.sm = null;
            this.parameters.sm_dyn = null;
        }
    }
}