namespace SFXUtility.Class
{
    #region

    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using Logger;

    #endregion

    internal class Updater : IDisposable
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly Uri _updateUrl;
        private readonly Version _version;
        private readonly Uri _versionUrl;

        private readonly WebClient _webClient = new WebClient
        {
            Proxy = null,
            Encoding = Encoding.UTF8,
        };

        private string _backupExtension = ".bak";

        #endregion

        #region Constructors

        public Updater(ILogger logger, Uri versionUrl, Uri updateUrl, Version version)
        {
            _logger = logger;
            _logger.Prefix = "SFXUtility-Updater";
            _versionUrl = versionUrl;
            _updateUrl = updateUrl;
            _version = version;

            var bakPath = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) + _backupExtension;
            if (File.Exists(bakPath) && !Utilities.IsFileLocked(bakPath))
            {
                File.Delete(bakPath);
            }
        }

        #endregion

        #region Properties

        public string BackupExtension
        {
            get { return _backupExtension; }
            set { _backupExtension = value; }
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            if (_webClient.IsBusy)
            {
                _webClient.CancelAsync();
            }
            _webClient.Dispose();
        }

        public async void Update()
        {
            try
            {
                string versionString = await _webClient.DownloadStringTaskAsync(_versionUrl);
                Version version;
                if (Version.TryParse(versionString, out version))
                {
                    if (version > _version)
                    {
                        var asmPath = Assembly.GetExecutingAssembly().Location;
                        File.Move(asmPath, Path.GetFileNameWithoutExtension(asmPath) + _backupExtension);
                        await _webClient.DownloadFileTaskAsync(_updateUrl, asmPath);
                        Chat.Print("SFXUtilities updated, please reload!");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        #endregion
    }
}