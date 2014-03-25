using System;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem.Net
{
    public sealed class FrameworkVersion
    {
        public FrameworkVersion(Version version, int? servicePack, FrameworkInstallationType installationType)
        {
            Version = version;
            ServicePack = servicePack;
            InstallationType = installationType;
        }

        public Version Version
        {
            get;
            private set;
        }

        public int? ServicePack
        {
            get;
            private set;
        }

        public FrameworkInstallationType InstallationType
        {
            get;
            private set;
        }

        public string DisplayName
        {
            get
            {
                string installationType = InstallationType == FrameworkInstallationType.ClientProfile ? " Client Profile" : string.Empty;
                return string.Format(".NET Framework {0}{1}", Version, installationType);
            }
        }

        public string DisplayVersion
        {
            get
            {
                return string.Format("v{0}.{1}{2}", Version.Major, Version.Minor, (Version.Build == -1 ? "" : "." + Version.Build));
            }
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is FrameworkVersion)
                return Equals(obj as FrameworkVersion);
            return false;
        }

        public bool Equals(FrameworkVersion version)
        {
            return this.Version == version.Version && ServicePack == version.ServicePack && InstallationType == version.InstallationType;
        }

        public static bool operator ==(FrameworkVersion a, FrameworkVersion b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(FrameworkVersion a, FrameworkVersion b)
        {
            return !a.Equals(b);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Version.GetHashCode() ^ InstallationType.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return DisplayName;
        }
    }
}
