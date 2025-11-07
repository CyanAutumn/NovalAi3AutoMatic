using System.Collections.Generic;

namespace AutoNai3Tools.Services {
    internal interface IWildcardService {
        IReadOnlyList<WildcardSnippet> LoadSnippets(string folderPath);
        WildcardSnippet AddSnippet(string folderPath, string name, string content);
        WildcardSnippet UpdateSnippet(string folderPath, string name, string content);
        void DeleteSnippet(string folderPath, string name);
    }
}
