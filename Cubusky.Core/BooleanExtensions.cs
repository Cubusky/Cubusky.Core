using System.Numerics;

namespace Cubusky
{
    internal static class BooleanExtensions
    {
        public static int ToInt(this bool value) => value ? 1 : 0;
        public static int ToInt(this bool value, int ifTrue) => value ? ifTrue : 0;
        public static int ToInt(this bool value, int ifTrue, int ifFalse) => value ? ifTrue : ifFalse;

#if NET8_0_OR_GREATER
        public static TNumber ToNumber<TNumber>(this bool value)
            where TNumber : INumberBase<TNumber>
            => value ? TNumber.One : TNumber.Zero;

        public static TNumber ToNumber<TNumber>(this bool value, TNumber ifTrue)
            where TNumber : INumberBase<TNumber>
            => value ? ifTrue : TNumber.Zero;

        public static TNumber ToNumber<TNumber>(this bool value, TNumber ifTrue, TNumber ifFalse)
            where TNumber : INumberBase<TNumber>
            => value ? ifTrue : ifFalse;
#endif
    }
}
