using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace AutoNai3Tools.utils
{
    internal class Prompt
    {
        private static string GetFolderPrompt(Form1 form)
        {
            string folderPath = form.txtRandomPromptFolderPath.Text;
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

            if (txtFiles.Length == 0)
            {
                throw new Exception("文件夹"+folderPath+"下没有 txt 文件");
            }
            Random random = new Random();
            string randomTxtFile = txtFiles[random.Next(txtFiles.Length)];
            string t_prompt = File.ReadAllText(randomTxtFile);
            string[] words1 = t_prompt.Split(',').Select(word => word.Trim()).ToArray();
            string[] words2 = form.txtPromptBlackList.Text.Split(',').Select(word => word.Trim()).ToArray();
            var result = words1.Where(word => !words2.Contains(word));
            string[] words3 = form.txtPromptBlackList.Text.Replace(" ","_").Split(',').Select(word => word.Trim()).ToArray();
            result = result.Where(word => !words3.Contains(word));
            return string.Join(",", result).Trim();
        }

        private static string GetWillcard(string tag,Form1 form)
        {
            string folderPath = form.txtWildcardFolderPath.Text;
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

            tag = tag.Substring(1, tag.Length - 2);
            string filePath = folderPath + "\\" + tag+".txt";
            string[] lines = File.ReadAllLines(filePath);

            // 从每行随机选择一个单词并组合
            Random random = new Random();
            string words = lines[random.Next(lines.Length)];
            return words;
        }
        public static string PrevArtistRandom = "";
        public static string PrevArtistFixed = "";
        public static string PrevRandomPrompt = "";
        public static Dictionary<string,string>PrevWillcard = new Dictionary<string, string>();
        private static string ParsePrompt(string tag,Form1 form,int runNum){
            if (tag == "<随机画师>")
            {
                if (form.chkKeepRandomArtist.Checked)
                {
                    if (runNum == 0 || runNum % form.numKeepParams.Value == 0)
                    {
                        List<List<Artist>> artistGroupList = Artist.ParseArtistTxtToArtistGroupList(form.txtArtistRandom.Text);
                        PrevArtistRandom = Artist.GetArtistPrompt(artistGroupList, ((int)form.numDefaultArtistWeightReduceMax.Value), ((int)form.numDefaultArtistWeightIncreaseMax.Value), form.chkArtistModify.Checked, ((int)form.numArtistMin.Value), ((int)form.numArtistMax.Value));
                    }
                }
                else
                {
                    List<List<Artist>> artistGroupList = Artist.ParseArtistTxtToArtistGroupList(form.txtArtistRandom.Text);
                    PrevArtistRandom = Artist.GetArtistPrompt(artistGroupList, ((int)form.numDefaultArtistWeightReduceMax.Value), ((int)form.numDefaultArtistWeightIncreaseMax.Value), form.chkArtistModify.Checked, ((int)form.numArtistMin.Value), ((int)form.numArtistMax.Value));
                }
                form.PrintLog("<随机画师>：" + PrevArtistRandom);
                return PrevArtistRandom;
            }
            else if (tag == "<固定画师>") {
                PrevArtistFixed = form.txtArtistFixed.Text;
                form.PrintLog("<固定画师>：" + PrevArtistFixed);
                return PrevArtistFixed;
            }
            else if (tag == "<随机提示词>")
            {
                if (form.chkKeepRandomPrompt.Checked)
                {
                    if (runNum == 0 || runNum % form.numKeepParams.Value == 0)
                    {
                        PrevRandomPrompt = GetFolderPrompt(form);
                    }
                }
                else
                {
                    PrevRandomPrompt = GetFolderPrompt(form);
                }
                form.PrintLog("<随机提示词>：" + PrevRandomPrompt);
                return PrevRandomPrompt;
            }
            else
            {
                if (!PrevWillcard.ContainsKey(tag))
                {
                    PrevWillcard[tag] = GetWillcard(tag, form);
                }
                else if (form.chkKeepWildcard.Checked)
                {
                    if (runNum == 0 || runNum % form.numKeepParams.Value == 0)
                    {
                        PrevWillcard[tag] = GetWillcard(tag, form);
                    }
                }
                else
                {
                    PrevWillcard[tag] = GetWillcard(tag, form);
                }
                form.PrintLog(tag+"：" + PrevWillcard[tag]);
                return PrevWillcard[tag];
            }
        }
        public static string GetPrompt(string prompt,Form1 form,int runNum){
            string[] tagList = prompt.Split(',');
            for (int i = 0; i < tagList.Length; i++)
            {
                string tag = tagList[i].Trim();
                if (tag.StartsWith("<") && tag.EndsWith(">"))
                {
                    string resultTag = ParsePrompt(tag,form,runNum);
                    tagList[i] = tagList[i].Replace(tag, resultTag);
                }
            }
            return string.Join(",", tagList);
        }
    }
}
