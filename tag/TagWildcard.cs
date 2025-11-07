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
        private readonly IPromptContext context;

        public TagWildcard(string tag, IPromptContext context, string originalTag)
        {
            tag = tag.Trim();
            this.text = tag;
            this.pickRandom = GetPickType(tag);
            this.index = 0;
            this.context = context;
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
            return context.SettingProps.KeepWildcard && (context.RunNumber % context.RunKeepParams) != 0;
        }

        protected override string ParseResultText()
        {
            string[] lines = Tools.GetFileLine(this.context.PicProps.WildcardFolderPath, text);
            if (pickRandom)
            {
                Random random = new Random();
                int tIndex = random.Next(lines.Length);
                string words = lines[tIndex];
                Logger.Info($"<{this.text}>：{words}",
                    context: Logger.Context(("tag", this.text), ("index", tIndex), ("value", words)));
                return words;
            }
            else
            {
                string words = lines[index];
                int currentIndex = index;
                index = (index + 1) % lines.Length;
                Logger.Info($"<{this.text}>：{words}",
                    context: Logger.Context(("tag", this.text), ("index", currentIndex), ("value", words)));
                return words;
            }
        }
    }
}
