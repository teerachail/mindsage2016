using MindSageWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MindSageWeb.Theories
{
    public class APIUtilSpecs
    {
        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("1", "")]
        [InlineData("123", "")]
        [InlineData("1234", "1234")]
        [InlineData("123456789", "6789")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "WXYZ")]
        public void Get4DigitsCorrect(string cardNumber, string expectedValue)
        {
            var result = APIUtil.GetLast4Characters(cardNumber);
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("1", "")]
        [InlineData("123", "")]
        [InlineData("1234", "")]
        [InlineData("123456789", "1xxxx6789")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "ABCDEFGHIJKLMNOPQRxxxxWXYZ")]
        public void EncodeCreditCard(string cardNumber, string expectedValue)
        {
            var result = APIUtil.EncodeCreditCard(cardNumber);
            Assert.Equal(expectedValue, result);
        }
    }
}
