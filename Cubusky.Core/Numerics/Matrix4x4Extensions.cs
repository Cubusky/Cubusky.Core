using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cubusky.Numerics
{
    /// <summary>Provides extension methods for the <see cref="Matrix4x4"/> struct.</summary>
    public static class Matrix4x4Extensions
    {
        /// <summary>Creates a transformation matrix from the specified vector position, rotation from a unit vector and an angle to rotate around the vector, and single-precision scalar.</summary>
        /// <inheritdoc cref="doc_CreateTransformation" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransformation(Vector3 position, Vector3 axis, float angle, float scale)
            => CreateTransformation(position, Quaternion.CreateFromAxisAngle(axis, angle), Vector3.One * scale);

        /// <summary>Creates a transformation matrix from the specified vector position, rotation from a unit vector and an angle to rotate around the vector, and vector scale.</summary>
        /// <inheritdoc cref="doc_CreateTransformation" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransformation(Vector3 position, Vector3 axis, float angle, Vector3 scales)
            => CreateTransformation(position, Quaternion.CreateFromAxisAngle(axis, angle), scales);

        /// <summary>Creates a transformation matrix from the specified vector position, rotation from the given yaw, pitch, and roll, and single-precision scalar.</summary>
        /// <inheritdoc cref="doc_CreateTransformation" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransformation(Vector3 position, float yaw, float pitch, float roll, float scale)
            => CreateTransformation(position, Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll), Vector3.One * scale);

        /// <summary>Creates a transformation matrix from the specified vector position, rotation from the given yaw, pitch, and roll, and vector scale.</summary>
        /// <inheritdoc cref="doc_CreateTransformation" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransformation(Vector3 position, float yaw, float pitch, float roll, Vector3 scales)
            => CreateTransformation(position, Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll), scales);

        /// <summary>Creates a transformation matrix from the specified vector position, Quaternion rotation, and single-precision scalar.</summary>
        /// <inheritdoc cref="doc_CreateTransformation" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransformation(Vector3 position, Quaternion quaternion, float scale)
            => CreateTransformation(position, quaternion, Vector3.One * scale);

        /// <summary>Creates a transformation matrix from the specified vector position, Quaternion rotation, and vector scale.</summary>
        /// <inheritdoc cref="doc_CreateTransformation" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4 CreateTransformation(Vector3 position, Quaternion quaternion, Vector3 scales)
        {
            var translationMatrix = Matrix4x4.CreateTranslation(position);
            var rotationMatrix = Matrix4x4.CreateFromQuaternion(quaternion);
            var scaleMatrix = Matrix4x4.CreateScale(scales);

            // Combine the matrices: scale -> rotate -> translate
            return scaleMatrix * rotationMatrix * translationMatrix;
        }

        /// <param name="position">The amount to translate in each axis.</param>
        /// <param name="quaternion">The source Quaternion.</param>
        /// <param name="axis">The unit vector to rotate around.</param>
        /// <param name="angle">The angle, in radians, to rotate around the axis vector.</param>
        /// <param name="yaw">The yaw angle, in radians, around the Y axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the X axis.</param>
        /// <param name="roll">The roll angle, in radians, around the Z axis.</param>
        /// <param name="radians">The amount of rotation, in radians.</param>
        /// <param name="scale">The scale to use.</param>
        /// <param name="scales">The scales to use.</param>
        /// <returns>The transformation matrix.</returns>
        internal static Matrix4x4 doc_CreateTransformation(Vector3 position, Quaternion quaternion, Vector3 axis, float angle, float yaw, float pitch, float roll, float radians, float scale, Vector3 scales) => default;

        //public static Matrix4x4 CreateTransformation(Vector3 position, Quaternion quaternion, float scale, Vector3 centerPoint)
        //public static Matrix4x4 CreateTransformation(Vector3 position, Quaternion quaternion, Vector3 scales, Vector3 centerPoint)
    }
}
