using AutoNai3Tools.tag;
using AutoNai3Tools.artist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;

namespace AutoNai3Tools.utils {
    internal class Prompt {
        internal static string[] GetPromptBlackList(IPromptContext context, bool replaceSpaceWithUnderscore = false) {
            if (context?.PicProps == null || !context.PicProps.EnablePromptBlackList)
                return Array.Empty<string>();

            string raw = context.PicProps.PromptBlackList ?? string.Empty;
            if (replaceSpaceWithUnderscore) {
                raw = raw.Replace(" ", "_");
            }

            return raw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.Trim())
                .Where(word => !string.IsNullOrEmpty(word))
                .ToArray();
        }

        internal static List<Regex> GetPromptBlackListRegex(IPromptContext context) {
            List<Regex> patterns = new List<Regex>();
            if (context?.PicProps == null || !context.PicProps.EnablePromptBlackList)
                return patterns;

            string raw = context.PicProps.PromptBlackListRegex ?? string.Empty;
            var lines = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines) {
                var pattern = line.Trim();
                if (string.IsNullOrEmpty(pattern))
                    continue;
                try {
                    patterns.Add(new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase));
                }
                catch (Exception ex) {
                    Logger.Warn("提示词正则黑名单无效",
                        context: Logger.Context(("pattern", pattern), ("reason", ex.Message)));
                }
            }
            return patterns;
        }

        private static string GetFolderPrompt(IPromptContext context) {
            string folderPath = context.PicProps.RandomPromptFolderPath;
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

            if (txtFiles.Length == 0) {
                throw new Exception("文件夹" + folderPath + "下没有 txt 文件");
            }

            Random random = new Random();
            string randomTxtFile = txtFiles[random.Next(txtFiles.Length)];
            string t_prompt = File.ReadAllText(randomTxtFile);
            string[] words1 = t_prompt.Split(',').Select(word => word.Trim()).ToArray();

            IEnumerable<string> filtered = words1;
            if (context.PicProps.EnablePromptBlackList) {
                string[] words2 = GetPromptBlackList(context);
                filtered = filtered.Where(word => !words2.Contains(word));
                string[] words3 = GetPromptBlackList(context, true);
                filtered = filtered.Where(word => !words3.Contains(word));
                var regexList = GetPromptBlackListRegex(context);
                if (regexList.Count > 0) {
                    filtered = filtered.Where(word => !regexList.Any(regex => regex.IsMatch(word)));
                }
            }

            return string.Join(",", filtered).Trim();
        }

        private static string GetWillcard(string tag, IPromptContext context) {
            string folderPath = context.PicProps.WildcardFolderPath;
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

            tag = tag.Substring(1, tag.Length - 2);
            string filePath = folderPath + "\\" + tag + ".txt";
            string[] lines = File.ReadAllLines(filePath);

            Random random = new Random();
            string words = lines[random.Next(lines.Length)];
            return words;
        }

        public static string PrevArtistRandom = "";
        public static string PrevArtistFixed = "";
        public static string PrevRandomPrompt = "";
        public static List<TagBase> tagList = new List<TagBase>();
        public static string prevPrompt = "";

        public static Dictionary<string, string> GetPrompt(string prompt, IPromptContext context) {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string[] strTagList = prompt.Split(',');
            if (prompt != prevPrompt) {
                tagList.Clear();
                foreach (var item in strTagList) {
                    tagList.Add(TagTools.GetTagExample(item, context));
                }

                prevPrompt = prompt;
            }

            List<string> retTagList = new List<string>();
            for (int i = 0; i < tagList.Count; i++) {
                if (result.ContainsKey(strTagList[i])) {
                    strTagList[i] += $"_{new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString()}+{i}";
                }

                result.Add(strTagList[i], tagList[i].ToString());
            }

            return result;
        }

        public static string GetDataPrompt(Dictionary<string, string> data) {
            List<string> result = new List<string>();
            foreach (var item in data) {
                result.Add(item.Value);
            }

            return string.Join(",", result);
        }

        public static string GetNoArtistPrompt(Dictionary<string, string> data) {
            List<string> result = new List<string>();
            foreach (var item in data) {
                if (item.Key.Contains("<固定画师") || item.Key.Contains("<随机画师"))
                    continue;
                result.Add(item.Value);
            }

            return string.Join(",", result).Replace("year 2023", "").Replace("years 2023", "").Replace("year_2023", "")
                .Replace("years_2023", "");
        }
    }
}
