using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagDefault : TagBase {
        public string text { get; set; }

        public TagDefault(string tag, IPromptContext context, string originalTag) {
            this.text = tag;
            this.originalTag = originalTag;
        }

        protected override string ParseResultText() {
            return text;
        }
    }
}
