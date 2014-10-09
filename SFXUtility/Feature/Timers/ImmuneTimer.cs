#region License

/*
 Copyright 2014 - 2014 Nikita Bernthaler
 ImmuneTimer.cs is part of SFXUtility.
 
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

namespace SFXUtility.Feature
{
    internal class ImmuneTimers
    {
        //public ImmuneTimers(IContainer container)
        //    : base(container)
        //{
        //    CustomEvents.Game.OnGameLoad += OnGameLoad;
        //}

        //public override bool Enabled
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public override string Name
        //{
        //    get { return "Immune"; }
        //}

        //private void OnGameLoad(EventArgs args)
        //{
        //    Logger.Prefix = string.Format("{0} - {1}", BaseName, Name);
        //    try
        //    {
        //        //if (IoC.IsRegistered<Timers>())
        //        //{
        //        //    TimersLoaded(IoC.Resolve<Timers>());
        //        //}
        //        //else
        //        //{
        //        //    if (IoC.IsRegistered<Mediator>())
        //        //    {
        //        //        IoC.Resolve<Mediator>().Register("Timers_loaded", TimersLoaded);
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteBlock(ex.Message, ex.ToString());
        //    }
        //}

        //private void TimersLoaded(object o)
        //{
        //    try
        //    {
        //        if (!(o is Timers)) 
        //            return;

        //        var timers = o as Timers;
        //        if (timers.Menu != null)
        //        {
        //            timers.Menu.AddSubMenu(Menu);

        //            //Add Events
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteBlock(ex.Message, ex.ToString());
        //    }
        //}
    }
}