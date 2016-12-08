using System;
using Cake.Xamarin.Sample.Shared;
using Xunit;

namespace Cake.Xamarin.Sample.Tests
{
    public class FirstTest
    {
        [Fact]
        public void Passing_Test()
        {
            Assert.Equal(4, new SharedClass().Add(2, 2));
        }

        [Fact]
        public void Failing_Test()
        {
            Assert.Equal(5, new SharedClass().Add(2, 2));
        }
    }
}
