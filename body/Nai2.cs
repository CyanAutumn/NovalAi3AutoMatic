using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.body {
    class Nai2 : NovalAIBase {
        public Nai2(Dictionary<string, object> kwargs) : base(kwargs) {
            this.model = BodyTools.GetEnumDescription(BodyTools.Model.Nai2);
        }
    }
}