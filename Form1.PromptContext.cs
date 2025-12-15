using AutoNai3Tools.utils;

namespace AutoNai3Tools {
    public partial class Form1 : IPromptContext {
        PicProperty IPromptContext.PicProps => picProps;
        SettingProperty IPromptContext.SettingProps => settingProps;
        int IPromptContext.RunNumber => runNum;
        int IPromptContext.RunKeepParams => picProps.RunKeepParams;
        string IPromptContext.ArtistFixedText => txtArtistFixed.Text;
        string IPromptContext.ArtistRandomText => txtArtistRandom.Text;
        int IPromptContext.DefaultArtistWeightReduceMax => (int)numDefaultArtistWeightReduceMax.Value;
        int IPromptContext.DefaultArtistWeightIncreaseMax => (int)numDefaultArtistWeightIncreaseMax.Value;
        double IPromptContext.DefaultArtistWeightReduceDoubleColonMax => (double)numDefaultArtistWeightReduceDoubleColonMax.Value;
        double IPromptContext.DefaultArtistWeightIncreaseDoubleColonMax => (double)numDefaultArtistWeightIncreaseDoubleColonMax.Value;
        bool IPromptContext.ArtistModify => chkArtistModify.Checked;
        int IPromptContext.ArtistMin => (int)numArtistMin.Value;
        int IPromptContext.ArtistMax => (int)numArtistMax.Value;

        void IPromptContext.SetRunNumber(int runNumber) {
            runNum = runNumber;
        }
    }
}
