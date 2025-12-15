using AutoNai3Tools.utils;
using AutoNai3Tools.artist;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.artist {
    internal class ArtistTools {
        private static bool IsDoubleColonWeightedTag(string tag) {
            if (string.IsNullOrWhiteSpace(tag) || !tag.EndsWith("::", StringComparison.Ordinal))
                return false;

            int firstSeparator = tag.IndexOf("::", StringComparison.Ordinal);
            if (firstSeparator <= 0)
                return false;

            string prefix = tag.Substring(0, firstSeparator);
            return double.TryParse(prefix, NumberStyles.Float, CultureInfo.InvariantCulture, out _);
        }

        private static double RandomDoubleColonWeight(double maxWeight, Random random) {
            if (maxWeight <= 0)
                return 0;

            if (maxWeight <= 1)
                return Math.Round(maxWeight, 2);

            return Math.Round(1 + (random.NextDouble() * (maxWeight - 1)), 2);
        }

        private static string ApplyDoubleColonWeighting(string tag, Artist artist, double defaultWeightReduceMax, double defaultWeightIncreaseMax, Random random) {
            if (IsDoubleColonWeightedTag(tag))
                return tag;

            bool hasCustomWeight = artist.WeightReduceMax != null || artist.WeightIncreaseMax != null;

            double weight = 0;
            if (hasCustomWeight) {
                int reduceMin = Math.Max(1, artist.WeightReduceMin ?? 1);
                int reduceMax = artist.WeightReduceMax ?? 0;
                int increaseMin = Math.Max(1, artist.WeightIncreaseMin ?? 1);
                int increaseMax = artist.WeightIncreaseMax ?? 0;

                bool canReduce = reduceMax > 0;
                bool canIncrease = increaseMax > 0;

                if (canReduce && canIncrease) {
                    if (random.Next(0, 2) == 0) {
                        weight = -random.Next(reduceMin, reduceMax + 1);
                    }
                    else {
                        weight = random.Next(increaseMin, increaseMax + 1);
                    }
                }
                else if (canReduce) {
                    weight = -random.Next(reduceMin, reduceMax + 1);
                }
                else if (canIncrease) {
                    weight = random.Next(increaseMin, increaseMax + 1);
                }
            }
            else {
                bool canReduce = defaultWeightReduceMax > 0;
                bool canIncrease = defaultWeightIncreaseMax > 0;

                if (canReduce && canIncrease) {
                    if (random.Next(0, 2) == 0) {
                        weight = -RandomDoubleColonWeight(defaultWeightReduceMax, random);
                    }
                    else {
                        weight = RandomDoubleColonWeight(defaultWeightIncreaseMax, random);
                    }
                }
                else if (canReduce) {
                    weight = -RandomDoubleColonWeight(defaultWeightReduceMax, random);
                }
                else if (canIncrease) {
                    weight = RandomDoubleColonWeight(defaultWeightIncreaseMax, random);
                }
            }

            if (Math.Abs(weight) < 0.00001)
                weight = 0;

            return weight.ToString("0.##", CultureInfo.InvariantCulture) + "::" + tag;
        }

        private static Artist ParseArtistTagToArtist(string artistTag) {
            string[] strArtistAttrList = artistTag.Split(',');
            if (strArtistAttrList.Length != 1 && strArtistAttrList.Length != 5) {
                throw new Exception(artistTag + " 错误，请检查格式");
            }
            string artistName = strArtistAttrList[0].Trim();
            if (strArtistAttrList.Length == 1) {
                return new Artist(artistName, null, null, null, null);
            }
            else {
                return new Artist(artistName, int.Parse(strArtistAttrList[1]), int.Parse(strArtistAttrList[2]), int.Parse(strArtistAttrList[3]), int.Parse(strArtistAttrList[4]));
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

        public static string GetArtistPrompt(List<List<Artist>> artistsGroupList, int defaultWeightReduceMax, int defaultWeightIncreaseMax, double defaultWeightReduceDoubleColonMax, double defaultWeightIncreaseDoubleColonMax, bool isArtistModify, int minArtist, int maxArtist) {
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
                    if (resultArtist.EndsWith("::", StringComparison.Ordinal)) {
                        resultArtistList.Add(ApplyDoubleColonWeighting(resultArtist, artist, defaultWeightReduceDoubleColonMax, defaultWeightIncreaseDoubleColonMax, random));
                        continue;
                    }
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
