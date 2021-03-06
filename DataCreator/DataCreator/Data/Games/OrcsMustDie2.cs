﻿using System.ComponentModel.Composition;

namespace XElement.CloudSyncHelper.DataCreator.Data.Games
{
    [Export( typeof( AbstractGameInfo ) )]
    internal class OrcsMustDie2 : AbstractGameInfo
    {
        [ImportingConstructor]
        public OrcsMustDie2() : base( "41128DF6-5E43-45A1-9DA3-70EE2760FCFE" )
        {
            this.ApplicationName = "Orcs Must Die! 2";
            this.FolderName = "Orcs Must Die 2012 [Orcs Must Die! 2]";
            this.TechnicalNameMatcher = this.ApplicationName;
            return;
        }


        protected override void OnImportsSatisfied()
        {
            this.DefinitionInfo = this._definitionFactory.GetSteamCloud();
            return;
        }
    }
}
