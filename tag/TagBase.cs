using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AutoNai3Tools.tag {
    internal class TagBase {
        public string prevResult { get; set; }
        public string originalTag { get; set; }

        protected virtual string ParseResultText() {
            return null;
        }

        protected virtual bool KeepText() {
            return false;
        }

        public override string ToString() {
            if (prevResult == null || !KeepText())
                prevResult = ParseResultText();

            string pattern = "<.*?>";
            string result = Regex.Replace(this.originalTag, pattern, prevResult);
            return result;
        }
    }
}
