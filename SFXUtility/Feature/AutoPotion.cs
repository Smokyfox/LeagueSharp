#region License

/*
 Copyright 2014 - 2014 Nikita Bernthaler
 AutoPotion.cs is part of SFXUtility.
 
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
    using System.Linq;
    using Class;
    using IoCContainer;
    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class AutoPotion : Base
    {
        #region Fields

        private List<Potion> _potions = new List<Potion>
        {
            new Potion
            {
                BuffName = "ItemCrystalFlask",
                MinCharges = 1,
                ItemId = (ItemId) Item.Pot.CrystallineFlask,
                Priority = 1,
                TypeList = new List<PotionType> {PotionType.Health, PotionType.Mana}
            },
            new Potion
            {
                BuffName = "RegenerationPotion",
                MinCharges = 0,
                ItemId = (ItemId) Item.Pot.HealthPotion,
                Priority = 2,
                TypeList = new List<PotionType> {PotionType.Health}
            },
            new Potion
            {
                BuffName = "FlaskOfCrystalWater",
                MinCharges = 0,
                ItemId = (ItemId) Item.Pot.ManaPotion,
                Priority = 3,
                TypeList = new List<PotionType> {PotionType.Mana}
            },
            new Potion
            {
                BuffName = "ItemMiniRegenPotion",
                MinCharges = 0,
                ItemId = (ItemId) Item.Pot.TotalBiscuitOfRejuvenation,
                Priority = 4,
                TypeList = new List<PotionType> {PotionType.Health, PotionType.Mana}
            }
        };

        #endregion

        #region Enums

        private enum PotionType
        {
            Health,
            Mana
        };

        #endregion

        #region Constructors

        public AutoPotion(Container container) : base(container)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        #endregion

        #region Properties

        public override bool Enabled
        {
            get { return Menu != null && Menu.Item("Enabled").GetValue<bool>(); }
        }

        public override string Name
        {
            get { return "Auto Potion"; }
        }

        #endregion

        #region Methods

        private InventorySlot GetPotionSlot(PotionType type)
        {
            return (from potion in _potions
                where potion.TypeList.Contains(type)
                from item in ObjectManager.Player.InventoryItems
                where item.Id == potion.ItemId && item.Charges >= potion.MinCharges
                select item).FirstOrDefault();
        }

        private bool IsBuffActive(PotionType type)
        {
            return
                _potions.Where(potion => potion.TypeList.Contains(type))
                    .Any(potion => ObjectManager.Player.IsBuffActive(potion.BuffName));
        }

        private void OnGameLoad(EventArgs args)
        {
            Logger.Prefix = string.Format("{0} - {1}", BaseName, Name);
            try
            {
                _potions = _potions.OrderBy(x => x.Priority).ToList();

                Menu = new Menu(Name, Name);

                var healthMenu = new Menu("Health", "Health");
                healthMenu.AddItem(new MenuItem("HealthPotion", "Use Health Potion").SetValue(true));
                healthMenu.AddItem(new MenuItem("HealthPercent", "HP Trigger Percent").SetValue(new Slider(60)));

                var manaMenu = new Menu("Mana", "Mana");
                manaMenu.AddItem(new MenuItem("ManaPotion", "Use Mana Potion").SetValue(true));
                manaMenu.AddItem(new MenuItem("ManaPercent", "MP Trigger Percent").SetValue(new Slider(60)));

                Menu.AddSubMenu(healthMenu);
                Menu.AddSubMenu(manaMenu);

                Menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(false));

                BaseMenu.AddSubMenu(Menu);

                Game.OnGameUpdate += OnGameUpdate;
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
                if (Menu.Item("HealthPotion").GetValue<bool>())
                {
                    if (ObjectManager.Player.HealthPercentage() <= Menu.Item("HealthPercent").GetValue<Slider>().Value)
                    {
                        InventorySlot healthSlot = GetPotionSlot(PotionType.Health);
                        if (healthSlot != null && !IsBuffActive(PotionType.Health))
                            healthSlot.UseItem();
                    }
                }

                if (Menu.Item("ManaPotion").GetValue<bool>())
                {
                    if (ObjectManager.Player.ManaPercentage() <= Menu.Item("ManaPercent").GetValue<Slider>().Value)
                    {
                        InventorySlot manaSlot = GetPotionSlot(PotionType.Mana);
                        if (manaSlot != null && !IsBuffActive(PotionType.Mana))
                            manaSlot.UseItem();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        #endregion

        #region Nested Types

        private class Potion
        {
            #region Properties

            public string BuffName { get; set; }
            public ItemId ItemId { get; set; }
            public int MinCharges { get; set; }
            public int Priority { get; set; }
            public List<PotionType> TypeList { get; set; }

            #endregion
        }

        #endregion
    }
}