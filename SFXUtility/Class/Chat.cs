namespace SFXUtility.Class
{
    #region

    using LeagueSharp;

    #endregion

    public class Chat
    {
        #region Fields

        public const string DefaultColor = "#F7A100";

        #endregion

        #region Methods

        public static void Print(string message, string hexColor = DefaultColor)
        {
            Game.PrintChat(string.Format("<font color='{0}'>{1}</font>", hexColor, message));
        }

        #endregion
    }
}