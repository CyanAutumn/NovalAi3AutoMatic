using AutoNai3Tools.utils;
using AutoNai3Tools.artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag
{
    internal class TagRandomArtist : TagBase
    {
        public string text { get; set; }
        public bool pickRandom { get; set; }
        private readonly IPromptContext context;

        public TagRandomArtist(string tag, IPromptContext context, string originalTag)
        {
            this.text = tag;
            this.context = context;
            this.originalTag = originalTag;
        }

        protected override bool KeepText()
        {
            return context.SettingProps.KeepRandomArtist && (context.RunNumber % context.RunKeepParams) != 0;
        }

        protected override string ParseResultText()
        {
            List<List<Artist>> artistGroupList = ArtistTools.ParseArtistTxtToArtistGroupList(context.ArtistRandomText);
            string randomArtist = ArtistTools.GetArtistPrompt(artistGroupList,
                context.DefaultArtistWeightReduceMax, context.DefaultArtistWeightIncreaseMax,
                context.DefaultArtistWeightReduceDoubleColonMax, context.DefaultArtistWeightIncreaseDoubleColonMax,
                context.ArtistModify, context.ArtistMin, context.ArtistMax);
            Logger.Info($"<随机画师>：{randomArtist}",
                context: Logger.Context(("value", randomArtist)));
            return randomArtist;
        }
    }
}
