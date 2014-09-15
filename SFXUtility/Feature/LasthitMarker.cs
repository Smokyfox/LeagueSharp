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

    #endregion

    internal class LasthitMarker : Base
    {
        #region Fields

        private List<Obj_AI_Minion> _killableMinions = new List<Obj_AI_Minion>();
        private Menu _menu;

        #endregion

        #region Constructors

        public LasthitMarker(Container container)
            : base(container)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        #endregion

        #region Properties

        public override bool Enabled
        {
            get { return _menu != null && _menu.Item("Enabled").GetValue<bool>(); }
        }

        public override string Name
        {
            get { return "Lasthit Marker"; }
        }

        #endregion

        #region Methods

        private void OnDraw(EventArgs args)
        {
            if (!Enabled)
                return;
            try
            {
                var color = _menu.Item("DrawingColor").GetValue<Color>();
                var radius = _menu.Item("DrawingRadius").GetValue<Slider>().Value;
                var circleThickness = BaseMenu.Item("MiscCircleThickness").GetValue<Slider>().Value;
                var circleQuality = BaseMenu.Item("MiscCircleQuality").GetValue<Slider>().Value;

                foreach (Obj_AI_Minion minion in _killableMinions)
                {
                    Utility.DrawCircle(minion.Position, minion.BoundingRadius + radius, color, circleThickness,
                        circleQuality);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        private void OnGameLoad(EventArgs args)
        {
            Logger.Prefix = string.Format("{0} - {1}", BaseName, Name);
            try
            {
                _menu = new Menu(Name, Name);

                var drawingMenu = new Menu("Drawing", "Drawing");
                drawingMenu.AddItem(new MenuItem("DrawingColor", "Color").SetValue(Color.Fuchsia));
                drawingMenu.AddItem(new MenuItem("DrawingRadius", "Circle Radius").SetValue(new Slider(30)));

                var distanceMenu = new Menu("Distance", "Distance");
                distanceMenu.AddItem(new MenuItem("DistanceEnabled", "Limit by Distance").SetValue(true));
                distanceMenu.AddItem(
                    new MenuItem("DistanceLimit", "Distance Limit").SetValue(new Slider(1000, 500, 3000)));

                _menu.AddSubMenu(drawingMenu);
                _menu.AddSubMenu(distanceMenu);

                _menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(true));

                BaseMenu.AddSubMenu(_menu);

                Game.OnGameUpdate += OnGameUpdate;
                Drawing.OnDraw += OnDraw;
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }


        private void OnGameUpdate(EventArgs args)
        {
            if (!Enabled)
                return;
            try
            {
                List<Obj_AI_Minion> list = (from minion in ObjectManager.Get<Obj_AI_Minion>()
                    where minion.IsValidTarget() && minion.Health > 1f
                    where
                        !_menu.Item("DistanceEnabled").GetValue<bool>() ||
                        minion.Distance(ObjectManager.Player.Position) <=
                        _menu.Item("DistanceLimit").GetValue<Slider>().Value
                    where
                        minion.Health <=
                        DamageLib.CalcPhysicalMinionDmg(
                            ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod - 1,
                            minion, true)
                    select minion).ToList();
                _killableMinions = list;
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        #endregion
    }
}