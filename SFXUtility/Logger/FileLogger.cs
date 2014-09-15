namespace SFXUtility.Logger
{
    #region

    using System.IO;
    using System.Text;

    #endregion

    internal class FileLogger : ILogger
    {
        #region Fields

        private readonly StreamWriter _streamWriter;

        #endregion

        #region Constructors

        public FileLogger(string file)
        {
            File = file;
            _streamWriter = new StreamWriter(file, true, Encoding.UTF8);
        }

        #endregion

        #region Properties

        public string File { get; set; }
        public string Prefix { get; set; }

        #endregion

        #region Methods

        public void Dispose()
        {
            _streamWriter.Dispose();
        }

        public void Write(string message)
        {
            _streamWriter.Write(string.IsNullOrWhiteSpace(Prefix) ? message : string.Format("{0}: {1}", Prefix, message));
        }

        public void WriteBlock(string header, string message)
        {
            _streamWriter.WriteLine(string.IsNullOrWhiteSpace(Prefix)
                ? header
                : string.Format("{0}: {1}", Prefix, header));
            _streamWriter.WriteLine("--------------------");
            _streamWriter.WriteLine(message);
            _streamWriter.WriteLine("--------------------");
            _streamWriter.WriteLine(string.Empty);
        }

        public void WriteLine(string message)
        {
            _streamWriter.WriteLine(string.IsNullOrWhiteSpace(Prefix)
                ? message
                : string.Format("{0}: {1}", Prefix, message));
        }

        #endregion
    }
}