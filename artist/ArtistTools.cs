using AutoNai3Tools.utils;
using AutoNai3Tools.artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.artist {
    internal class ArtistTools {
        private static Artist ParseArtistTagToArtist(string artistTag) {
            string[] strArtistAttrList = artistTag.Split(',');
            if (strArtistAttrList.Length != 1 && strArtistAttrList.Length != 5) {
                throw new Exception(artistTag + " 错误，请检查格式");
            }
            if (strArtistAttrList.Length == 1) {
                return new Artist(strArtistAttrList[0], null, null, null, null);
            }
            else {
                return new Artist(strArtistAttrList[0], int.Parse(strArtistAttrList[1]), int.Parse(strArtistAttrList[2]), int.Parse(strArtistAttrList[3]), int.Parse(strArtistAttrList[4]));
            }
        }

        public static List<List<Artist>> ParseArtistTxtToArtistGroupList(string artistTxt) {
            List<List<Artist>> artistClassGroupList = new List<List<Artist>>();
            string[] artistGroupList = artistTxt.Split(new string[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < artistGroupList.Length; i++) {
                List<Artist> artistList = new List<Artist>();
                string[] artistGroup = artistGroupList[i].Split('|');
                for (int j = 0; j < artistGroup.Length; j++) {
                    artistList.Add(ParseArtistTagToArtist(artistGroup[j]));
                }
                artistClassGroupList.Add(artistList);
            }
            return artistClassGroupList;
        }

        private static List<List<Artist>> AddArtistModify(List<List<Artist>> artistsGroupList) {
            for (int i = 0; i < artistsGroupList.Count; i++) {
                for (int j = 0; j < artistsGroupList[i].Count; j++) {
                    artistsGroupList[i][j].ArtistName = "artist:" + artistsGroupList[i][j].ArtistName;
                }
            }
            return artistsGroupList;
        }
        private static List<List<Artist>> GetRandomArtistGroupList(List<List<Artist>> artistsGroupList, int artistNum) {
            Random rand = new Random();
            for (int i = artistsGroupList.Count - 1; i >= 1; i--) {
                int j = rand.Next(i + 1);
                List<Artist> temp = artistsGroupList[i];
                artistsGroupList[i] = artistsGroupList[j];
                artistsGroupList[j] = temp;
            }

            List<List<Artist>> randomArtistGroupList = artistsGroupList.Take(artistNum).ToList();
            return randomArtistGroupList;
        }

        public static string GetArtistPrompt(List<List<Artist>> artistsGroupList, int defaultWeightReduceMax, int defaultWeightIncreaseMax, bool isArtistModify, int minArtist, int maxArtist) {
            if (maxArtist < minArtist) { throw new Exception("抽取画师数量的的最大值必须大于或等于最小值"); }
            if (maxArtist > artistsGroupList.Count) { throw new Exception("抽取画师数量最大值大于画师组内画师的数量"); }
            if (minArtist > artistsGroupList.Count) { throw new Exception("抽取画师数量最小值大于画师组内画师的数量"); }

            if (isArtistModify) { artistsGroupList = AddArtistModify(artistsGroupList); }

            Random random = new Random();

            int artistNum = random.Next(minArtist, maxArtist);
            List<List<Artist>> randomArtistGroupList = GetRandomArtistGroupList(artistsGroupList, artistNum);
            List<string> resultArtistList = new List<string>();
            for (int i = 0; i < randomArtistGroupList.Count; i++) {
                for (int j = 0; j < randomArtistGroupList[i].Count; j++) {
                    Artist artist = randomArtistGroupList[i][j];
                    string resultArtist = artist.ArtistName;
                    if ((artist.WeightReduceMax != null && artist.WeightReduceMax >= 0) || (artist.WeightIncreaseMin != null && artist.WeightIncreaseMin >= 0) || (artist.WeightReduceMax == null && defaultWeightReduceMax >= 0) || (artist.WeightIncreaseMax == null && defaultWeightIncreaseMax >= 0)) {
                        if (random.Next(0, 2) == 0 && ((artist.WeightReduceMax != null && artist.WeightReduceMax > 0) || (artist.WeightReduceMax == null && defaultWeightReduceMax > 0))) {
                            int reduceMax = artist.WeightReduceMax == null ? defaultWeightReduceMax : artist.WeightReduceMax.Value;
                            int reduceMin = artist.WeightReduceMin == null ? 0 : artist.WeightReduceMin.Value;
                            for (int k = 0; k < random.Next(reduceMin, reduceMax) + 1; k++) {
                                resultArtist = "[" + resultArtist + "]";
                            }
                        }
                        else if (random.Next(0, 2) == 1 && ((artist.WeightIncreaseMax != null && artist.WeightIncreaseMax > 0) || (artist.WeightIncreaseMax == null && defaultWeightIncreaseMax > 0))) {
                            int reduceMax = artist.WeightIncreaseMin == null ? defaultWeightIncreaseMax : artist.WeightIncreaseMin.Value;
                            int reduceMin = artist.WeightIncreaseMin == null ? 0 : artist.WeightIncreaseMin.Value;
                            for (int k = 0; k < random.Next(reduceMin, reduceMax) + 1; k++) {
                                resultArtist = "{" + resultArtist + "}";
                            }
                        }
                    }
                    resultArtistList.Add(resultArtist);
                }
            }
            return string.Join(",", resultArtistList);
        }
    }
}
