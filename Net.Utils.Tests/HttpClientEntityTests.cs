using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Net.Utils.Tests
{
    [TestFixture]
    internal class HttpClientEntityTests
    {
        #region Private fields and properties

        //

        #endregion

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

            Assert.DoesNotThrow(() => { _ = new HttpClientEntity(); });
            Assert.DoesNotThrowAsync(async () => await Task.Run(() => { _ = new HttpClientEntity(); }));

            Utils.MethodComplete();
        }

        [Test]
        public void ConstructorSetup_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var isTimeout in EnumValues.GetBool())
            {
                foreach (var timeout in EnumValues.GetInt())
                {
                    foreach (var host in EnumValues.GetUri())
                    {
                        Assert.DoesNotThrow(() => { _ = new HttpClientEntity(isTimeout, timeout, host); });
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() => { _ = new HttpClientEntity(); }));
                    }
                }
            }
            
            Utils.MethodComplete();
        }

        [Test]
        public void OpenTask_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var isTimeout in EnumValues.GetBool())
            {
                foreach (var timeout in EnumValues.GetInt())
                {
                    foreach (var host in EnumValues.GetUri())
                    {
                        foreach (var isTaskWait in EnumValues.GetBool())
                        {
                            Assert.DoesNotThrow(() =>
                            {
                                TestContext.WriteLine($@"Assert.DoesNotThrow. IsTaskWait: {isTaskWait}. isTimeout: {isTimeout}. timeout: {timeout}. host: {host}");
                                var proxy = new ProxyEntity();
                                var httpClient = new HttpClientEntity(isTimeout, timeout, host)
                                {
                                    IsTaskWait = isTaskWait,
                                };
                                httpClient.OpenTask(proxy);
                                if (httpClient.IsTaskWait)
                                {
                                    TestContext.WriteLine($@"{httpClient.Status}");
                                }
                            });
                            Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                            {
                                TestContext.WriteLine($@"Assert.DoesNotThrow. IsTaskWait: {isTaskWait}. isTimeout: {isTimeout}. timeout: {timeout}. host: {host}");
                                var proxy = new ProxyEntity();
                                var httpClient = new HttpClientEntity(isTimeout, timeout, host)
                                {
                                    IsTaskWait = isTaskWait,
                                };
                                httpClient.OpenTask(proxy);
                                if (httpClient.IsTaskWait)
                                {
                                    TestContext.WriteLine($@"{httpClient.Status}");
                                }
                            }));
                        }
                    }
                }
            }
            
            Utils.MethodComplete();
        }
    }
}