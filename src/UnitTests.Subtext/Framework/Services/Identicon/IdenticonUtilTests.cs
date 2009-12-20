using MbUnit.Framework;
using Subtext.Identicon;

namespace UnitTests.Subtext.Framework.Services.Identicon
{
    [TestFixture]
    public class IdenticonUtilTests
    {
        /// <summary>
        /// Run some tests using some pre-calculated salts, ip addresses, 
        /// and codes.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="salt"></param>
        /// <param name="expectedCode"></param>
        [RowTest]
        [Row("127.0.0.1", "RandomSalt", 2038335937)]
        [Row("127.0.0.1", "AnotherSalt", -516553779)]
        [Row("210.120.68.16", "AnotherSalt", 1592465917)]
        public void CodeReturnsProperHash(string ip, string salt, long expectedCode)
        {
            IdenticonUtil.Salt = salt;
            Assert.AreEqual(expectedCode, IdenticonUtil.Code(ip));
        }

        [Test]
        public void Code_WithNullIp_ThrowsArgumentNullException()
        {
            UnitTestHelper.AssertThrowsArgumentNullException(() => IdenticonUtil.Code(null));
        }
    }
}