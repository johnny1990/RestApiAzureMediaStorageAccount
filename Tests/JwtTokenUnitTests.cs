using Application.Helpers;
using Application.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class JwtTokenUnitTests
    {
        [TestMethod]
        public void GetClaims_ReturnsClaims()
        {
            // Arrange
            var userTokens = new UserTokens
            {
                UserName = "test",
                Id = Guid.NewGuid()
            };

            // Act
            var result = JwtHelpers.GetClaims(userTokens, out Guid Id);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void GenTokenkey_ReturnsUserTokens()
        {
            // Arrange
            var userTokens = new UserTokens
            {
                UserName = "test",
                Id = Guid.NewGuid()
            };
            var jwtSettings = new JwtSettings
            {
                IssuerSigningKey = "your-128-bit-secret-key",
                ValidIssuer = "test",
                ValidAudience = "test"
            };
            // Act
            var result = JwtHelpers.GenTokenkey(userTokens, jwtSettings);
            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(userTokens.UserName, result.UserName);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(userTokens.Id, result.Id);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(Guid.Empty, result.GuidId);
        }
    }
}
