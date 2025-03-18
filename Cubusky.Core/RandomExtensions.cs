using System;
using System.Runtime.CompilerServices;

namespace Cubusky
{
    /// <summary>Provides extension methods for the <see cref="Random"/> class.</summary>
    public static class RandomExtensions
    {
#if NETSTANDARD2_1_OR_GREATER
        /// <summary>Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <returns>A single-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NextSingle(this Random random) => (float)random.NextDouble();

        /// <summary>Returns a non-negative random integer.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <returns>A 64-bit signed integer that is greater than or equal to 0 and less than <see cref="long.MaxValue"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long NextInt64(this Random random) => (long)random.NextDouble(long.MaxValue);

        /// <summary>Returns a non-negative random integer that is less than the specified maximum.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
        /// <returns>
        /// A 64-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily
        /// includes 0 but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals 0, <paramref name="maxValue"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long NextInt64(this Random random, long maxValue) => (long)random.NextDouble(maxValue);

        /// <summary>Returns a random integer that is within a specified range.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>
        /// A 64-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/>
        /// but not <paramref name="maxValue"/>. If minValue equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long NextInt64(this Random random, long minValue, long maxValue) => (long)random.NextDouble(minValue, maxValue);
#endif

        /// <summary>Returns a non-negative random floating-point number that is less than the specified maximum.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
        /// <returns>A single-precision floating point number that is greater than or equal to 0.0, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily
        /// includes 0 but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals 0, <paramref name="maxValue"/> is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NextSingle(this Random random, float maxValue)
        {
            Throw.IfArgumentNegative(maxValue, nameof(maxValue));
            return random.NextSingle() * maxValue;
        }

        /// <summary>Returns a random floating-point number that is within a specified range.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>A single-precision floating point number that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily
        /// includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. However, if <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NextSingle(this Random random, float minValue, float maxValue)
        {
            Throw.IfArgumentGreaterThan(minValue, maxValue, nameof(minValue));
            return random.NextSingle() * (maxValue - minValue) + minValue;
        }

        /// <summary>Returns a non-negative random floating-point number that is less than the specified maximum.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily
        /// includes 0 but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals 0, <paramref name="maxValue"/> is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NextDouble(this Random random, double maxValue)
        {
            Throw.IfArgumentNegative(maxValue, nameof(maxValue));
            return random.NextDouble() * maxValue;
        }

        /// <summary>Returns a random floating-point number that is within a specified range.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>A double-precision floating point number that is greater than or equal to <paramref name="minValue"/>, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily
        /// includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. However, if <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            Throw.IfArgumentGreaterThan(minValue, maxValue, nameof(minValue));
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>Returns a non-negative random <see cref="TimeSpan"/>.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <returns>A <see cref="TimeSpan"/> that is greater than or equal to <see cref="TimeSpan.Zero"/> and less than <see cref="TimeSpan.MaxValue"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan NextTimeSpan(this Random random) => new TimeSpan(random.NextInt64());

        /// <summary>Returns a non-negative random <see cref="TimeSpan"/> that is less than the specified maximum.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="maxValue">The exclusive upper bound of the random <see cref="TimeSpan"/> to be generated. <paramref name="maxValue"/> must be greater than or equal to <see cref="TimeSpan.Zero"/>.</param>
        /// <returns>
        /// A <see cref="TimeSpan"/> that is greater than or equal to <see cref="TimeSpan.Zero"/>, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily
        /// includes <see cref="TimeSpan.Zero"/> but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals <see cref="TimeSpan.Zero"/>, <paramref name="maxValue"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan NextTimeSpan(this Random random, TimeSpan maxValue) => new TimeSpan(random.NextInt64(maxValue.Ticks));

        /// <summary>Returns a random <see cref="TimeSpan"/> that is within a specified range.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="minValue">The inclusive lower bound of the random <see cref="TimeSpan"/> returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random <see cref="TimeSpan"/> returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>
        /// A <see cref="TimeSpan"/> greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/>
        /// but not <paramref name="maxValue"/>. If minValue equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeSpan NextTimeSpan(this Random random, TimeSpan minValue, TimeSpan maxValue) => new TimeSpan(random.NextInt64(minValue.Ticks, maxValue.Ticks));

        /// <summary>Returns a non-negative random <see cref="DateTime"/>.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <returns>A <see cref="DateTime"/> that is greater than or equal to <see cref="DateTime.UnixEpoch"/> and less than <see cref="DateTime.MaxValue"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime NextDatetime(this Random random) => new DateTime(random.NextInt64(DateTime.UnixEpoch.Ticks, DateTime.MaxValue.Ticks));

        /// <summary>Returns a non-negative random <see cref="DateTime"/> that is less than the specified maximum.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="maxValue">The exclusive upper bound of the random <see cref="DateTime"/> to be generated. <paramref name="maxValue"/> must be greater than or equal to <see cref="DateTime.UnixEpoch"/>.</param>
        /// <returns>
        /// A <see cref="DateTime"/> that is greater than or equal to <see cref="DateTime.UnixEpoch"/>, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily
        /// includes <see cref="DateTime.UnixEpoch"/> but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals <see cref="DateTime.UnixEpoch"/>, <paramref name="maxValue"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime NextDatetime(this Random random, DateTime maxValue) => new DateTime(random.NextInt64(DateTime.UnixEpoch.Ticks, maxValue.Ticks));

        /// <summary>Returns a random <see cref="DateTime"/> that is within a specified range.</summary>
        /// <param name="random">The <see cref="Random"/> class instance.</param>
        /// <param name="minValue">The inclusive lower bound of the random <see cref="DateTime"/> returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random <see cref="DateTime"/> returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>
        /// A <see cref="DateTime"/> greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/>
        /// but not <paramref name="maxValue"/>. If minValue equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime NextDatetime(this Random random, DateTime minValue, DateTime maxValue) => new DateTime(random.NextInt64(minValue.Ticks, maxValue.Ticks));
    }
}