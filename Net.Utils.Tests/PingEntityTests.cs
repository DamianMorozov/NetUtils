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
                        Assert.DoesNotThrow(() =>
                        {
                            _ = new PingEntity(timeoutPing, timeoutTask, false);
                        });
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                        {
                            _ = new PingEntity(timeoutPing, timeoutTask, false);
                        }));
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
                        var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false);
                        ping.Hosts.Add("google.com");
                        ping.Hosts.Add("google-fake.com");
                        ping.Hosts.Add("127.0.0.1");
                        ping.Hosts.Add("1.1.1.1");
                        //ping.Hosts.Add("localhost");
                        ping.Open();
                        ping.Close();
                        TestContext.WriteLine($@"{ping.Log}");
                    });
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false);
                        ping.Hosts.Add("google.com");
                        ping.Hosts.Add("google-fake.com");
                        ping.Hosts.Add("127.0.0.1");
                        ping.Hosts.Add("1.1.1.1");
                        //ping.Hosts.Add("localhost");
                        ping.Open();
                        ping.Close();
                        TestContext.WriteLine($@"{ping.Log}");
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
                        var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false);
                        ping.Hosts.Add("google.com");
                        ping.Hosts.Add("google-fake.com");
                        ping.Hosts.Add("127.0.0.1");
                        ping.Hosts.Add("1.1.1.1");
                        //ping.Hosts.Add("localhost");
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
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, useRepeat: false);
                        ping.Hosts.Add("google.com");
                        ping.Hosts.Add("google-fake.com");
                        ping.Hosts.Add("127.0.0.1");
                        ping.Hosts.Add("1.1.1.1");
                        //ping.Hosts.Add("localhost");
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
                    }));
                }
            }

            Utils.MethodComplete();
        }
    }
}