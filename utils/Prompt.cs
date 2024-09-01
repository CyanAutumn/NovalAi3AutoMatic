using AutoNai3Tools.tag;
using AutoNai3Tools.artist;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace AutoNai3Tools.utils {
    internal class Prompt {
        private static string GetFolderPrompt(Form1 form) {
            string folderPath = form.txtRandomPromptFolderPath.Text;
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

            if (txtFiles.Length == 0) {
                throw new Exception("文件夹" + folderPath + "下没有 txt 文件");
            }
            Random random = new Random();
            string randomTxtFile = txtFiles[random.Next(txtFiles.Length)];
            string t_prompt = File.ReadAllText(randomTxtFile);
            string[] words1 = t_prompt.Split(',').Select(word => word.Trim()).ToArray();
            string[] words2 = form.txtPromptBlackList.Text.Split(',').Select(word => word.Trim()).ToArray();
            var result = words1.Where(word => !words2.Contains(word));
            string[] words3 = form.txtPromptBlackList.Text.Replace(" ", "_").Split(',').Select(word => word.Trim()).ToArray();
            result = result.Where(word => !words3.Contains(word));
            return string.Join(",", result).Trim();
        }

        private static string GetWillcard(string tag, Form1 form) {
            string folderPath = form.txtWildcardFolderPath.Text;
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

        public static string GetPrompt(string prompt, Form1 form) {
            if (prompt != prevPrompt) {
                string[] strTagList = prompt.Split(',');
                tagList.Clear();
                foreach (var item in strTagList) {
                    tagList.Add(TagTools.GetTagExample(item, form));
                }
                prevPrompt = prompt;
            }

            List<string> retTagList = new List<string>();
            foreach (var item in tagList) {
                retTagList.Add(item.ToString());
            }
            return string.Join(",", retTagList);
        }
    }
}
