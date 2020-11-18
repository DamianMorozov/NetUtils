using NUnit.Framework;
using System.Threading.Tasks;

namespace Net.Utils.Tests
{
    [TestFixture]
    internal class HttpClientEntityTests
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

            Assert.DoesNotThrow(() => { _ = new HttpClientEntity(); });
            Assert.DoesNotThrowAsync(async () => await Task.Run(() => { _ = new HttpClientEntity(); }));

            Utils.MethodComplete();
        }

        [Test]
        public void ConstructorSetup_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var timeout in EnumValues.GetInt())
            {
                foreach (var host in EnumValues.GetUri())
                {
                    Assert.DoesNotThrow(() => { _ = new HttpClientEntity(timeout, host); });
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() => { _ = new HttpClientEntity(); }));
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void OpenTask_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var timeout in EnumValues.GetTimeoutMs())
            {
                foreach (var host in EnumValues.GetUri())
                {
                    foreach (var isTaskWait in EnumValues.GetBool())
                    {
                        Assert.DoesNotThrow(() =>
                        {
                            TestContext.WriteLine($@"Assert.DoesNotThrow. IsTaskWait: {isTaskWait}. timeout: {timeout}. host: {host}");
                            var proxy = new ProxyEntity();
                            var httpClient = new HttpClientEntity(timeout, host);
                            httpClient.OpenTask(isTaskWait, proxy);
                            if (isTaskWait)
                            {
                                TestContext.WriteLine($@"{httpClient.Status}");
                                TestContext.WriteLine($@"{httpClient.Content}");
                            }
                        });
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                        {
                            TestContext.WriteLine($@"Assert.DoesNotThrow. IsTaskWait: {isTaskWait}. timeout: {timeout}. host: {host}");
                            var proxy = new ProxyEntity();
                            var httpClient = new HttpClientEntity(timeout, host);
                            httpClient.OpenTask(isTaskWait, proxy);
                            if (isTaskWait)
                            {
                                TestContext.WriteLine($@"{httpClient.Status}");
                                TestContext.WriteLine($@"{httpClient.Content}");
                            }
                        }));
                    }
                }
            }

            Utils.MethodComplete();
        }
    }
}