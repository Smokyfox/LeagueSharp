namespace SFXUtility.Logger
{
    #region

    using System;

    #endregion

    internal class ConsoleLogger : ILogger
    {
        #region Constructors

        public ConsoleLogger() : this(ConsoleColor.Yellow)
        {
        }

        public ConsoleLogger(ConsoleColor consoleColor)
        {
            ConsoleColor = consoleColor;
        }

        #endregion

        #region Properties

        public ConsoleColor ConsoleColor { get; set; }
        public string Prefix { get; set; }

        #endregion

        #region Methods

        public void Dispose()
        {
        }

        public void Write(string message)
        {
            Console.ForegroundColor = ConsoleColor;
            Console.Write(string.IsNullOrWhiteSpace(Prefix) ? message : string.Format("{0}: {1}", Prefix, message));
            Console.ResetColor();
        }

        public void WriteBlock(string header, string message)
        {
            Write(string.IsNullOrWhiteSpace(Prefix)
                ? header
                : string.Format("{0}: {1}", Prefix, header) + Environment.NewLine);
            Console.WriteLine("--------------------");
            Write(message + Environment.NewLine);
            Console.WriteLine("--------------------");
            Console.WriteLine(string.Empty);
        }

        public void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }

        #endregion
    }
}