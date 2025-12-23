using System;
using System.ComponentModel;
using System.Globalization;

namespace AutoNai3Tools.utils {
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizedDisplayNameAttribute : DisplayNameAttribute {
        private readonly string resourceKey;

        public LocalizedDisplayNameAttribute(string resourceKey) : base(resourceKey) {
            this.resourceKey = resourceKey;
        }

        public override string DisplayName =>
            Properties.Resources.ResourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture) ?? base.DisplayName;
    }

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizedCategoryAttribute : CategoryAttribute {
        public LocalizedCategoryAttribute(string resourceKey) : base(resourceKey) {
        }

        protected override string GetLocalizedString(string value) {
            return Properties.Resources.ResourceManager.GetString(value, CultureInfo.CurrentUICulture) ?? value;
        }
    }
}
