using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagDefault : TagBase {
        public string text { get; set; }
        Form1 form;

        public TagDefault(string tag, Form1 form) {
            this.text = tag;
            this.form = form;
        }

        protected override string ParseResultText() {
            return text;
        }
    }
}
