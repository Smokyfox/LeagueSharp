namespace SFXUtility.IoCContainer
{
    #region

    using System;

    #endregion

    internal class MappingKey
    {
        #region Constructors

        public MappingKey(Type type, bool singleton, string instanceName)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Type = type;
            Singleton = singleton;
            InstanceName = instanceName;
        }

        #endregion

        #region Properties

        public object Instance { get; set; }

        public string InstanceName { get; protected set; }

        public bool Singleton { get; protected set; }
        public Type Type { get; protected set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var compareTo = obj as MappingKey;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (compareTo == null)
                return false;

            return Type == compareTo.Type &&
                   string.Equals(InstanceName, compareTo.InstanceName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int multiplier = 31;
                int hash = GetType().GetHashCode();

                hash = hash*multiplier + Type.GetHashCode();
                hash = hash*multiplier + (InstanceName == null ? 0 : InstanceName.GetHashCode());

                return hash;
            }
        }


        public override string ToString()
        {
            const string format = "{0} ({1}) - hash code: {2}";

            return string.Format(format, InstanceName ?? "[null]", Type.FullName, GetHashCode());
        }

        public string ToTraceString()
        {
            const string format = "Instance Name: {0} ({1})";

            return string.Format(format, InstanceName ?? "[null]", Type.FullName);
        }

        #endregion
    }
}