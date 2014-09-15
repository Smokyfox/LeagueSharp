namespace SFXUtility.Class
{
    #region

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using LeagueSharp;
    using LeagueSharp.Common;
    using SharpDX;

    #endregion

    internal static class Utilities
    {
        #region Methods

        public static void AddItems(this Menu menu, List<MenuItem> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                menu.AddItem(menuItem);
            }
        }

        public static float HealthPercentage(this Obj_AI_Hero hero)
        {
            return hero.Health*100/hero.MaxHealth;
        }

        public static string HexConverter(Color color)
        {
            return string.Format("#{0}", color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2"));
        }

        public static bool IsBuffActive(this Obj_AI_Hero hero, string buffName)
        {
            return hero.Buffs.Any(buff => buff.IsActive && buff.Name == buffName);
        }

        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open))
                {
                }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }

        public static bool IsOnScreen(this Obj_AI_Base obj)
        {
            Vector2 screen = Drawing.WorldToScreen(obj.Position);
            return !(screen.X < 0) && !(screen.X > Drawing.Width) && !(screen.Y < 0) && !(screen.Y > Drawing.Height);
        }

        public static bool IsOnScreen(this Obj_AI_Base obj, float radius)
        {
            Vector2 screen = Drawing.WorldToScreen(obj.Position);
            return !(screen.X + radius < 0) && !(screen.X - radius > Drawing.Width) && !(screen.Y + radius < 0) &&
                   !(screen.Y - radius > Drawing.Height);
        }

        public static float ManaPercentage(this Obj_AI_Hero hero)
        {
            return hero.Mana*100/hero.MaxMana;
        }

        #endregion
    }
}