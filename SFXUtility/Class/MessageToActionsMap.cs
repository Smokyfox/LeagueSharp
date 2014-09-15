namespace SFXUtility.Class
{
    #region

    using System;
    using System.Collections.Generic;

    #endregion

    internal class MessageToActionsMap
    {
        #region Fields

        private readonly Dictionary<object, List<WeakAction>> _map;

        #endregion

        #region Constructor

        internal MessageToActionsMap()
        {
            _map = new Dictionary<object, List<WeakAction>>();
        }

        #endregion

        #region Methods

        internal void AddAction(object message, Action<object> callback)
        {
            if (!_map.ContainsKey(message))
                _map[message] = new List<WeakAction>();

            _map[message].Add(new WeakAction(callback));
        }

        internal List<Action<object>> GetActions(object message)
        {
            if (!_map.ContainsKey(message))
                return null;

            List<WeakAction> weakActions = _map[message];
            var actions = new List<Action<object>>();
            for (int i = weakActions.Count - 1; i > -1; --i)
            {
                WeakAction weakAction = weakActions[i];
                if (!weakAction.IsAlive)
                    weakActions.RemoveAt(i);
                else
                    actions.Add(weakAction.CreateAction());
            }

            RemoveMessageIfNecessary(weakActions, message);

            return actions;
        }

        private void RemoveMessageIfNecessary(List<WeakAction> weakActions, object message)
        {
            if (weakActions.Count == 0)
                _map.Remove(message);
        }

        #endregion
    }
}