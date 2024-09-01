using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagWildcard : TagBase {
        public string text { get; set; }
        public int index { get; set; }
        public bool pickRandom { get; set; }
        Form1 form;

        public TagWildcard(string tag, Form1 form) {
            tag = tag.Trim();
            this.text = tag;
            this.pickRandom = GetPickType(tag);
            this.index = 0;
            this.form = form;
        }

        private bool GetPickType(string tag) {
            string tempTag = tag.Substring(1, tag.Length - 2);
            string[] tempAfterList = tag.Split(new char[] { ':' });
            text = tempAfterList[0];
            if (tempAfterList.Length == 1)
                return true;
            if (tempAfterList[1] == "随机")
                return true;
            if (tempAfterList[1] == "顺序")
                return false;

            return false;
        }

        protected override bool KeepText() {
            return form.chkKeepWildcard.Checked && (form.runNum % form.numKeepParams.Value) != 0;
        }

        protected override string ParseResultText() {
            string[] lines = Tools.GetFileLine(this.form.txtWildcardFolderPath.Text, text);
            if (pickRandom) {
                Random random = new Random();
                string words = lines[random.Next(lines.Length)];
                form.PrintLog($"<{this.text}>:{words}");
                return words;
            }
            else {
                string words = lines[index];
                index = (index + 1) % lines.Length;
                form.PrintLog($"<{this.text}>:{words}");
                return words;
            }
        }
    }
}
