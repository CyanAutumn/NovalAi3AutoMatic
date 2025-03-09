using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.body {
    class Nai3 : NovalAI {
        public Nai3(Dictionary<string, object> kwargs) : base(kwargs) {
            this.model = "nai-diffusion-3";
        }
    }
}