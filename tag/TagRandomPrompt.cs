using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag
{
    internal class TagRandomPrompt : TagBase
    {
        public string text { get; set; }
        public int index { get; set; }
        public int length { get; set; }
        public bool pickRandom { get; set; }
        Form1 form;

        public TagRandomPrompt(string tag, Form1 form, string originalTag)
        {
            this.text = tag;
            this.pickRandom = GetPickType(tag);
            this.index = 0;
            this.form = form;
            this.length = Tools.GetFileSize(this.form.txtRandomPromptFolderPath.Text);
            this.originalTag = originalTag;
        }

        private bool GetPickType(string tag)
        {
            string tempTag = tag.Substring(1, tag.Length - 2);
            string[] tempAfterList = tag.Split(new char[] { ':' });
            text = tempAfterList[0];
            if (tempAfterList.Length == 1)
                return true;
            return tempAfterList[1] == "随机";
        }

        protected override bool KeepText()
        {
            return form.chkKeepRandomPrompt.Checked && (form.runNum % form.picProps.RunKeepParams) != 0;
        }

        protected override string ParseResultText()
        {
            string tPrompt = null;
            int tIndex = index;
            if (pickRandom)
            {
                Random random = new Random();
                tIndex = random.Next(length);
                tPrompt = Tools.GetPromptFromFolderTxt(this.form.txtRandomPromptFolderPath.Text, tIndex);
            }
            else
            {
                tPrompt = Tools.GetPromptFromFolderTxt(this.form.txtRandomPromptFolderPath.Text, index);
                index = (index + 1) % length;
            }

            string[] words1 = tPrompt.Split(',').Select(word => word.Trim()).ToArray();
            string[] words2 = form.txtPromptBlackList.Text.Split(',').Select(word => word.Trim()).ToArray();
            var result = words1.Where(word => !words2.Contains(word));
            string[] words3 = form.txtPromptBlackList.Text.Replace(" ", "_").Split(',').Select(word => word.Trim())
                .ToArray();
            result = result.Where(word => !words3.Contains(word));
            string strResult = string.Join(",", result).Trim();
            Logger.Info($"<随机提示词> {tIndex}:{strResult}");
            return strResult;
        }
    }
}