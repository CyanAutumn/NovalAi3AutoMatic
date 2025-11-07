using AutoNai3Tools.utils;

namespace AutoNai3Tools.utils {
    internal interface IPromptContext {
        PicProperty PicProps { get; }
        SettingProperty SettingProps { get; }
        int RunNumber { get; }
        int RunKeepParams { get; }
        string ArtistFixedText { get; }
        string ArtistRandomText { get; }
        int DefaultArtistWeightReduceMax { get; }
        int DefaultArtistWeightIncreaseMax { get; }
        bool ArtistModify { get; }
        int ArtistMin { get; }
        int ArtistMax { get; }
        void SetRunNumber(int runNumber);
    }
}
