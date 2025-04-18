﻿using System;
using System.Runtime.CompilerServices;

#if NET8_0_OR_GREATER
using System.Numerics;
#endif

namespace Cubusky
{
    internal static partial class Throw
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentGreaterThan<T>(T value, T other, string? paramName = null)
            where T : IComparable<T>
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, other, paramName);
#else
            if (value.CompareTo(other) > 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, $"The value was greater than {other}.");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentGreaterThanOrEqual<T>(T value, T other, string? paramName = null)
            where T : IComparable<T>
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(value, other, paramName);
#else
            if (value.CompareTo(other) >= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, $"The value was greater than or equal to {other}.");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentEqual<T>(T value, T other, string? paramName = null)
            where T : IEquatable<T>
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfEqual(value, other, paramName);
#else
            if (value.Equals(other))
            {
                throw new ArgumentOutOfRangeException(paramName, value, $"The value was equal to {other}.");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentNotEqual<T>(T value, T other, string? paramName = null)
            where T : IEquatable<T>
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfNotEqual(value, other, paramName);
#else
            if (!value.Equals(other))
            {
                throw new ArgumentOutOfRangeException(paramName, value, $"The value was not equal to {other}.");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentLessThanOrEqual<T>(T value, T other, string? paramName = null)
            where T : IComparable<T>
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, other, paramName);
#else
            if (value.CompareTo(other) <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, $"The value was less than or equal to {other}.");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentLessThan<T>(T value, T other, string? paramName = null)
            where T : IComparable<T>
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfLessThan(value, other, paramName);
#else
            if (value.CompareTo(other) < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, $"The value was less than {other}.");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentZero<T>(T value, string? paramName = null)
#if NET8_0_OR_GREATER
            where T : INumberBase<T>
        {
            ArgumentOutOfRangeException.ThrowIfZero(value, paramName);
        }
#else
    where T : struct, IEquatable<T>
        {
            if (value.Equals(default))
            {
                throw new ArgumentOutOfRangeException(paramName, value, "The value was zero.");
            }
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentNegative<T>(T value, string? paramName = null)
#if NET8_0_OR_GREATER
            where T : INumberBase<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value, paramName);
        }
#else
    where T : struct, IComparable<T>
        {
            if (value.CompareTo(default) < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, "The value was negative.");
            }
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentNegativeOrZero<T>(T value, string? paramName = null)
#if NET8_0_OR_GREATER
            where T : INumberBase<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value, paramName);
        }
#else
    where T : struct, IComparable<T>
        {
            if (value.CompareTo(default) <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName, value, "The value was negative or zero.");
            }
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentNull(object? argument, string? paramName = null)
        {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(argument, paramName);
#else
            if (argument == null)
            {
                throw new ArgumentNullException(paramName, "The argument was null.");
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentNullOrEmpty(string? argument, string? paramName = null)
        {
#if NET8_0_OR_GREATER
            ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
#else
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException("The argument was null or empty.", paramName);
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfArgumentNullOrWhiteSpace(string? argument, string? paramName = null)
        {
#if NET8_0_OR_GREATER
            ArgumentException.ThrowIfNullOrWhiteSpace(argument, paramName);
#else
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException("The argument was null or whitespace.", paramName);
            }
#endif
        }
    }
}
