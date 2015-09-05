/*
 * Created by guinness
 * User: guinness
 * Date: 2015/09/05
 */

namespace ExtensionMethods
{
    using NUnit.Framework;

    /// <summary>
    ///     Unit testing class
    /// </summary>
    [TestFixture]
    public class UnitTests
    {
        /// <summary>
        ///     Initialise unit test resources
        /// </summary>
        [SetUp]
        public void Init()
        {
            // Empty
        }

        /// <summary>
        ///     Cleanup unit test resources
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            // Empty
        }

        /// <summary>
        ///     Test Is*() methods
        /// </summary>
        [Test]
        public void TestIs()
        {
            const string username = "softwarespot";

            // IsNotNull
            Assert.IsTrue(username.IsNotNull(), "IsNotNull() is working");

            // IsInt
            Assert.IsTrue("21".IsInt(), "IsInt() is working");
        }

        /// <summary>
        ///     Test ParseAs* methods
        /// </summary>
        [Test]
        public void TestParse()
        {
            // ParseAsInt
            Assert.AreEqual(21, "21".ParseAsInt(), "ParseAsInt() is working");
        }
    }
}