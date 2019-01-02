using AutoMapper;
using GrabNReadApp.Web.Automapper;

namespace GrabNReadApp.Tests
{
    public class TestInitializer
    {
        private static bool testsInitialized = false;
        private static readonly object obj = new object();

        public static void Initialize()
        {
            lock (obj)
            {
                if (!testsInitialized)
                {
                    Mapper.Initialize(config => config.AddProfile<MappingConfiguration>());
                    testsInitialized = true;
                }

            }
        }
    }
}
