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
        private readonly IPromptContext context;

        public TagRandomPrompt(string tag, IPromptContext context, string originalTag)
        {
            this.text = tag;
            this.pickRandom = GetPickType(tag);
            this.index = 0;
            this.context = context;
            this.length = Tools.GetFileSize(this.context.PicProps.RandomPromptFolderPath);
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
            return context.SettingProps.KeepRandomPrompt && (context.RunNumber % context.RunKeepParams) != 0;
        }

        protected override string ParseResultText()
        {
            string tPrompt = null;
            int tIndex = index;
            if (pickRandom)
            {
                Random random = new Random();
                tIndex = random.Next(length);
                tPrompt = Tools.GetPromptFromFolderTxt(this.context.PicProps.RandomPromptFolderPath, tIndex);
            }
            else
            {
                tPrompt = Tools.GetPromptFromFolderTxt(this.context.PicProps.RandomPromptFolderPath, index);
                index = (index + 1) % length;
            }

            string[] words1 = tPrompt.Split(',').Select(word => word.Trim()).ToArray();
            IEnumerable<string> filtered = words1;

            string[] words2 = Prompt.GetPromptBlackList(context);
            if (words2.Length > 0)
                filtered = filtered.Where(word => !words2.Contains(word));

            string[] words3 = Prompt.GetPromptBlackList(context, true);
            if (words3.Length > 0)
                filtered = filtered.Where(word => !words3.Contains(word));

            var regexList = Prompt.GetPromptBlackListRegex(context);
            if (regexList.Count > 0) {
                filtered = filtered.Where(word => !regexList.Any(regex => regex.IsMatch(word)));
            }

            string strResult = string.Join(",", filtered).Trim();
            Logger.Info($"<随机提示词>：{strResult}",
                context: Logger.Context(("index", tIndex), ("value", strResult)));
            return strResult;
        }
    }
}
