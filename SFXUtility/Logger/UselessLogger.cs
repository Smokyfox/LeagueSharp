namespace SFXUtility.Logger
{
    internal class UselessLogger : ILogger
    {
        #region Properties

        public string Prefix { get; set; }

        #endregion

        #region Methods

        public void Dispose()
        {
        }

        public void Write(string message)
        {
        }

        public void WriteBlock(string header, string message)
        {
        }

        public void WriteLine(string message)
        {
        }

        #endregion
    }
}