using System.Threading.Tasks;
using NetUtils;
using NUnit.Framework;

namespace NetUtilsTests
{
    [TestFixture]
    internal class PingEntityTests
    {
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

            foreach (var timeoutPing in EnumValues.GetTimeoutMs())
            {
                foreach (var timeoutTask in EnumValues.GetTimeoutMs())
                {
                    foreach (var dontFragment in EnumValues.GetBool())
                    {
                        foreach (var bufferSize in EnumValues.GetBytes())
                        {
                            foreach (var ttl in EnumValues.GetBytes())
                            {
                                Assert.DoesNotThrow(() =>
                                {
                                    _ = new PingEntity(timeoutPing, bufferSize, ttl, dontFragment, timeoutTask, false);
                                    _ = new PingEntity(timeoutPing, bufferSize, ttl, dontFragment, timeoutTask, false);
                                    _ = new PingEntity(timeoutPing, bufferSize, ttl, dontFragment, timeoutTask, false);
                                });
                                Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                                {
                                    _ = new PingEntity(timeoutPing, bufferSize, ttl, dontFragment, timeoutTask, false);
                                    _ = new PingEntity(timeoutPing, bufferSize, ttl, dontFragment, timeoutTask, false);
                                    _ = new PingEntity(timeoutPing, bufferSize, ttl, dontFragment, timeoutTask, false);
                                }));
                            }
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

            foreach (var timeoutPing in EnumValues.GetBytes())
            {
                foreach (var timeoutRepeat in EnumValues.GetBytes())
                {
                    foreach (var dontFragment in EnumValues.GetBool())
                    {
                        foreach (var bufferSize in EnumValues.GetBytes())
                        {
                            foreach (var ttl in EnumValues.GetBytes())
                            {
                                if ((timeoutPing > 127 && timeoutPing < 257) && (timeoutRepeat > 127 && timeoutRepeat < 257) &&
                                    (bufferSize >= 0 && bufferSize < 1465) && (ttl > 0 && ttl < 256))
                                {
                                    Assert.DoesNotThrow(() =>
                                    {
                                        var ping = new PingEntity(timeoutPing: timeoutPing, bufferSize: bufferSize, ttl: ttl, dontFragment: dontFragment,
                                            timeoutTask: timeoutRepeat, useRepeat: false);
                                        TestContext.WriteLine($@"{ping.Settings}");
                                        ping.Hosts.Add("google.com");
                                        ping.Hosts.Add("microsoft.com");
                                        ping.Hosts.Add("127.0.0.1");
                                        ping.Hosts.Add("yandex.com");
                                        ping.Hosts.Add("localhost");
                                        ping.Open();
                                        ping.Close();
                                        TestContext.WriteLine($@"{ping.Log}");
                                    });
                                }
                            }
                        }
                    }
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void OpenAsync_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var timeoutPing in EnumValues.GetBytes())
            {
                foreach (var timeoutRepeat in EnumValues.GetBytes())
                {
                    foreach (var dontFragment in EnumValues.GetBool())
                    {
                        foreach (var bufferSize in EnumValues.GetBytes())
                        {
                            foreach (var ttl in EnumValues.GetBytes())
                            {
                                if ((timeoutPing > 127 && timeoutPing < 257) && (timeoutRepeat > 127 && timeoutRepeat < 257) &&
                                    (bufferSize >= 0 && bufferSize < 1465) && (ttl > 0 && ttl < 256))
                                {
                                    Assert.DoesNotThrow(() =>
                                    {
                                        var ping = new PingEntity(timeoutPing: timeoutPing, bufferSize: bufferSize, ttl: ttl, dontFragment: dontFragment,
                                            timeoutTask: timeoutRepeat, useRepeat: false);
                                        TestContext.WriteLine($@"{ping.Settings}");
                                        ping.Hosts.Add("google.com");
                                        ping.Hosts.Add("microsoft.com");
                                        ping.Hosts.Add("127.0.0.1");
                                        ping.Hosts.Add("yandex.com");
                                        ping.Hosts.Add("localhost");
                                        var task = Task.Run(async () =>
                                        {
                                            await ping.OpenAsync().ConfigureAwait(true);
                                        });
                                        task.Wait();
                                        var taskClose = Task.Run(async () =>
                                        {
                                            await ping.CloseAsync().ConfigureAwait(true);
                                        });
                                        taskClose.Wait();
                                        TestContext.WriteLine($@"{ping.Log}");
                                    });
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