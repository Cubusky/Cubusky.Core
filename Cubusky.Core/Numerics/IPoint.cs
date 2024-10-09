#if NET8_0_OR_GREATER
using System.Numerics;

namespace Cubusky.Numerics
{
    /// <summary>Defines a vector type.</summary>
    /// <typeparam name="TSelf">The type that implements the interface.</typeparam>
    public interface IPoint<TSelf>
        : IAdditionOperators<TSelf, TSelf, TSelf>
        , IAdditiveIdentity<TSelf, TSelf>
        , IDivisionOperators<TSelf, TSelf, TSelf>
        , IDecrementOperators<TSelf>
        , IEqualityOperators<TSelf, TSelf, bool>
        , IIncrementOperators<TSelf>
        , IModulusOperators<TSelf, TSelf, TSelf>
        , IMultiplicativeIdentity<TSelf, TSelf>
        , IMultiplyOperators<TSelf, TSelf, TSelf>
        , ISubtractionOperators<TSelf, TSelf, TSelf>
        , IUnaryNegationOperators<TSelf, TSelf>
        , IUnaryPlusOperators<TSelf, TSelf>
        where TSelf : IPoint<TSelf>
    {
        /// <summary>Gets the value <c>0</c> for the type.</summary>
        static abstract TSelf Zero { get; }

        /// <summary>Gets the value <c>1</c> for the type.</summary>
        static abstract TSelf One { get; }
    }
}
#endif
