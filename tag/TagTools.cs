using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagTools {
        public static TagBase GetTagExample(string tag, Form1 form) {
            string tTag = tag.Trim();
            if (tTag.StartsWith("<") && tTag.EndsWith(">")) {
                tTag = tTag.Substring(1, tTag.Length - 2);
                switch (tTag) {
                    case "固定画师":
                        return new TagFixedArtist(tTag, form);
                    case "随机画师":
                        return new TagRandomArtist(tTag, form);
                    case "随机提示词":
                        return new TagRandomPrompt(tTag, form);
                }
                return new TagWildcard(tTag, form);
            }
            return new TagDefault(tTag, form);
        }
    }
}
