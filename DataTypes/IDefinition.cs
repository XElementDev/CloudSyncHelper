﻿using System.Collections.Generic;

namespace XElement.CloudSyncHelper.DataTypes
{
    public interface IDefinition
    {
        IEnumerable<IOsConfigurationInfo> OsConfigs { get; }

        bool SupportsSteamCloud { get; }
    }
}
