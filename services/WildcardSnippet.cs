namespace AutoNai3Tools.Services {
    internal sealed class WildcardSnippet {
        public WildcardSnippet(string name, string content) {
            Name = name;
            Content = content;
        }

        public string Name { get; }
        public string Content { get; }
    }
}
