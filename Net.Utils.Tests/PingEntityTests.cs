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
                            _ = new PingEntity(timeoutPing, timeoutTask, isRepeat);
                        });
                        Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                        {
                            _ = new PingEntity(timeoutPing, timeoutTask, isRepeat);
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

            foreach (var timeoutPing in EnumValues.GetTimeoutMs())
            {
                foreach (var timeoutTask in EnumValues.GetTimeoutMs())
                {
                    Assert.DoesNotThrow(() =>
                    {
                        foreach (var isTaskWait in EnumValues.GetBool())
                        {
                            var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, isRepeat: false);
                            ping.Hosts.Add("localhost");
                            ping.Hosts.Add("google.com");
                            ping.OpenTask(isTaskWait);
                            if (isTaskWait)
                                TestContext.WriteLine($@"{ping.Status}");
                        }
                    });
                    Assert.DoesNotThrowAsync(async () => await Task.Run(() =>
                    {
                        foreach (var isTaskWait in EnumValues.GetBool())
                        {
                            var ping = new PingEntity(timeoutPing: timeoutPing, timeoutTask: timeoutTask, isRepeat: false);
                            ping.Hosts.Add("localhost");
                            ping.Hosts.Add("google.com");
                            ping.OpenTask(isTaskWait);
                            if (isTaskWait)
                                TestContext.WriteLine($@"{ping.Status}");
                        }
                    }));
                }
            }

            Utils.MethodComplete();
        }
    }
}