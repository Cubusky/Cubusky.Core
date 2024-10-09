using System;
using Xunit;

namespace Cubusky.Tests
{
    public class RandomExtensionsTests
    {
        public static readonly Random random = new Random();

        [Fact]
        public void Random_NextInt64()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextInt64();
                Assert.InRange(result, 0, long.MaxValue - 1);
            }
        }

        [Fact]
        public void Random_NextInt64_Negative()
        {
            Assert.Throws<ArgumentOutOfRangeException>("maxValue", () => random.NextInt64(-1));
        }

        [Fact]
        public void Random_NextInt64_Zero()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextInt64(0);
                Assert.Equal(0, result);
            }
        }

        [Fact]
        public void Random_NextInt64_100()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextInt64(100);
                Assert.InRange(result, 0, 99);
            }
        }

        [Fact]
        public void Random_NextInt64_Same()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextInt64(100, 100);
                Assert.Equal(100, result);
            }
        }

        [Fact]
        public void Random_NextInt64_MaxLessThanMin()
        {
            Assert.Throws<ArgumentOutOfRangeException>("minValue", () => random.NextInt64(100, 99));
        }

        [Fact]
        public void Random_NextInt64_Range()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextInt64(42, 69);
                Assert.InRange(result, 42, 69);
            }
        }

        [Fact]
        public void Random_NextInt64_NegativeRange()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextInt64(-69, -42);
                Assert.InRange(result, -69, -42);
            }
        }
    }
}
