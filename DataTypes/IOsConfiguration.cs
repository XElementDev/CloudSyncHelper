﻿using System.Collections.Generic;
using XElement.DotNet.System.Environment;

namespace XElement.CloudSyncHelper.DataTypes
{
    public interface IOsConfiguration
    {
        string Author { get; }

        IReadOnlyList<ILinkInfo> Links { get; }

        string Name { get; }

        OsId OsId { get; }
    }
}
