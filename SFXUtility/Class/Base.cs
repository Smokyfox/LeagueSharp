namespace SFXUtility.Class
{
    #region

    using System;
    using IoCContainer;
    using LeagueSharp.Common;
    using Logger;

    #endregion

    internal abstract class Base
    {
        #region Constructors

        protected Base(IContainer container)
        {
            if (!container.IsRegistered<SFXUtility>())
                throw new InvalidOperationException("SFXUtility");
            if (!container.IsRegistered<ILogger>())
                throw new InvalidOperationException("ILogger");

            IoC = container;

            var sfx = IoC.Resolve<SFXUtility>();

            Logger = IoC.Resolve<ILogger>();
            BaseMenu = sfx.Menu;
            BaseName = sfx.Name;
            sfx.OnUnload += OnUnload;
        }

        #endregion

        #region Properties

        public abstract bool Enabled { get; }
        public abstract string Name { get; }

        protected Menu BaseMenu { get; private set; }

        protected string BaseName { get; private set; }
        protected IContainer IoC { get; private set; }

        protected ILogger Logger { get; set; }

        #endregion

        #region Methods

        protected virtual void OnUnload(object sender, EventArgs args)
        {
            IoC.Deregister(GetType());
        }

        #endregion
    }
}