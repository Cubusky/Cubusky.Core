using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cubusky.Numerics
{
    /// <summary>Provides extension methods for the <see cref="Matrix3x2"/> struct.</summary>
    public static class Matrix3x2Extensions
    {
        /// <summary>Creates a transformation matrix from the specified vector position, single-precision rotation in radians, and single-precision scalar.</summary>
        /// <inheritdoc cref="Matrix4x4Extensions.doc_CreateTransformation(Vector3, Quaternion, Vector3, float, float, float, float, float, float, Vector3)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 CreateTransformation(Vector2 position, float radians, float scale)
            => CreateTransformation(position, radians, Vector2.One * scale);

        /// <summary>Creates a transformation matrix from the specified vector position, single-precision rotation in radians, and vector scale.</summary>
        /// <inheritdoc cref="Matrix4x4Extensions.doc_CreateTransformation(Vector3, Quaternion, Vector3, float, float, float, float, float, float, Vector3)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix3x2 CreateTransformation(Vector2 position, float radians, Vector2 scales)
        {
            var translationMatrix = Matrix3x2.CreateTranslation(position);
            var rotationMatrix = Matrix3x2.CreateRotation(radians);
            var scaleMatrix = Matrix3x2.CreateScale(scales);

            // Combine the matrices: scale -> rotate -> translate
            return scaleMatrix * rotationMatrix * translationMatrix;
        }
    }
}
