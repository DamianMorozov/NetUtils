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
    }
}