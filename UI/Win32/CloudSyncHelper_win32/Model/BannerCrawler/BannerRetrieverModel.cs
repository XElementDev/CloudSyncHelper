﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Timers;
using XElement.CloudSyncHelper.UI.Win32.Model.BannerCrawler;
using XElement.Common.UI;

namespace XElement.CloudSyncHelper.UI.Win32.Model
{
#region not unit-tested
    [Export]
    public class BannerRetrieverModel : ViewModelBase, IPartImportsSatisfiedNotification
    {
        private void DoRetrieval()
        {
            var cachedBannerFilePaths = Directory.EnumerateFiles( this._config.PathToBannerCache, 
                                                                 "*.*", SearchOption.TopDirectoryOnly );
            this._cachedBannerFilePaths = cachedBannerFilePaths;
        }

        public string GetPathToBanner( IBannerId bannerInformation )
        {
            var pathToBanner = default( string );

            if ( this._cachedBannerFilePaths != null )
            {
                var id = bannerInformation.Id.ToString();
                pathToBanner = this._cachedBannerFilePaths.FirstOrDefault( fileName =>
                {
                    var fileNameWoExtension = Path.GetFileNameWithoutExtension( fileName );
                    return fileNameWoExtension.ToLower() == id.ToLower();
                } );
            }

            return pathToBanner;
        }

        private void InitializeRetrievalBackgroundWorker()
        {
            this._retrievalBackgroundWorker = new BackgroundWorker();
            this._retrievalBackgroundWorker.DoWork += ( s, e ) => this.DoRetrieval();
            this._retrievalBackgroundWorker.RunWorkerCompleted += ( s, e ) =>
            {
                this.RaisePropertyChanged( null );
                this._startRetrievingTimer.Start();
            };
        }

        private void InitializeRetrievingTimer()
        {
            this._startRetrievingTimer = new Timer();
            var fiveSeconds = 5000D;
            this._startRetrievingTimer.Interval = fiveSeconds;
            this._startRetrievingTimer.AutoReset = true;
            this._startRetrievingTimer.Elapsed += ( s, e ) => this.StartRetrievalInBackground();
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            this.DoRetrieval();

            this.InitializeRetrievalBackgroundWorker();
            this.InitializeRetrievingTimer();
            this._startRetrievingTimer.Start();
        }

        private void StartRetrievalInBackground()
        {
            this._startRetrievingTimer.Stop();
            this._retrievalBackgroundWorker.RunWorkerAsync();
        }

        [Import]
        private IConfig _config = null;

        private IEnumerable<string> _cachedBannerFilePaths;
        private BackgroundWorker _retrievalBackgroundWorker;
        private Timer _startRetrievingTimer;
    }
#endregion
}
