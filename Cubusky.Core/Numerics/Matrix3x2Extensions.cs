using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cubusky.Numerics
{
    public static partial class Matrix
    {
        /// <summary>Creates a transformation matrix from the specified vector position, single-precision rotation in radians, and single-precision scalar.</summary>
        /// <inheritdoc cref="doc_CreateTransformation" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 CreateTransformation3x2(Vector2 position, float radians, float scale)
            => CreateTransformation3x2(position, radians, Vector2.One * scale);

        /// <summary>Creates a transformation matrix from the specified vector position, single-precision rotation in radians, and vector scale.</summary>
        /// <inheritdoc cref="doc_CreateTransformation" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 CreateTransformation3x2(Vector2 position, float radians, Vector2 scales)
        {
            var translationMatrix = Matrix3x2.CreateTranslation(position);
            var rotationMatrix = Matrix3x2.CreateRotation(radians);
            var scaleMatrix = Matrix3x2.CreateScale(scales);

            // Combine the matrices: scale -> rotate -> translate
            return scaleMatrix * rotationMatrix * translationMatrix;
        }
    }
}
