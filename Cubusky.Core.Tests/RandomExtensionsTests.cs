using System;
using Xunit;

namespace Cubusky.Tests
{
    public class RandomExtensionsTests
    {
        public static readonly Random random = new Random();

        [Fact]
        public void Random_NextSingle()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextSingle();
                Assert.InRange(result, 0.0f, 1.0f);
            }
        }

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

        [Fact]
        public void Random_NextSingle_MaxValue()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextSingle(10.0f);
                Assert.InRange(result, 0.0f, 10.0f);
            }
        }

        [Fact]
        public void Random_NextSingle_Range()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextSingle(5.0f, 10.0f);
                Assert.InRange(result, 5.0f, 10.0f);
            }
        }

        [Fact]
        public void Random_NextDouble_MaxValue()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextDouble(10.0);
                Assert.InRange(result, 0.0, 10.0);
            }
        }

        [Fact]
        public void Random_NextDouble_Range()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextDouble(5.0, 10.0);
                Assert.InRange(result, 5.0, 10.0);
            }
        }

        [Fact]
        public void Random_NextTimeSpan()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextTimeSpan();
                Assert.InRange(result, TimeSpan.Zero, TimeSpan.MaxValue);
            }
        }

        [Fact]
        public void Random_NextTimeSpan_MaxValue()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextTimeSpan(TimeSpan.FromMinutes(10));
                Assert.InRange(result, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            }
        }

        [Fact]
        public void Random_NextTimeSpan_Range()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextTimeSpan(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
                Assert.InRange(result, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            }
        }

        [Fact]
        public void Random_NextDatetime()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextDatetime();
                Assert.InRange(result, DateTime.UnixEpoch, DateTime.MaxValue);
            }
        }

        [Fact]
        public void Random_NextDatetime_MaxValue()
        {
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextDatetime(DateTime.Now);
                Assert.InRange(result, DateTime.UnixEpoch, DateTime.Now);
            }
        }

        [Fact]
        public void Random_NextDatetime_Range()
        {
            var minValue = new DateTime(2022, 1, 1);
            var maxValue = new DateTime(2022, 12, 31);
            for (int i = 0; i < 50; i++)
            {
                var result = random.NextDatetime(minValue, maxValue);
                Assert.InRange(result, minValue, maxValue);
            }
        }
    }
}
