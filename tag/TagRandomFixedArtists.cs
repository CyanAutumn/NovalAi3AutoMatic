using AutoNai3Tools.artist;
using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagRandomFixedArtists : TagBase {
        public string text { get; set; }
        public bool pickRandom { get; set; }
        private readonly IPromptContext context;

        public TagRandomFixedArtists(string tag, IPromptContext context, string originalTag) {
            this.text = tag;
            this.context = context;
            this.originalTag = originalTag;
        }

        protected override bool KeepText() {
            return context.SettingProps.KeepRandomArtist && (context.RunNumber % context.RandomFixedKeepParams) != 0;
        }

        protected override string ParseResultText() {
            List<List<Artist>> artistGroupList = ArtistTools.ParseArtistTxtToArtistGroupList(context.ArtistRandomFixedText);
            string randomArtist = ArtistTools.GetArtistPromptFixed(artistGroupList,
                context.DefaultArtistRandomFixedWeightIncreaseMin,
                context.DefaultArtistRandomFixedWeightIncreaseMax,
                context.ArtistModifyRandomFixed);
            Logger.Info($"<固定随机画师>：{randomArtist}",
                context: Logger.Context(("value", randomArtist)));
            return randomArtist;
        }
    }
}
