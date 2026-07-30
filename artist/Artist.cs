using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.utils {
    internal class Artist {
        public string ArtistName { get; set; }
        public int? WeightIncreaseMax { get; set; }
        public int? WeightIncreaseMin { get; set; }
        public int? WeightReduceMax { get; set; }
        public int? WeightReduceMin { get; set; }
        public double? WeightIncreaseMaxDoubleColon { get; set; }
        public double? WeightIncreaseMinDoubleColon { get; set; }
        public double? WeightReduceMaxDoubleColon { get; set; }
        public double? WeightReduceMinDoubleColon { get; set; }

        public Artist(string artistName, int? weightReduceMin, int? weightReduceMax, int? weightIncreaseMin, int? weightIncreaseMax) {
            this.ArtistName = artistName;
            this.WeightReduceMin = weightReduceMin;
            this.WeightReduceMax = weightReduceMax;
            this.WeightIncreaseMin = weightIncreaseMin;
            this.WeightIncreaseMax = weightIncreaseMax;
        }
    }
}
