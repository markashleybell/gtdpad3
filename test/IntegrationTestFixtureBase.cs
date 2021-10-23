using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Respawn;

namespace test
{
    public abstract class IntegrationTestFixtureBase
    {
        private static readonly Checkpoint _db = new Checkpoint {
            TablesToIgnore = new[] {
                "Users",
                "SectionTypes"
            }
        };

        protected readonly IConfigurationRoot _cfg;

        protected IntegrationTestFixtureBase() =>
            _cfg = new ConfigurationBuilder()
             .AddJsonFile("testsettings.json")
             .Build();

        [SetUp]
        public void SetUp() =>
            _db.Reset(_cfg.GetConnectionString("Test"));

        [TearDown]
        public void TearDown()
        {
        }
    }
}
