﻿using XElement.CloudSyncHelper.DataTypes;

namespace XElement.CloudSyncHelper.Logic
{
    public interface IDefinition
    {
        IOsConfigurationInfo BestFittingOsConfigurationInfo { get; }


        bool HasSuitableConfig { get; }


        bool IsInCloud { get; }


        bool IsLinked { get; }
    }
}
