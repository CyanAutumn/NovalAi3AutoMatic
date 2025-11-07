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
            this.length = Tools.GetFileSize(this.form.picProps.RandomPromptFolderPath);
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
            return form.settingProps.KeepRandomPrompt && (form.runNum % form.picProps.RunKeepParams) != 0;
        }

        protected override string ParseResultText()
        {
            string tPrompt = null;
            int tIndex = index;
            if (pickRandom)
            {
                Random random = new Random();
                tIndex = random.Next(length);
                tPrompt = Tools.GetPromptFromFolderTxt(this.form.picProps.RandomPromptFolderPath, tIndex);
            }
            else
            {
                tPrompt = Tools.GetPromptFromFolderTxt(this.form.picProps.RandomPromptFolderPath, index);
                index = (index + 1) % length;
            }

            string[] words1 = tPrompt.Split(',').Select(word => word.Trim()).ToArray();
            IEnumerable<string> filtered = words1;
            if (form.picProps.EnablePromptBlackList) {
                string[] words2 = Prompt.GetPromptBlackList(form);
                filtered = filtered.Where(word => !words2.Contains(word));
                string[] words3 = Prompt.GetPromptBlackList(form, true);
                filtered = filtered.Where(word => !words3.Contains(word));
                var regexList = Prompt.GetPromptBlackListRegex(form);
                if (regexList.Count > 0) {
                    filtered = filtered.Where(word => !regexList.Any(regex => regex.IsMatch(word)));
                }
            }

            string strResult = string.Join(",", filtered).Trim();
            Logger.Info($"<随机提示词>：{strResult}",
                context: Logger.Context(("index", tIndex), ("value", strResult)));
            return strResult;
        }
    }
}
