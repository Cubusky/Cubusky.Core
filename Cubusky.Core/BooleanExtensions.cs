using System.Runtime.CompilerServices;
#if NET8_0_OR_GREATER
using System.Numerics;
#endif

namespace Cubusky
{
    /// <summary>Provides extension methods for booleans.</summary>
    public static class BooleanExtensions
    {
        /// <summary>Converts a boolean to 1 if true or 0 if false.</summary>
        /// <param name="value">The boolean to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this bool value)
            => value ? 1 : 0;

        /// <summary>Converts a boolean to a number.</summary>
        /// <param name="value">The boolean to convert.</param>
        /// <param name="ifTrue">The number to return if the boolean is true, otherwise 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this bool value, int ifTrue)
            => value ? ifTrue : 0;

        /// <summary>Converts a boolean to a number.</summary>
        /// <param name="value">The boolean to convert.</param>
        /// <param name="ifTrue">The number to return if the boolean is true.</param>
        /// <param name="ifFalse">The number to return if the boolean is false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this bool value, int ifTrue, int ifFalse)
            => value ? ifTrue : ifFalse;

        /// <summary>Converts a nullable boolean to a number.</summary>
        /// <param name="value">The boolean to convert.</param>
        /// <param name="ifTrue">The number to return if the boolean is true.</param>
        /// <param name="ifFalse">The number to return if the boolean is false.</param>
        /// <param name="ifNull">The number to return if the boolean is null.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this bool? value, int ifTrue, int ifFalse, int ifNull)
            => value.HasValue ? value.Value ? ifTrue : ifFalse : ifNull;

#if NET8_0_OR_GREATER
        /// <summary>Converts a boolean to 1 if true or 0 if false.</summary>
        /// <param name="value">The boolean to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToNumber<T>(this bool value)
            where T : INumberBase<T> => value ? T.One : T.Zero;

        /// <summary>Converts a boolean to a number.</summary>
        /// <param name="value">The boolean to convert.</param>
        /// <param name="ifTrue">The number to return if the boolean is true, otherwise 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToNumber<T>(this bool value, T ifTrue)
            where T : INumberBase<T> => value ? ifTrue : T.Zero;

        /// <summary>Converts a boolean to a number.</summary>
        /// <param name="value">The boolean to convert.</param>
        /// <param name="ifTrue">The number to return if the boolean is true.</param>
        /// <param name="ifFalse">The number to return if the boolean is false.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToNumber<T>(this bool value, T ifTrue, T ifFalse)
            where T : INumberBase<T> => value ? ifTrue : ifFalse;

        /// <summary>Converts a nullable boolean to a number.</summary>
        /// <param name="value">The boolean to convert.</param>
        /// <param name="ifTrue">The number to return if the boolean is true.</param>
        /// <param name="ifFalse">The number to return if the boolean is false.</param>
        /// <param name="ifNull">The number to return if the boolean is null.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToNumber<T>(this bool? value, T ifTrue, T ifFalse, T ifNull)
            where T : INumberBase<T> => value.HasValue ? value.Value ? ifTrue : ifFalse : ifNull;
#endif
    }
}
