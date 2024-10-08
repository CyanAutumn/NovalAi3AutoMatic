using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagTools {
        public static TagBase GetTagExample(string tag, Form1 form) {
            //string[] replaceOldStr = { "{", "}", "[", "]" };
            //string tTag = tag.Trim();
            //for (int i = 0; i < replaceOldStr.Length; i++)
            //    tTag = tTag.Replace(replaceOldStr[i], "");

            string pattern = "<.*?>";
            Match match = Regex.Match(tag, pattern);
            if (match.Success) {
                string tTag = match.Value.Substring(1, match.Value.Length - 2);
                switch (tTag) {
                    case "固定画师":
                        return new TagFixedArtist(tTag, form, tag);
                    case "随机画师":
                        return new TagRandomArtist(tTag, form, tag);
                }
                if (tTag.StartsWith("随机提示词"))
                    return new TagRandomPrompt(tTag, form, tag);
                return new TagWildcard(tTag, form, tag);
            }
            return new TagDefault(tag, form, tag);
        }
    }
}
