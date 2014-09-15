namespace SFXUtility.Logger
{
    #region

    using System;

    #endregion

    internal interface ILogger : IDisposable
    {
        #region Properties

        string Prefix { get; set; }

        #endregion

        #region Methods

        void Write(string message);

        void WriteBlock(string header, string message);
        void WriteLine(string message);

        #endregion
    }
}