using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace LiteDevelop.Framework.FileSystem.Net
{
    /// <summary>
    /// Detects which versions of the .NET Framework are installed.
    /// </summary>
    public static class FrameworkDetector
    {
        private static FrameworkVersion[] _versions;

        /// <summary>
        /// Determines which versions are installed according to the registry.
        /// </summary>
        /// <returns>An array of .NET Framework versions.</returns>
        public static FrameworkVersion[] GetInstalledVersions()
        {
            if (_versions != null)
                return _versions;

            var versions = new List<FrameworkVersion>();

            var frameworkSetupNDPNode = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            if (frameworkSetupNDPNode != null)
            {
                foreach (var keyName in frameworkSetupNDPNode.GetSubKeyNames())
                {
                    if (keyName[0] == 'v')
                    {
                        var key = frameworkSetupNDPNode.OpenSubKey(keyName);

                        if (key != null)
                        {

                            // key "v4" has a slightly different structure than the previous ones.
                            if (keyName == "v4")
                            {
                                // required by LiteDevelop, so is always installed.
                                versions.Add(new FrameworkVersion(new Version(4, 0), null, FrameworkInstallationType.ClientProfile));
                                versions.Add(new FrameworkVersion(new Version(4, 0), null, FrameworkInstallationType.Full));

                                foreach (var subKeyName in key.GetSubKeyNames())
                                {
                                    var subKey = key.OpenSubKey(subKeyName);

                                    if (subKey != null)
                                    {
                                        IEnumerable<FrameworkVersion> frameworkVersions;

                                        // check if 4.5 and/or 4.5.1 is installed.
                                        if (TryGetFramework45VersionsFromKey(subKey, out frameworkVersions))
                                        {
                                            versions.AddRange(frameworkVersions);
                                            break;
                                        }

                                        subKey.Close();
                                    }
                                }
                            }
                            else
                            {
                                // get 2.0 -> 3.5

                                FrameworkVersion frameworkVersion = null;

                                if (TryGetFrameworkVersionFromKey(key, out frameworkVersion))
                                {
                                    versions.Add(frameworkVersion);
                                }
                            }

                            key.Close();
                        }
                    }
                }

                frameworkSetupNDPNode.Close();
            }

            return _versions = versions.ToArray();
        }

        private static bool TryGetFrameworkVersionFromKey(RegistryKey key, out FrameworkVersion frameworkVersion)
        {
            frameworkVersion = null;

            bool isInstalled;
            if (TryGetIsInstalled(key, out isInstalled) && isInstalled)
            {
                Version version;
                if (!TryGetVersionValue(key, out version))
                {
                    return false;
                }

                int? servicePack = null;
                int servicePackRaw = 0;
                if (TryGetServicePackValue(key, out servicePackRaw))
                {
                    servicePack = servicePackRaw;
                }

                frameworkVersion = new FrameworkVersion(
                    version, 
                    servicePack, 
                    key.Name == "Client" ? FrameworkInstallationType.ClientProfile : FrameworkInstallationType.Full);
                return true;
            }

            return false;
        }

        private static bool TryGetFramework45VersionsFromKey(RegistryKey key, out IEnumerable<FrameworkVersion> frameworkVersions)
        {
            var versions = new List<FrameworkVersion>();
            frameworkVersions = versions;

            int releaseValue;
            if (TryGetReleaseValue(key, out releaseValue))
            {
                if (releaseValue >= 378389)
                    versions.Add(new FrameworkVersion(new Version(4, 5), null, FrameworkInstallationType.Full));

                if (releaseValue >= 378758)
                    versions.Add(new FrameworkVersion(new Version(4, 5, 1), null, FrameworkInstallationType.Full));
            }

            return versions.Count != 0;        
        }

        private static bool TryGetIsInstalled(RegistryKey key, out bool isInstalled)
        {
            isInstalled = false;

            var installValue = key.GetValue("Install");
            if (installValue != null)
            {
                isInstalled = Convert.ToBoolean(installValue);
                return true;
            }

            return false;
        }

        private static bool TryGetVersionValue(RegistryKey key, out Version version)
        {
            version = default(Version);

            var versionValue = key.GetValue("Version");
            if (versionValue != null)
            {
                if (Version.TryParse(versionValue.ToString(), out version))
                {
                    version = new Version(version.Major, version.Minor);
                    return true;
                }
            }

            return false;
        }

        private static bool TryGetServicePackValue(RegistryKey key, out int servicePack)
        {
            servicePack = 0;

            var servicePackObject = key.GetValue("SP");
            if (servicePackObject != null)
            {
                servicePack = (int)servicePackObject;
                return true;
            }
            
            return false;
        }

        private static bool TryGetReleaseValue(RegistryKey key, out int releaseValue)
        {
            releaseValue = 0;

            var releaseValueObject = key.GetValue("Release");
            if (releaseValueObject != null)
            {
                releaseValue = (int)releaseValueObject;
                return true;
            }

            return false;
        }
        
    }
}
