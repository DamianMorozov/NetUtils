using NetUtils;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NetUtilsTests
{
    [TestFixture]
    internal class ProgramTests
    {
        /// <summary>
        /// Setup private fields.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            Utils.MethodStart();
            //
            Utils.MethodComplete();
        }

        /// <summary>
        /// Reset private fields to default state.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            Utils.MethodStart();
            // 
            Utils.MethodComplete();
        }

        [Test]
        public void ConstructorSetupDefault_DoesNotThrow()
        {
            Utils.MethodStart();

            Assert.DoesNotThrow(() => { _ = new ProxyEntity(); });
            Assert.DoesNotThrowAsync(async () => await Task.Run(() => { _ = new ProxyEntity(); }));

            Utils.MethodComplete();
        }

        [Test]
        public void ConstructorSetup_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var use in EnumValues.GetBool())
            {
                foreach (var useDefaultCredentials in EnumValues.GetBool())
                {
                    foreach (var host in EnumValues.GetUri())
                    {
                        foreach (var port in EnumValues.GetInt())
                        {
                            foreach (var domain in EnumValues.GetString())
                            {
                                foreach (var username in EnumValues.GetString())
                                {
                                    foreach (var password in EnumValues.GetString())
                                    {
                                        Assert.DoesNotThrow(() =>
                                        {
                                            _ = new ProxyEntity(use, useDefaultCredentials, host, port, domain, username, password);
                                        });
                                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                                        {
                                            _ = new ProxyEntity(use, useDefaultCredentials, host, port, domain, username, password);
                                        }));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Utils.MethodComplete();
        }
    }
}