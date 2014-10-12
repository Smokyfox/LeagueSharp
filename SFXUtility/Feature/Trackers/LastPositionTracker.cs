#region License

/*
 Copyright 2014 - 2014 Nikita Bernthaler
 LastPositionTracker.cs is part of SFXUtility.
 
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
    #region

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Class;
    using IoCContainer;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Properties;
    using SharpDX;

    #endregion

    internal class LastPositionTracker : Base
    {
        #region Fields

        private readonly List<LastPositionStruct> _enemies = new List<LastPositionStruct>();
        private Trackers _trackers;

        #endregion

        #region Constructors

        public LastPositionTracker(IContainer container)
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
                return _trackers != null && _trackers.Menu != null &&
                       _trackers.Menu.Item(_trackers.Name + "Enabled").GetValue<bool>() && Menu != null &&
                       Menu.Item(Name + "Enabled").GetValue<bool>();
            }
        }

        public override string Name
        {
            get { return "Last Position"; }
        }

        #endregion

        #region Methods

        private void OnGameLoad(EventArgs args)
        {
            try
            {
                Logger.Prefix = string.Format("{0} - {1}", BaseName, Name);

                if (IoC.IsRegistered<Trackers>() && IoC.Resolve<Trackers>().Initialized)
                {
                    TrackersLoaded(IoC.Resolve<Trackers>());
                }
                else
                {
                    if (IoC.IsRegistered<Mediator>())
                    {
                        IoC.Resolve<Mediator>().Register("Trackers_initialized", TrackersLoaded);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        private void TrackersLoaded(object o)
        {
            try
            {
                if (o is Trackers && (o as Trackers).Menu != null)
                {
                    _trackers = (o as Trackers);

                    Menu = new Menu(Name, Name);

                    var eMenuItem = new MenuItem(Name + "Enabled", "Enabled").SetValue(false);
                    eMenuItem.ValueChanged +=
                        (sender, args) => _enemies.ForEach(enemy => enemy.Active = args.GetNewValue<bool>());

                    Menu.AddItem(eMenuItem);

                    _trackers.Menu.AddSubMenu(Menu);

                    foreach (
                        Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValid && hero.IsEnemy)
                        )
                    {
                        _enemies.Add(new LastPositionStruct(hero) {Active = Enabled});
                    }

                    Initialized = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        #endregion

        #region Nested Types

        private class LastPositionStruct
        {
            #region Fields

            private readonly Obj_AI_Hero _hero;
            private readonly Render.Sprite _sprite;
            private bool _active;
            private bool _added;

            #endregion

            #region Constructors

            public LastPositionStruct(Obj_AI_Hero hero)
            {
                _hero = hero;
                var mPos = Drawing.WorldToMinimap(hero.Position);
                _sprite =
                    new Render.Sprite(
                        (Bitmap) Resources.ResourceManager.GetObject(string.Format("LP_{0}", hero.ChampionName)),
                        new Vector2(mPos.X, mPos.Y))
                    {
                        VisibleCondition = delegate { return Active && !_hero.IsVisible && !_hero.IsDead; },
                        PositionUpdate = delegate
                        {
                            var pos = Drawing.WorldToMinimap(hero.Position);
                            return new Vector2(pos.X - (_sprite.Size.X/2), pos.Y - (_sprite.Size.Y/2));
                        }
                    };
            }

            #endregion

            #region Properties

            public bool Active
            {
                private get { return _active; }
                set
                {
                    _active = value;
                    Update();
                }
            }

            #endregion

            #region Methods

            private void Update()
            {
                if (_sprite == null)
                    return;

                if (_active && !_added)
                {
                    _sprite.Add(0);
                    _added = true;
                }
                else if (!_active && _added)
                {
                    _sprite.Remove();
                    _added = false;
                }
            }

            #endregion
        }

        #endregion
    }
}