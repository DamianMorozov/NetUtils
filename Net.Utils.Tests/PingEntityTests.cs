using NUnit.Framework;
using System.Threading.Tasks;

namespace Net.Utils.Tests
{
    [TestFixture]
    internal class PingEntityTests
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

            Assert.DoesNotThrow(() =>
            {
                _ = new PingEntity();
            });
            Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
            {
                _ = new PingEntity();
            }));

            Utils.MethodComplete();
        }

        [Test]
        public void ConstructorSetup_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var isRepeat in EnumValues.GetBool())
            {
                foreach (var timeoutPing in EnumValues.GetTimeoutMs())
                {
                    foreach (var timeoutTask in EnumValues.GetTimeoutMs())
                    {
                        foreach (var useStopWatch in EnumValues.GetBool())
                        {
                            Assert.DoesNotThrow(() =>
                            {
                                _ = new PingEntity(timeoutPing, timeoutTask, false, useStopWatch);
                            });
                            Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                            {
                                _ = new PingEntity(timeoutPing, timeoutTask, false, useStopWatch);
                            }));
                        }
                    }
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void Open_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var timeoutPing in EnumValues.GetTimeoutMs())
            {
                foreach (var timeoutTask in EnumValues.GetTimeoutMs())
                {
                    Assert.DoesNotThrow(() =>
                    {
                        foreach (var useStopWatch in EnumValues.GetBool())
                        {
                            var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false, useStopWatch: useStopWatch);
                            ping.Hosts.Add("localhost");
                            ping.Hosts.Add("google.com");
                            ping.Hosts.Add("google-fake.com");
                            ping.Open();
                            TestContext.WriteLine($@"{ping.Status}");
                        }
                    });
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        foreach (var useStopWatch in EnumValues.GetBool())
                        {
                            var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false, useStopWatch: useStopWatch);
                            ping.Hosts.Add("localhost");
                            ping.Hosts.Add("google.com");
                            ping.Hosts.Add("google-fake.com");
                            ping.Open();
                            TestContext.WriteLine($@"{ping.Status}");
                        }
                    }));
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void OpenAsync_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var timeoutPing in EnumValues.GetTimeoutMs())
            {
                foreach (var timeoutTask in EnumValues.GetTimeoutMs())
                {
                    Assert.DoesNotThrow(() =>
                    {
                        foreach (var useStopWatch in EnumValues.GetBool())
                        {
                            var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false, useStopWatch: useStopWatch);
                            ping.Hosts.Add("localhost");
                            ping.Hosts.Add("google.com");
                            ping.Hosts.Add("google-fake.com");
                            var task = Task.Run(async () =>
                            {
                                await ping.OpenAsync().ConfigureAwait(true);
                            });
                            task.Wait();
                            TestContext.WriteLine($@"{ping.Status}");
                        }
                    });
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        foreach (var useStopWatch in EnumValues.GetBool())
                        {
                            var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false, useStopWatch: useStopWatch);
                            ping.Hosts.Add("localhost");
                            ping.Hosts.Add("google.com");
                            ping.Hosts.Add("google-fake.com");
                            var task = Task.Run(async () =>
                            {
                                await ping.OpenAsync().ConfigureAwait(true);
                            });
                            task.Wait();
                            TestContext.WriteLine($@"{ping.Status}");
                        }
                    }));
                }
            }

            Utils.MethodComplete();
        }
    }
}