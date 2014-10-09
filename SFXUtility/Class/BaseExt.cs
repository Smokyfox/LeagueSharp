#region License

/*
 Copyright 2014 - 2014 Nikita Bernthaler
 BaseExt.cs is part of SFXUtility.
 
 SFXUtility is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 SFXUtility is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with SFXUtility. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

namespace SFXUtility.Class
{
    #region

    using System;
    using IoCContainer;
    using LeagueSharp.Common;

    #endregion

    internal abstract class BaseExt : Base
    {
        #region Constructors

        protected BaseExt(IContainer container)
            : base(container)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        #endregion

        #region Properties

        public override bool Enabled
        {
            get
            {
                return Menu != null && Menu.Item("Enabled").GetValue<bool>();
                ;
            }
        }

        #endregion

        #region Methods

        private void OnGameLoad(EventArgs args)
        {
            Logger.Prefix = string.Format("{0} - {1}", BaseName, Name);
            try
            {
                Menu = new Menu(Name, Name);
                if (IoC.IsRegistered<Mediator>())
                {
                    IoC.Resolve<Mediator>().NotifyColleagues(Name + "_loaded", this);
                }
                Menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(true));
                BaseMenu.AddSubMenu(Menu);
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        #endregion
    }
}