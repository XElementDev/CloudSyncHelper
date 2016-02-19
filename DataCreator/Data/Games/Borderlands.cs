﻿using System.ComponentModel.Composition;

namespace XElement.CloudSyncHelper.DataCreator.Data.Games
{
    [Export( typeof( AbstractGameInfo ) )]
    internal class Borderlands : AbstractGameInfo
    {
        [ImportingConstructor]
        public Borderlands() : base( "FA773668-0021-4493-9C3F-2D981C98244E" )
        {
            this.ApplicationName = "Borderlands";
            this.FolderName = "Borderlands 2009 [Borderlands]";
            this.TechnicalNameMatcher = "Borderlands";
        }

        protected override void OnImportsSatisfied()
        {
            this.Configuration = this._configFactory.GetSteamCloud();
        }
    }
}
