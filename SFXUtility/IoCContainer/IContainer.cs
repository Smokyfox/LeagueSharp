namespace SFXUtility.IoCContainer
{
    #region

    using System;

    #endregion

    internal interface IContainer
    {
        #region Methods

        void Deregister(Type type, string instanceName = null);

        void Deregister<T>(string instanceName = null);
        bool IsRegistered(Type type, string instanceName = null);

        bool IsRegistered<T>(string instanceName = null);

        void Register(Type from, Type to, bool singleton, bool initialize, string instanceName = null);

        void Register<TFrom, TTo>(bool singleton, bool initialize, string instanceName = null) where TTo : TFrom;

        void Register(Type type, Func<object> createInstanceDelegate, bool singleton, bool initialize,
            string instanceName = null);

        void Register<T>(Func<T> createInstanceDelegate, bool singleton, bool initialize, string instanceName = null);

        object Resolve(Type type, string instanceName = null);

        T Resolve<T>(string instanceName = null);

        #endregion
    }
}