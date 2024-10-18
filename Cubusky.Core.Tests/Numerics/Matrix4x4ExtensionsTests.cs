using System.Numerics;
using Xunit;

namespace Cubusky.Numerics.Tests
{
    public class Matrix4x4ExtensionsTests
    {
        [Fact]
        public void CreateTransformation_PositionAxisAngleScale()
        {
            // Arrange
            var position = new Vector3(1.465f, -1.095f, -1.85f);
            var axis = Vector3.UnitY;
            var angle = 2.0263264f;
            //var scale = 2f;
            var scales = new Vector3(2.0000045f, 2f, 2.0000045f);

            var expectedMatrix = new Matrix4x4(
                -0.879879f, 0f, -1.79606f, 0f,
                0f, 2f, 0f, 0f,
                1.79606f, 0f, -0.879879f, 0f,
                1.465f, -1.095f, -1.85f, 1f
            );

            // Act
            var result = Matrix.CreateTransformation4x4(position, axis, angle, scales);

            // Assert
            AssertMatrix4x4(expectedMatrix, result);
        }

        [Fact]
        public void CreateTransformation_PositionYawPitchRollScale()
        {
            // Arrange
            var position = new Vector3(1.465f, -1.095f, -1.85f);
            var yaw = -1.4533058f;
            var pitch = 0.73660237f;
            var roll = -1.8669899f;
            //var scale = 2f;
            var scales = new Vector3(2.000004f, 1.9999976f, 2.0000021f);

            var expectedMatrix = new Matrix4x4(
                1.2077587f, -1.4170002f, -0.73037356f, 0f,
                0.61368513f, -0.43242517f, 1.8537501f, 0f,
                -1.4712985f, -1.3435514f, 0.1736634f, 0f,
                1.465f, -1.095f, -1.85f, 1f
            );

            // Act
            var result = Matrix.CreateTransformation4x4(position, yaw, pitch, roll, scales);

            // Assert
            AssertMatrix4x4(expectedMatrix, result);
        }

        [Fact]
        public void CreateTransformation_PositionQuaternionScale()
        {
            // Arrange
            var position = new Vector3(1.465f, -1.095f, -1.85f);
            var quaternion = new Quaternion(0.65826652f, -0.15254301f, -0.4180808f, 0.6071443f);
            //var scale = 2f;
            var scales = new Vector3(2.000004f, 1.9999976f, 2.0000021f);

            var expectedMatrix = new Matrix4x4(
                1.20776f, -1.417f, -0.730372f, 0f,
                0.613685f, -0.432426f, 1.85375f, 0f,
                -1.4713f, -1.34355f, 0.173662f, 0f,
                1.465f, -1.095f, -1.85f, 1f
            );

            // Act
            var result = Matrix.CreateTransformation4x4(position, quaternion, scales);

            // Assert
            AssertMatrix4x4(expectedMatrix, result);
        }

        [Fact]
        public void CreateTransformation_PositionQuaternionScales()
        {
            // Arrange
            var position = new Vector3(1.465f, -1.095f, -1.85f);
            var quaternion = new Quaternion(0.65826652f, -0.15254301f, -0.4180808f, 0.6071443f);
            var scales = new Vector3(1.2699995f, 0.67000014f, 1.6050038f);

            var expectedMatrix = new Matrix4x4(
                0.766925f, -0.899793f, -0.463786f, 0f,
                0.205585f, -0.144863f, 0.621007f, 0f,
                -1.18072f, -1.0782f, 0.139364f, 0f,
                1.465f, -1.095f, -1.85f, 1f
            );

            // Act
            var result = Matrix.CreateTransformation4x4(position, quaternion, scales);

            // Assert
            AssertMatrix4x4(expectedMatrix, result);
        }

        private static void AssertMatrix4x4(Matrix4x4 expected, Matrix4x4 result)
        {
            Assert.Multiple(
                () => Assert.Equal(expected.M11, result.M11, 1E-5f),
                () => Assert.Equal(expected.M12, result.M12, 1E-5f),
                () => Assert.Equal(expected.M13, result.M13, 1E-5f),
                () => Assert.Equal(expected.M14, result.M14, 1E-5f),
                () => Assert.Equal(expected.M21, result.M21, 1E-5f),
                () => Assert.Equal(expected.M22, result.M22, 1E-5f),
                () => Assert.Equal(expected.M23, result.M23, 1E-5f),
                () => Assert.Equal(expected.M24, result.M24, 1E-5f),
                () => Assert.Equal(expected.M31, result.M31, 1E-5f),
                () => Assert.Equal(expected.M32, result.M32, 1E-5f),
                () => Assert.Equal(expected.M33, result.M33, 1E-5f),
                () => Assert.Equal(expected.M34, result.M34, 1E-5f),
                () => Assert.Equal(expected.M41, result.M41, 1E-5f),
                () => Assert.Equal(expected.M42, result.M42, 1E-5f),
                () => Assert.Equal(expected.M43, result.M43, 1E-5f),
                () => Assert.Equal(expected.M44, result.M44, 1E-5f)
            );
        }
    }
}
