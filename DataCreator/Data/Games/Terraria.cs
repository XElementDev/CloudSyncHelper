﻿using System.ComponentModel.Composition;

namespace XElement.CloudSyncHelper.DataCreator.Data.Games
{
    [Export( typeof( AbstractGameInfo ) )]
    internal class Terraria : AbstractGameInfo
    {
        [ImportingConstructor]
        public Terraria()
        {
            this.DisplayName = "Terraria";
            this.FolderName = "Terraria 2011 [Terraria]";
            this.TechnicalNameMatcher = "Terraria";
        }

        protected override void OnImportsSatisfied()
        {
            this.Configuration = this._configFactory.GetSteamCloud();
        }
    }
}
