using System;
using Xunit;
using Cake.Xamarin.Sample.Shared;

namespace Cake.Xamarin.Sample.Tests
{
    public class SecondTest
    {
        [Fact]
        public void Passing_Test()
        {
            Assert.Equal(4, new SharedClass().Add(2, 2));
        }
    }
}
