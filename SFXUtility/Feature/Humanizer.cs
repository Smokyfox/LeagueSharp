namespace SFXUtility.Feature
{
    #region

    using System;
    using Class;
    using IoCContainer;
    using LeagueSharp;
    using LeagueSharp.Common;

    #endregion

    internal class Humanizer : Base
    {
        #region Fields

        private float _lastMovement;
        private float _lastSpell;
        private Menu _menu;

        #endregion

        #region Constructors

        public Humanizer(Container container)
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
            get { return "Humanizer"; }
        }

        #endregion

        #region Methods

        private void OnGameLoad(EventArgs args)
        {
            Logger.Prefix = string.Format("{0} - {1}", BaseName, Name);
            try
            {
                _menu = new Menu(Name, Name);

                var delayMenu = new Menu("Delay", "Delay");
                delayMenu.AddItem(new MenuItem("DelaySpells", "Spells (ms)").SetValue(new Slider(20, 0, 250)));
                delayMenu.AddItem(new MenuItem("DelayMovement", "Movement (ms)").SetValue(new Slider(20, 0, 250)));

                _menu.AddSubMenu(delayMenu);

                _menu.AddItem(new MenuItem("Enabled", "Enabled").SetValue(true));

                BaseMenu.AddSubMenu(_menu);

                Game.OnGameSendPacket += OnGameSendPacket;
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        private void OnGameSendPacket(GamePacketEventArgs args)
        {
            if (!Enabled)
                return;

            try
            {
                var spellsDelay = _menu.Item("DelaySpells").GetValue<Slider>().Value;
                var movementDelay = _menu.Item("DelayMovement").GetValue<Slider>().Value;

                if (spellsDelay > 0 && (new GamePacket(args.PacketData)).Header == Packet.C2S.Cast.Header)
                {
                    Packet.C2S.Cast.Struct castStruct = Packet.C2S.Cast.Decoded(args.PacketData);
                    if (castStruct.SourceNetworkId == ObjectManager.Player.NetworkId)
                    {
                        if (_lastSpell + spellsDelay > Environment.TickCount)
                        {
                            args.Process = false;
                        }
                        else
                        {
                            _lastSpell = Environment.TickCount;
                        }
                    }
                }

                if (movementDelay > 0 && (new GamePacket(args.PacketData)).Header == Packet.C2S.Move.Header)
                {
                    Packet.C2S.Move.Struct movementStruct = Packet.C2S.Move.Decoded(args.PacketData);
                    if (movementStruct.MoveType == 2)
                    {
                        if (movementStruct.SourceNetworkId == ObjectManager.Player.NetworkId)
                        {
                            if (_lastMovement + movementDelay > Environment.TickCount)
                            {
                                args.Process = false;
                            }
                            else
                            {
                                _lastMovement = Environment.TickCount;
                            }
                        }
                    }
                    else if (movementStruct.MoveType == 3)
                    {
                        _lastMovement = 0f;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteBlock(ex.Message, ex.ToString());
            }
        }

        #endregion
    }
}