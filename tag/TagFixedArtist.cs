using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag
{
    internal class TagFixedArtist : TagBase
    {
        public string text { get; set; }
        private readonly IPromptContext context;

        public TagFixedArtist(string tag, IPromptContext context, string originalTag)
        {
            this.text = tag;
            this.context = context;
            this.originalTag = originalTag;
        }

        protected override string ParseResultText()
        {
            Logger.Info($"<固定画师>：{context.ArtistFixedText}",
                context: Logger.Context(("value", context.ArtistFixedText)));
            return context.ArtistFixedText;
        }
    }
}
