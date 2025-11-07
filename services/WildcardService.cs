using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoNai3Tools.utils;

namespace AutoNai3Tools.Services {
    internal sealed class WildcardService : IWildcardService {
        public IReadOnlyList<WildcardSnippet> LoadSnippets(string folderPath) {
            string resolvedFolder = RequireFolder(folderPath);
            if (!Directory.Exists(resolvedFolder))
                throw new DirectoryNotFoundException($"找不到 wildcard 目录：{resolvedFolder}");

            return Directory.GetFiles(resolvedFolder, "*.txt")
                .OrderBy(file => file, StringComparer.OrdinalIgnoreCase)
                .Select(file => new WildcardSnippet(Path.GetFileName(file), File.ReadAllText(file)))
                .ToList();
        }

        public WildcardSnippet AddSnippet(string folderPath, string name, string content) {
            string resolvedFolder = RequireFolder(folderPath);
            string normalizedName = NormalizeName(name);
            Tools.IsExist(resolvedFolder, true);
            string filePath = Path.Combine(resolvedFolder, normalizedName);
            if (File.Exists(filePath))
                throw new InvalidOperationException($"片段“{normalizedName}”已存在");

            File.WriteAllText(filePath, content ?? string.Empty);
            return new WildcardSnippet(normalizedName, content ?? string.Empty);
        }

        public WildcardSnippet UpdateSnippet(string folderPath, string name, string content) {
            string resolvedFolder = RequireFolder(folderPath);
            string normalizedName = NormalizeName(name);
            string filePath = Path.Combine(resolvedFolder, normalizedName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("未找到要更新的片段", filePath);

            File.WriteAllText(filePath, content ?? string.Empty);
            return new WildcardSnippet(normalizedName, content ?? string.Empty);
        }

        public void DeleteSnippet(string folderPath, string name) {
            string resolvedFolder = RequireFolder(folderPath);
            string normalizedName = NormalizeName(name);
            string filePath = Path.Combine(resolvedFolder, normalizedName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private static string RequireFolder(string folderPath) {
            if (string.IsNullOrWhiteSpace(folderPath))
                throw new ArgumentException("Wildcard 目录不能为空", nameof(folderPath));
            return folderPath;
        }

        private static string NormalizeName(string name) {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("片段名不能为空", nameof(name));

            string trimmed = name.Trim();
            if (!trimmed.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                trimmed += ".txt";
            return trimmed;
        }
    }
}
