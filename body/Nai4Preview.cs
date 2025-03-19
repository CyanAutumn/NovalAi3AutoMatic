using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.body {
    class Nai4Preview : Nai4Full {
        public Nai4Preview(Dictionary<string, object> kwargs) : base(kwargs) {
            this.model = BodyTools.GetEnumDescription(BodyTools.Model.Nai4_Preview);
        }
    }
}