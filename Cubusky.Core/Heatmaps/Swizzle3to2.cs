namespace Cubusky.Heatmaps
{
    /// <summary>Represents the swizzle options for converting a 3D vector to a 2D vector.</summary>
    public enum Swizzle3to2 : byte
    {
        /// <summary>Swizzle option to convert the X and Y components of a 3D vector to a 2D vector.</summary>
        XY,

        /// <summary>Swizzle option to convert the X and Z components of a 3D vector to a 2D vector.</summary>
        XZ,

        /// <summary>Swizzle option to convert the Y and Z components of a 3D vector to a 2D vector.</summary>
        YZ,
    }
}
