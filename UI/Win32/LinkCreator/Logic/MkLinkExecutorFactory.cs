﻿using XElement.CloudSyncHelper.Logic.Execution.MkLink;

namespace XElement.CloudSyncHelper.UI.Win32.LinkCreator.Logic
{
#region not unit-tested
    internal class MkLinkExecutorFactory : IFactory
    {
        public MkLinkExecutorFactory() { }


        public IExecutor Get( ParametersDTO parameters )
        {
            return new Executor( parameters );
        }
    }
#endregion
}