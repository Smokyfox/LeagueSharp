namespace SFXUtility
{
    #region

    using System;
    using Class;
    using IoCContainer;
    using LeagueSharp.Common;

    #endregion

    internal class SFXUtility
    {
        #region Fields

        private readonly IContainer _container;

        #endregion

        #region Constructors

        public SFXUtility(IContainer container)
        {
            _container = container;

            Menu = new Menu(Name, Name, true);

            var miscMenu = new Menu("Misc", "Misc");

            var infoMenu = new Menu("Info", "Info");
            infoMenu.AddItem(new MenuItem("InfoVersion", string.Format("Version: {0}", Version)));
            infoMenu.AddItem(new MenuItem("InfoIRC", "IRC: Appril"));
            infoMenu.AddItem(new MenuItem("InfoGithub", "Github:").SetValue(new StringList(new[]
            {
                "Smokyfox",
                "Lizzaran"
            })));

            miscMenu.AddSubMenu(infoMenu);

            miscMenu.AddItem(new MenuItem("MiscCircleQuality", "Circles Quality").SetValue(new Slider(20, 80, 3)));
            miscMenu.AddItem(new MenuItem("MiscCircleThickness", "Circles Thickness").SetValue(new Slider(3, 10, 1)));

            Menu.AddSubMenu(miscMenu);

            AppDomain.CurrentDomain.DomainUnload += OnExit;
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            CustomEvents.Game.OnGameEnd += OnGameEnd;
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        #endregion

        #region Events

        public event EventHandler OnUnload;

        #endregion

        #region Properties

        public Menu Menu { get; private set; }

        public string Name
        {
            get { return "SFXUtility"; }
        }

        public Version Version
        {
            get { return new Version(0, 5, 0); }
        }

        #endregion

        #region Methods

        private void OnExit(object sender, EventArgs e)
        {
            EventHandler handler = OnUnload;
            if (null != handler) handler(this, EventArgs.Empty);
        }

        private void OnGameEnd(EventArgs args)
        {
            OnExit(null, null);
        }

        private void OnGameLoad(EventArgs args)
        {
            Chat.Print(string.Format("{0} v{1} loaded.", Name, string.Format("{0}.{1}", Version.Major, Version.Minor) +
                                                               (Version.Minor > 0
                                                                   ? string.Format(".{0}", Version.Minor)
                                                                   : string.Empty)));
            //using (var updater = new Updater(_container.Resolve<ILogger>(), new Uri("versionUri"), new Uri("updateUri"), Version))
            //{
            //    updater.Update();
            //}
            Menu.AddToMainMenu();
        }

        #endregion
    }
}