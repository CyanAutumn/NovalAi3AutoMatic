using System;
using System.Collections.Generic;
using AutoNai3Tools.utils;

namespace AutoNai3Tools.Services {
    internal sealed class ConfigService : IConfigService {
        private readonly PresetConfigRepository presetRepository;
        private readonly SystemConfigRepository systemRepository;

        public ConfigService()
            : this(new PresetConfigRepository(), new SystemConfigRepository()) {
        }

        public ConfigService(PresetConfigRepository presetRepository, SystemConfigRepository systemRepository) {
            this.presetRepository = presetRepository ?? throw new ArgumentNullException(nameof(presetRepository));
            this.systemRepository = systemRepository ?? throw new ArgumentNullException(nameof(systemRepository));
        }

        public string PresetFolderPath => presetRepository.FolderPath;
        public string AutoSavePresetName => "上一次关闭时的自动保存";

        public IReadOnlyList<string> GetPresetNames() => presetRepository.ListPresetNames();

        public void SavePreset(string name, PresetConfigData data) => presetRepository.Save(name, data);

        public PresetConfigData LoadPreset(string name) => presetRepository.Load(name);

        public void DeletePreset(string name) => presetRepository.Delete(name);

        public void SaveAutoPreset(PresetConfigData data) => SavePreset(AutoSavePresetName, data);

        public PresetConfigData LoadAutoPreset() => LoadPreset(AutoSavePresetName);

        public void SaveSystemConfig(SystemConfigData data) => systemRepository.Save(data);

        public SystemConfigData LoadSystemConfig() => systemRepository.Load();
    }
}
