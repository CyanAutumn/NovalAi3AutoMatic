using AutoNai3Tools.utils;
using AutoNai3Tools.artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagRandomArtist : TagBase {
        public string text { get; set; }
        public bool pickRandom { get; set; }
        Form1 form;

        public TagRandomArtist(string tag, Form1 form, string originalTag) {
            this.text = tag;
            this.form = form;
            this.originalTag = originalTag;
        }

        protected override bool KeepText() {
            return form.chkKeepRandomArtist.Checked && (form.runNum % form.numKeepParams.Value) != 0;
        }

        protected override string ParseResultText() {
            List<List<Artist>> artistGroupList = ArtistTools.ParseArtistTxtToArtistGroupList(form.txtArtistRandom.Text);
            string randomArtist = ArtistTools.GetArtistPrompt(artistGroupList, ((int)form.numDefaultArtistWeightReduceMax.Value), ((int)form.numDefaultArtistWeightIncreaseMax.Value), form.chkArtistModify.Checked, ((int)form.numArtistMin.Value), ((int)form.numArtistMax.Value));
            form.PrintLog($"<随机画师>:{randomArtist}" );
            return randomArtist;
        }
    }
}
