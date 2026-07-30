using System;

namespace AutoNai3Tools.utils {
    internal static class ApiEndpoint {
        public const string DefaultAlias = "nai";
        public const string DefaultBaseUrl = "https://image.novelai.net";

        public static string ResolveBaseUrl(string api) {
            if (string.IsNullOrWhiteSpace(api))
                return DefaultBaseUrl;

            string trimmed = api.Trim();
            if (string.Equals(trimmed, DefaultAlias, StringComparison.OrdinalIgnoreCase))
                return DefaultBaseUrl;

            if (trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
                return trimmed.TrimEnd('/');
            }

            return ("https://" + trimmed).TrimEnd('/');
        }
    }
}
