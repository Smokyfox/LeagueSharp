namespace SFXUtility
{
    #region

    using System;
    using System.Linq;
    using System.Reflection;
    using Class;
    using IoCContainer;
    using Logger;

    #endregion

    internal class Program
    {
        #region Methods

        private static void Main(string[] args)
        {
            try
            {
                var container = new Container();
                container.Register<ILogger, ConsoleLogger>();
                //container.Register<Mediator, Mediator>(true);

                container.Register(typeof (SFXUtility), () => new SFXUtility(container), true, true);

                var bType = typeof (Base);
                foreach (
                    var type in
                        Assembly.GetAssembly(bType)
                            .GetTypes()
                            .OrderBy(type => type.Name)
                            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(bType)))
                {
                    var tmpType = type;
                    container.Register(type, () => Activator.CreateInstance(tmpType, new object[] {container}), true,
                        true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion
    }
}