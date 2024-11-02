using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.tag {
    internal class TagFixedArtist : TagBase {
        public string text { get; set; }
        Form1 form;

        public TagFixedArtist(string tag, Form1 form, string originalTag) {
            this.text = tag;
            this.form = form;
            this.originalTag = originalTag;
        }

        protected override string ParseResultText() {
            form.log.Info($"<固定画师>:{form.txtArtistFixed.Text}");
            return form.txtArtistFixed.Text;
        }
    }
}
