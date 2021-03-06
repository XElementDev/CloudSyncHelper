﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using XElement.CloudSyncHelper.Serialization.DataTypes;
using XElement.DotNet.System.Environment;

namespace XElement.CloudSyncHelper.DataCreator.Data.Tools
{
    [Export( typeof( AbstractToolInfo ) )]
    internal class TeamSpeak3 : AbstractToolInfo
    {
        [ImportingConstructor]
        public TeamSpeak3() : base( "BF5B72BC-6351-481D-A25D-265BF9F063CB" )
        {
            this.ApplicationName = "TeamSpeak 3";
            this.FolderName = "TeamSpeak 3";
            this.TechnicalNameMatcher = "TeamSpeak 3 Client";
        }

        private List<AbstractLinkInfo> GetLinksForWin81_10()
        {
            return new List<AbstractLinkInfo>
            {
                new FileLinkInfo
                {
                    DestinationRoot = Environment.SpecialFolder.ApplicationData,
                    DestinationSubFolderPath = Path.Combine("TS3Client"),
                    DestinationTargetName = "settings.db",
                    SourceId = "settings.db"
                }
            };
        }

        protected override void OnImportsSatisfied()
        {
            var osConfigs = new List<OsConfigurationInfo>
            {
                this._osConfigFactory.Get( this.GetLinksForWin81_10(), OsId.Win81 ),    // TODO: Check config for Win8.1
                this._osConfigFactory.Get( this.GetLinksForWin81_10(), OsId.Win10 ), 
            };
            this.DefinitionInfo = this._definitionFactory.Get( osConfigs );
        }
    }
}
