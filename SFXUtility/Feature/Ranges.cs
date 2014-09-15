namespace SFXUtility.Feature
{
    #region

    using System;
    using System.Drawing;
    using Class;
    using IoCContainer;
    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class Ranges : Base
    {
        #region Fields

        private const float ExperienceRange = 1400f;
        private const float TurretRange = 900f;

        private Menu _menu;

        #endregion

        #region Constructors

        public Ranges(IContainer container)
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
            get { return "Ranges"; }
        }

        #endregion

        #region Methods

        private void DrawAttack()
        {
            try
            {
                var drawFriendly = _menu.Item("AttackFriendly").GetValue<bool>();
                var drawEnemy = _menu.Item("AttackEnemy").GetValue<bool>();
                var drawSelf = _menu.Item("AttackSelf").GetValue<bool>();

                if (!drawFriendly && !drawEnemy && !drawSelf)
                    return;

                var color = _menu.Item("AttackColor").GetValue<Color>();
                var circleThickness = BaseMenu.Item("MiscCircleThickness").GetValue<Slider>().Value;
                var circleQuality = BaseMenu.Item("MiscCircleQuality").GetValue<Slider>().Value;

                var distanceLimitEnabled = _menu.Item("DistanceEnabled").GetValue<bool>();
                var distanceLimit = _menu.Item("DistanceLimit").GetValue<Slider>().Value;

                foreach (Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>())
                {
                    var radius = hero.BoundingRadius + hero.AttackRange;
                    if (hero.IsValid && !hero.IsDead && hero.IsVisible && hero.IsOnScreen(radius))
                    {
                        if ((hero.IsAlly && drawFriendly || hero.IsMe && drawSelf || hero.IsEnemy && drawEnemy) &&
                            !(hero.IsMe && !drawSelf))
                        {
                            if (!distanceLimitEnabled || hero.Distance(ObjectManager.Player.Position) <= distanceLimit)
                            {
                                Utility.DrawCircle(hero.Position, radius, color, circleThickness, circleQuality);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        private void DrawExperience()
        {
            try
            {
                var drawFriendly = _menu.Item("ExperienceFriendly").GetValue<bool>();
                var drawEnemy = _menu.Item("ExperienceEnemy").GetValue<bool>();
                var drawSelf = _menu.Item("ExperienceSelf").GetValue<bool>();

                if (!drawFriendly && !drawEnemy && !drawSelf)
                    return;

                var color = _menu.Item("AttackColor").GetValue<Color>();
                var circleThickness = BaseMenu.Item("MiscCircleThickness").GetValue<Slider>().Value;
                var circleQuality = BaseMenu.Item("MiscCircleQuality").GetValue<Slider>().Value;

                var distanceLimitEnabled = _menu.Item("DistanceEnabled").GetValue<bool>();
                var distanceLimit = _menu.Item("DistanceLimit").GetValue<Slider>().Value;

                foreach (Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (hero.IsValid && !hero.IsDead && hero.IsVisible && hero.IsOnScreen(ExperienceRange))
                    {
                        if ((hero.IsAlly && drawFriendly || hero.IsMe && drawSelf || hero.IsEnemy && drawEnemy) &&
                            !(hero.IsMe && !drawSelf))
                        {
                            if (!distanceLimitEnabled || hero.Distance(ObjectManager.Player.Position) <= distanceLimit)
                            {
                                Utility.DrawCircle(hero.Position, ExperienceRange, color, circleThickness, circleQuality);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        private void DrawSpell()
        {
            try
            {
                var drawFriendlyQ = _menu.Item("SpellFriendlyQ").GetValue<bool>();
                var drawFriendlyW = _menu.Item("SpellFriendlyW").GetValue<bool>();
                var drawFriendlyE = _menu.Item("SpellFriendlyE").GetValue<bool>();
                var drawFriendlyR = _menu.Item("SpellFriendlyR").GetValue<bool>();
                var drawFriendly = drawFriendlyQ || drawFriendlyW || drawFriendlyE || drawFriendlyR;

                var drawEnemyQ = _menu.Item("SpellEnemyQ").GetValue<bool>();
                var drawEnemyW = _menu.Item("SpellEnemyW").GetValue<bool>();
                var drawEnemyE = _menu.Item("SpellEnemyE").GetValue<bool>();
                var drawEnemyR = _menu.Item("SpellEnemyR").GetValue<bool>();
                var drawEnemy = drawEnemyQ || drawEnemyW || drawEnemyE || drawEnemyR;

                var drawSelfQ = _menu.Item("SpellSelfQ").GetValue<bool>();
                var drawSelfW = _menu.Item("SpellSelfW").GetValue<bool>();
                var drawSelfE = _menu.Item("SpellSelfE").GetValue<bool>();
                var drawSelfR = _menu.Item("SpellSelfR").GetValue<bool>();
                var drawSelf = drawSelfQ || drawSelfW || drawSelfE || drawSelfR;

                if (!drawFriendly && !drawEnemy && !drawSelf)
                    return;

                var circleThickness = BaseMenu.Item("MiscCircleThickness").GetValue<Slider>().Value;
                var circleQuality = BaseMenu.Item("MiscCircleQuality").GetValue<Slider>().Value;

                var distanceLimitEnabled = _menu.Item("DistanceEnabled").GetValue<bool>();
                var distanceLimit = _menu.Item("DistanceLimit").GetValue<Slider>().Value;

                var spellMaxRange = _menu.Item("SpellMaxRange").GetValue<Slider>().Value;

                foreach (Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (hero.IsValid && !hero.IsDead && hero.IsVisible)
                    {
                        if (!distanceLimitEnabled || hero.Distance(ObjectManager.Player.Position) <= distanceLimit)
                        {
                            var color =
                                _menu.Item("Spell" + (hero.IsMe ? "Self" : (hero.IsEnemy ? "Enemy" : "Friendly")) +
                                           "Color").GetValue<Color>();
                            if ((hero.IsAlly && drawFriendlyQ || hero.IsEnemy && drawEnemyQ || hero.IsMe && drawSelfQ) &&
                                !(hero.IsMe && !drawSelfQ))
                            {
                                var range = hero.Spellbook.GetSpell(SpellSlot.Q).SData.CastRange[0];
                                if (range <= spellMaxRange && hero.IsOnScreen(range))
                                    Utility.DrawCircle(hero.Position, range, color, circleThickness, circleQuality);
                            }
                            if ((hero.IsAlly && drawFriendlyW || hero.IsEnemy && drawEnemyW || hero.IsMe && drawSelfW) &&
                                !(hero.IsMe && !drawSelfW))
                            {
                                var range = hero.Spellbook.GetSpell(SpellSlot.W).SData.CastRange[0];
                                if (range <= spellMaxRange && hero.IsOnScreen(range))
                                    Utility.DrawCircle(hero.Position, range, color, circleThickness, circleQuality);
                            }
                            if ((hero.IsAlly && drawFriendlyE || hero.IsEnemy && drawEnemyE || hero.IsMe && drawSelfE) &&
                                !(hero.IsMe && !drawSelfE))
                            {
                                var range = hero.Spellbook.GetSpell(SpellSlot.E).SData.CastRange[0];
                                if (range <= spellMaxRange && hero.IsOnScreen(range))
                                    Utility.DrawCircle(hero.Position, range, color, circleThickness, circleQuality);
                            }
                            if ((hero.IsAlly && drawFriendlyR || hero.IsEnemy && drawEnemyR || hero.IsMe && drawSelfR) &&
                                !(hero.IsMe && !drawSelfR))
                            {
                                var range = hero.Spellbook.GetSpell(SpellSlot.R).SData.CastRange[0];
                                if (range <= spellMaxRange && hero.IsOnScreen(range))
                                    Utility.DrawCircle(hero.Position, range, color, circleThickness, circleQuality);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        private void DrawTurret()
        {
            try
            {
                var drawFriendly = _menu.Item("TurretFriendly").GetValue<bool>();
                var drawEnemy = _menu.Item("TurretEnemy").GetValue<bool>();

                if (!drawFriendly && !drawEnemy)
                    return;

                var circleThickness = BaseMenu.Item("MiscCircleThickness").GetValue<Slider>().Value;
                var circleQuality = BaseMenu.Item("MiscCircleQuality").GetValue<Slider>().Value;

                var distanceLimitEnabled = _menu.Item("DistanceEnabled").GetValue<bool>();
                var distanceLimit = _menu.Item("DistanceLimit").GetValue<Slider>().Value;

                foreach (Obj_AI_Turret turret in ObjectManager.Get<Obj_AI_Turret>())
                {
                    if (turret.IsValid && !turret.IsDead && turret.IsVisible && turret.IsOnScreen(TurretRange))
                    {
                        if (turret.IsAlly && drawFriendly || turret.IsEnemy && drawEnemy)
                        {
                            if (!distanceLimitEnabled || turret.Distance(ObjectManager.Player.Position) <= distanceLimit)
                            {
                                Utility.DrawCircle(turret.Position, TurretRange,
                                    _menu.Item("Turret" + (turret.IsAlly ? "Friendly" : "Enemy") + "Color")
                                        .GetValue<Color>(), circleThickness, circleQuality);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        private void OnDraw(EventArgs args)
        {
            if (!Enabled)
                return;

            DrawExperience();
            DrawAttack();
            DrawTurret();
            DrawSpell();
        }

        private void OnGameLoad(EventArgs args)
        {
            Logger.Prefix = string.Format("{0} - {1}", BaseName, Name);
            try
            {
                _menu = new Menu(Name, Name);

                var experienceMenu = new Menu("Experience", "Experience");
                experienceMenu.AddItem(new MenuItem("ExperienceColor", "Color").SetValue(Color.Gray));
                experienceMenu.AddItem(new MenuItem("ExperienceSelf", "Self").SetValue(true));
                experienceMenu.AddItem(new MenuItem("ExperienceFriendly", "Friendly").SetValue(true));
                experienceMenu.AddItem(new MenuItem("ExperienceEnemy", "Enemy").SetValue(true));

                var attackMenu = new Menu("Attack", "Attack");
                attackMenu.AddItem(new MenuItem("AttackColor", "Color").SetValue(Color.Yellow));
                attackMenu.AddItem(new MenuItem("AttackSelf", "Self").SetValue(true));
                attackMenu.AddItem(new MenuItem("AttackFriendly", "Friendly").SetValue(true));
                attackMenu.AddItem(new MenuItem("AttackEnemy", "Enemy").SetValue(true));

                var turretMenu = new Menu("Turret", "Turret");
                turretMenu.AddItem(new MenuItem("TurretFriendlyColor", "Friendly Color").SetValue(Color.DarkGreen));
                turretMenu.AddItem(new MenuItem("TurretEnemyColor", "Enemy Color").SetValue(Color.DarkRed));
                turretMenu.AddItem(new MenuItem("TurretFriendly", "Friendly").SetValue(true));
                turretMenu.AddItem(new MenuItem("TurretEnemy", "Enemy").SetValue(true));

                var spellMenu = new Menu("Spell", "Spell");
                spellMenu.AddItem(new MenuItem("SpellMaxRange", "Max Spell Range").SetValue(new Slider(1000, 500, 3000)));


                var spellSelfMenu = new Menu("SpellSelf", "Self");
                spellSelfMenu.AddItem(new MenuItem("SpellSelfColor", "Color").SetValue(Color.Purple));
                spellSelfMenu.AddItem(new MenuItem("SpellSelfQ", "Q").SetValue(true));
                spellSelfMenu.AddItem(new MenuItem("SpellSelfW", "W").SetValue(true));
                spellSelfMenu.AddItem(new MenuItem("SpellSelfE", "E").SetValue(true));
                spellSelfMenu.AddItem(new MenuItem("SpellSelfR", "R").SetValue(true));

                spellMenu.AddSubMenu(spellSelfMenu);

                var spellFriendlyMenu = new Menu("SpellFriendly", "Friendly");
                spellFriendlyMenu.AddItem(new MenuItem("SpellFriendlyColor", "Color").SetValue(Color.Green));
                spellFriendlyMenu.AddItem(new MenuItem("SpellFriendlyQ", "Q").SetValue(true));
                spellFriendlyMenu.AddItem(new MenuItem("SpellFriendlyW", "W").SetValue(true));
                spellFriendlyMenu.AddItem(new MenuItem("SpellFriendlyE", "E").SetValue(true));
                spellFriendlyMenu.AddItem(new MenuItem("SpellFriendlyR", "R").SetValue(true));

                spellMenu.AddSubMenu(spellFriendlyMenu);

                var spellEnemyMenu = new Menu("SpellEnemy", "Enemy");
                spellEnemyMenu.AddItem(new MenuItem("SpellEnemyColor", "Color").SetValue(Color.Red));
                spellEnemyMenu.AddItem(new MenuItem("SpellEnemyQ", "Q").SetValue(true));
                spellEnemyMenu.AddItem(new MenuItem("SpellEnemyW", "W").SetValue(true));
                spellEnemyMenu.AddItem(new MenuItem("SpellEnemyE", "E").SetValue(true));
                spellEnemyMenu.AddItem(new MenuItem("SpellEnemyR", "R").SetValue(true));

                spellMenu.AddSubMenu(spellEnemyMenu);

                var distanceMenu = new Menu("Distance", "Distance");
                distanceMenu.AddItem(new MenuItem("DistanceEnabled", "Limit by Distance").SetValue(true));
                distanceMenu.AddItem(
                    new MenuItem("DistanceLimit", "Distance Limit").SetValue(new Slider(1500, 500, 3000)));

                _menu.AddSubMenu(experienceMenu);
                _menu.AddSubMenu(attackMenu);
                _menu.AddSubMenu(turretMenu);
                _menu.AddSubMenu(spellMenu);
                _menu.AddSubMenu(distanceMenu);

                _menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(true));

                BaseMenu.AddSubMenu(_menu);

                Drawing.OnDraw += OnDraw;
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        #endregion
    }
}