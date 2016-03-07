﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using XElement.CloudSyncHelper.Serialization.DataTypes;
using XElement.DotNet.System.Environment;

namespace XElement.CloudSyncHelper.DataCreator.Data.Tools
{
    [Export( typeof( AbstractToolInfo ) )]
    internal class ExactAudioCopy : AbstractToolInfo
    {
        [ImportingConstructor]
        public ExactAudioCopy() : base( "0408347B-5D6B-4561-88F0-06F3F379232B" )
        {
            this.ApplicationName = "Exact Audio Copy";
            this.FolderName = "Exact Audio Copy";
            this.TechnicalNameMatcher = "Exact Audio Copy.*";
        }

        private OsConfiguration GetConfigForWin8_1()
        {
            return new OsConfiguration { Links = this.GetLinksForWin81_10(), OsId = OsId.Win81 };
        }

        private OsConfiguration GetConfigForWin10()
        {
            return new OsConfiguration { Links = this.GetLinksForWin81_10(), OsId = OsId.Win10 };
        }

        private List<AbstractLinkInfo> GetLinksForWin81_10()
        {
            return new List<AbstractLinkInfo>
            {
                new FolderLinkInfo
                {
                    DestinationRoot = Environment.SpecialFolder.ApplicationData,
                    DestinationSubFolderPath = Path.Combine("EAC"),
                    DestinationTargetName = "Profiles",
                    SourceId = "Profiles"
                }
            };
        }

        protected override void OnImportsSatisfied()
        {
            var osConfig = new List<OsConfiguration>
            {
                this.GetConfigForWin8_1(),
                this.GetConfigForWin10()
            };
            var config = this._configFactory.Get( osConfig );
            this.Definition = this._definitionFactory.Get( config );
        }
    }
}