﻿using Microsoft.Practices.Prism.Events;
using System.ComponentModel.Composition;
using XElement.CloudSyncHelper.UI.Win32.DataTypes;

namespace XElement.CloudSyncHelper.UI.Win32.Model
{
#region not unit-tested
    [Export]
    internal class ProgramViewModelFactory : IFactory<ProgramViewModel>
    {
        public ProgramViewModel /*IFactory<T>.*/Get()
        {
            return new ProgramViewModel( this._eventAggregator, this._iconRetrieverModel );
        }

        [Import]
        private IEventAggregator _eventAggregator = null;

        [Import]
        private IconRetrieverModel _iconRetrieverModel = null;
    }
#endregion
}