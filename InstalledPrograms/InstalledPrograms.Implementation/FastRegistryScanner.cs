﻿using Microsoft.Win32;
using System.Collections.Generic;

namespace XElement.CloudSyncHelper.InstalledPrograms
{
#region not unit-tested
    //  --> Based on: http://www.codeproject.com/Tips/782919/Get-List-of-Installed-Applications-of-System-in-Cs
    //      Visited: 2015-08-02
    public class FastRegistryScanner : IScanner
    {
        public IEnumerable<IInstalledProgram> /*IScanner.*/GetInstalledPrograms()
        {
            IList<InstalledProgram> result = new List<InstalledProgram>();

            using ( RegistryKey regKey = Registry.LocalMachine.OpenSubKey( UNINSTALL_KEY ) )
            {
                foreach ( string subKeyName in regKey.GetSubKeyNames() )
                {
                    using ( RegistryKey subKey = regKey.OpenSubKey( subKeyName ) )
                    {
                        if ( subKey.GetValueNames().Length != 0 )
                        {
                            var installedApplication = new InstalledProgram();
                            installedApplication.DisplayName = subKey.GetValueOrDefault<string>( "DisplayName" );
                            installedApplication.EstimatedSize = subKey.GetValue( "EstimatedSize" );

                            result.Add( installedApplication );
                        }
                    }
                }
            }

            return result;
        }

        private const string UNINSTALL_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
    }
#endregion
}