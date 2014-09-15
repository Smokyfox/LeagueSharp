namespace SFXUtility.Class
{
    #region

    using System;
    using System.Collections.Generic;

    #endregion

    public class Mediator
    {
        #region Fields

        private readonly MessageToActionsMap _messageToCallbacksMap;

        #endregion

        #region Constructor

        public Mediator()
        {
            _messageToCallbacksMap = new MessageToActionsMap();
        }

        #endregion

        #region Methods

        public void NotifyColleagues(object from, object message)
        {
            List<Action<object>> actions = _messageToCallbacksMap.GetActions(from);

            if (actions != null)
                actions.ForEach(action => action(message));
        }

        public void Register(object from, Action<object> callback)
        {
            _messageToCallbacksMap.AddAction(from, callback);
        }

        #endregion
    }
}