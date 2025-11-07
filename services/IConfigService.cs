using System.Collections.Generic;
using AutoNai3Tools.utils;

namespace AutoNai3Tools.Services {
    internal interface IConfigService {
        string PresetFolderPath { get; }
        string AutoSavePresetName { get; }
        IReadOnlyList<string> GetPresetNames();
        void SavePreset(string name, PresetConfigData data);
        PresetConfigData LoadPreset(string name);
        void DeletePreset(string name);
        void SaveAutoPreset(PresetConfigData data);
        PresetConfigData LoadAutoPreset();
        void SaveSystemConfig(SystemConfigData data);
        SystemConfigData LoadSystemConfig();
    }
}
