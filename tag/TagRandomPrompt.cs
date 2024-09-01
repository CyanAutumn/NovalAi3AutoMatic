using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagRandomPrompt : TagBase {
        public string text { get; set; }
        public int index { get; set; }
        public bool pickRandom { get; set; }
        Form1 form;

        public TagRandomPrompt(string tag, Form1 form) {
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
            return form.chkKeepRandomPrompt.Checked && (form.runNum % form.numKeepParams.Value) != 0;
        }

        protected override string ParseResultText() {
            string[] lines = Tools.GetFolderLine(this.form.txtRandomPromptFolderPath.Text);
            string tPrompt;
            if (pickRandom) {
                Random random = new Random();
                tPrompt = lines[random.Next(lines.Length)];
            }
            else {
                tPrompt = lines[index];
                index = (index + 1) % lines.Length;
            }
            string[] words1 = tPrompt.Split(',').Select(word => word.Trim()).ToArray();
            string[] words2 = form.txtPromptBlackList.Text.Split(',').Select(word => word.Trim()).ToArray();
            var result = words1.Where(word => !words2.Contains(word));
            string[] words3 = form.txtPromptBlackList.Text.Replace(" ", "_").Split(',').Select(word => word.Trim()).ToArray();
            result = result.Where(word => !words3.Contains(word));
            return string.Join(",", result).Trim();
        }
    }
}
