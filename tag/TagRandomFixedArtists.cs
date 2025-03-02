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
        Form1 form;

        public TagRandomFixedArtists(string tag, Form1 form, string originalTag) {
            this.text = tag;
            this.form = form;
            this.originalTag = originalTag;
        }

        protected override bool KeepText() {
            return form.chkKeepRandomArtist.Checked && (form.runNum % form.numKeepParams.Value) != 0;
        }

        protected override string ParseResultText() {
            List<List<Artist>> artistGroupList = ArtistTools.ParseArtistTxtToArtistGroupList(form.txtArtistRandomFixed.Text);
            string randomArtist = ArtistTools.GetArtistPromptFixed(artistGroupList, ((int)form.numDefaultArtistRandomFixedWeightIncreaseMin.Value), ((int)form.numDefaultArtistRandomFixedWeightIncreaseMax.Value), form.chkArtistModifyRandomFixedArtist.Checked);
            Logger.Info($"<固定随机画师>:{randomArtist}");
            return randomArtist;
        }
    }
}