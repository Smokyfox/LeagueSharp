namespace SFXUtility.Class
{
    #region

    using System;
    using System.Reflection;

    #endregion

    internal class WeakAction : WeakReference
    {
        #region Fields

        private readonly MethodInfo _method;

        #endregion

        #region Constructors

        internal WeakAction(Action<object> action)
            : base(action.Target)
        {
            _method = action.Method;
        }

        #endregion

        #region Methods

        internal Action<object> CreateAction()
        {
            if (!base.IsAlive)
                return null;

            try
            {
                return Delegate.CreateDelegate(
                    typeof (Action<object>),
                    base.Target,
                    _method.Name)
                    as Action<object>;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}