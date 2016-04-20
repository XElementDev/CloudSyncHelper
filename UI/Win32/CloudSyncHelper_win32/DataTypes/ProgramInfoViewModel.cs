﻿using System;
using System.Collections.Generic;
using XElement.CloudSyncHelper.DataTypes;
using XElement.CloudSyncHelper.UI.Win32.Model;
using XElement.CloudSyncHelper.UI.Win32.Model.Enrichment;
using UiBannerCrawler = XElement.CloudSyncHelper.UI.Win32.Model.Enrichment.Banners;
using UiIconCrawler = XElement.CloudSyncHelper.UI.Win32.Model.Enrichment.Icons;

namespace XElement.CloudSyncHelper.UI.Win32.DataTypes
{
#region not unit-tested
    public class ProgramInfoViewModel : UiIconCrawler.IIconId, UiBannerCrawler.IObjectToCrawl
    {
        public ProgramInfoViewModel( IApplicationInfo applicationInfo, 
                                     IConfig config, 
                                     ConfigForOsHelper cfg4OsHelper )
        {
            this.ApplicationInfo = applicationInfo;
            this._config = config;
            this._configForOsHelper = cfg4OsHelper;

            InitializeExecutionLogic( applicationInfo );
        }

        internal IApplicationInfo ApplicationInfo { get; private set; }

        string BannerCrawler.ICrawlInformation.ApplicationName { get { return this.DisplayName; } }

        public string DisplayName
        {
            get
            {
                var displayName = String.Empty;
                if ( this.ApplicationInfo != null )
                {
                    displayName = this.ApplicationInfo.ApplicationName;
                }
                return displayName;
            }
        }

        public ExecutionLogic ExecutionLogic { get; private set; }

        public bool HasSuitableConfig
        {
            get
            {
                return this.ExecutionLogic != null &&
                    this.ExecutionLogic.HasSuitableConfig;
            }
        }

        Guid IRetrievalIdContainer.Id /*IBannerId. / IIconId.*/
        {
            get { return this.ApplicationInfo.Id; }
        }

        private void InitializeExecutionLogic( IApplicationInfo appInfo )
        {
            var pathVariables = new PathVariablesDTO
            {
                PathToSyncFolder = this._config.PathToSyncFolder,
                UplayUserName = this._config.UplayAccountName,
                UserName = this._config.UserName
            };
            this.ExecutionLogic = new ExecutionLogic( appInfo, pathVariables, this._configForOsHelper );
        }

        public bool IsInCloud { get { return this.ExecutionLogic.IsInCloud; } }

        public bool IsLinked { get { return this.ExecutionLogic.IsLinked; } }

        public IEnumerable<IOsConfigurationInfo> OsConfigs
        {
            get
            {
                IEnumerable<IOsConfigurationInfo> result = new List<IOsConfigurationInfo>();

                var definition = this.ApplicationInfo.DefinitionInfo;
                if ( definition != default( IDefinitionInfo ) )
                {
                    result = definition.OsConfigs;
                }

                return result;
            }
        }

        public bool SupportsSteamCloud
        {
            get { return this.ApplicationInfo.DefinitionInfo.SupportsSteamCloud; }
        }

        public string TechnicalNameMatcher { get { return this.ApplicationInfo.TechnicalNameMatcher; } }

        private IConfig _config;
        private ConfigForOsHelper _configForOsHelper;
    }
#endregion
}
