using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag
{
    internal class TagWildcard : TagBase
    {
        public string text { get; set; }
        public int index { get; set; }
        public bool pickRandom { get; set; }
        Form1 form;

        public TagWildcard(string tag, Form1 form, string originalTag)
        {
            tag = tag.Trim();
            this.text = tag;
            this.pickRandom = GetPickType(tag);
            this.index = 0;
            this.form = form;
            this.originalTag = originalTag;
        }

        private bool GetPickType(string tag)
        {
            string[] tempAfterList = tag.Split(new char[] { ':' });
            text = tempAfterList[0];
            if (tempAfterList.Length == 1)
                return true;
            return tempAfterList[1] == "随机";
        }

        protected override bool KeepText()
        {
            return form.chkKeepWildcard.Checked && (form.runNum % form.numKeepParams.Value) != 0;
        }

        protected override string ParseResultText()
        {
            string[] lines = Tools.GetFileLine(this.form.txtWildcardFolderPath.Text, text);
            if (pickRandom)
            {
                Random random = new Random();
                int tIndex = random.Next(lines.Length);
                string words = lines[tIndex];
                Logger.Info($"<{this.text}> {tIndex}:{words}");
                return words;
            }
            else
            {
                string words = lines[index];
                index = (index + 1) % lines.Length;
                Logger.Info($"<{this.text}> {index}:{words}");
                return words;
            }
        }
    }
}