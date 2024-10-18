using System.Numerics;
using Xunit;

namespace Cubusky.Numerics.Tests
{
    public class Matrix3x2ExtensionsTests
    {
        [Fact]
        public void CreateTransformation_PositionRadiansScale()
        {
            // Arrange
            var position = new Vector2(15, -5);
            var radians = 2.39285f;
            var scale = 2f;

            var expectedMatrix = new Matrix3x2(
                -1.4650906f, 1.3614366f,
                -1.3614366f, -1.4650906f,
                15f, -5f
            );

            // Act
            var resultMatrix = Matrix.CreateTransformation3x2(position, radians, scale);

            // Assert
            Assert.Equal(expectedMatrix, resultMatrix);
        }

        [Fact]
        public void CreateTransformation_PositionRadiansScales()
        {
            // Arrange
            var position = new Vector2(15, -5);
            var radians = 2.39285f;
            var scales = new Vector2(0.8f, 1.46f);

            var expectedMatrix = new Matrix3x2(
                -0.58603626f, 0.5445747f,
                -0.99384874f, -1.0695162f,
                15f, -5f
            );

            // Act
            var resultMatrix = Matrix.CreateTransformation3x2(position, radians, scales);

            // Assert
            Assert.Equal(expectedMatrix, resultMatrix);
        }

        [Fact]
        public void CreateTransformation_PositionSkew()
        {
            var position = new Vector2(15, -5);
            var skew = 0.657989f;

            var expectedMatrix = new Matrix3x2(
                1f, 0f,
                -0.61152697f, 0.7912236f,
                15f, -5f
            );

            // Act
            var resultMatrix = Matrix.CreateTransformation3x2(position, 0f, Vector2.One, Vector2.UnitX * skew);

            // Assert
            Assert.Equal(expectedMatrix, resultMatrix);
        }

        [Fact]
        public void CreateTransformation_PositionRadiansSkew()
        {
            var position = new Vector2(15, -5);
            var radians = 2.39285f;
            var skew = 0.657989f;

            var expectedMatrix = new Matrix3x2(
                -0.7325453f, 0.6807183f,
                -0.09062918f, -0.9958847f,
                15f, -5f
            );

            // Act
            var resultMatrix = Matrix.CreateTransformation3x2(position, radians, Vector2.One, Vector2.UnitX * skew);

            // Assert
            Assert.Equal(expectedMatrix, resultMatrix);
        }

        [Fact]
        public void CreateTransformation_PositionRadiansScalesSkew()
        {
            // Arrange
            var position = new Vector2(15, -5);
            var radians = 2.39285f;
            var scales = new Vector2(0.8f, 1.46f);
            var skew = 0.657989f;

            var expectedMatrix = new Matrix3x2(
                -0.58603626f, 0.5445747f,
                -0.13231862f, -1.4539918f,
                15f, -5f
            );

            // Act
            var resultMatrix = Matrix.CreateTransformation3x2(position, radians, scales, Vector2.UnitX * skew);

            // Assert
            Assert.Equal(expectedMatrix, resultMatrix);
        }
    }
}
