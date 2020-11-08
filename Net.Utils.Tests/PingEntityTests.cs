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
                foreach (var timeout in EnumValues.GetInt())
                {
                    foreach (var isTimeout in EnumValues.GetBool())
                    {
                        Assert.DoesNotThrow(() =>
                        {
                            _ = new PingEntity(isTimeout, timeout, isRepeat);
                        });
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                        {
                            _ = new PingEntity(isTimeout, timeout, isRepeat);
                        }));
                    }
                }
            }

            Utils.MethodComplete();
        }

        [Test]
        public void OpenTask_DoesNotThrow()
        {
            Utils.MethodStart();

            foreach (var timeout in EnumValues.GetInt())
            {
                foreach (var isTimeout in EnumValues.GetBool())
                {
                    Assert.DoesNotThrow(() =>
                    {
                        var ping = new PingEntity(isTimeout, timeout, false);
                        ping.Hosts.Add("localhost", false);
                        ping.Hosts.Add("google.com", false);
                        ping.OpenTask();
                        if (ping.IsTaskWait)
                        {
                            TestContext.WriteLine($@"{ping.Status}");
                        }
                    });
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        var ping = new PingEntity(isTimeout, timeout, false);
                        ping.Hosts.Add("localhost", false);
                        ping.Hosts.Add("google.com", false);
                        ping.OpenTask();
                        if (ping.IsTaskWait)
                        {
                            TestContext.WriteLine($@"{ping.Status}");
                        }
                    }));
                }
            }

            Utils.MethodComplete();
        }
    }
}