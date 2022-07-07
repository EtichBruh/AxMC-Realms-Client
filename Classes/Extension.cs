using Microsoft.Xna.Framework;

namespace nekoT
{
    public static class Extension
    {
        /// <summary>
        /// Compares Vector2 components to Vector2 B components
        /// </summary>
        /// <param name="a">First vector2</param>
        /// <param name="b">Second vector2</param>
        /// <returns><see langword="true"/> if Vector2 a X Y is greater than Vector2 b X Y </returns>
        public static bool More(this Vector2 a, Vector2 b) { return (a.X > b.X && a.Y > b.Y); }
        /// <summary>
        /// Compares Vector2 A components to B 
        /// </summary>
        /// <param name="a">First vector2</param>
        /// <param name="b">Second vector2</param>
        /// <returns><see langword="true"/> if Vector2 a X Y is greater than b</returns>
        public static bool More(this Vector2 a, float b) { return (a.X > b && a.Y > b); }
        /// <summary>
        /// Compares Vector2 A to Vector2 B 
        /// </summary>
        /// <param name="a">First vector2</param>
        /// <param name="b">Second vector2</param>
        /// <returns><see langword="true"/> if Vector2 a X Y is lower than Vector2 b X Y </returns>
        public static bool Less(this Vector2 a, Vector2 b) { return (a.X < b.X && a.Y < b.Y); }
        public static Vector2 Increase(this Vector2 a,float b) { a.Y += b; return a; }
        /// <summary>
        /// Compares Vector2 A to Point B 
        /// </summary>
        /// <param name="a">First vector2</param>
        /// <param name="b">Second vector2</param>
        /// <returns><see langword="true"/> if Vector2 a X Y is lower than Point b X Y </returns>
        public static bool Less(this Vector2 a, Point b) { return a.X < b.X && a.Y < b.Y; }
        /// <summary>
        /// Compares Vector2 A components to B 
        /// </summary>
        /// <param name="a">Vector2 to compare</param>
        /// <param name="b">Point to compare with</param>
        /// <returns><see langword="true"/> if Vector2 a X Y is lower than b</returns>
        public static bool Less(this Vector2 a, float b) { return (a.X < b && a.Y < b); }
        /// <summary>
        /// Multiplies <see cref="Microsoft.Xna.Framework.Point"/> components by <paramref name="b"/>
        /// </summary>
        /// <param name="a">Point</param>
        /// <param name="b">Multiply value</param>
        /// <returns><paramref name="a"/> with components multiplied by <paramref name="b"/></returns>
        public static Point MultiplyBy(this Point a, int b) { a.X *= b; a.Y *= b; return a; }
        /// <summary>
        /// Multiplies <see cref="Microsoft.Xna.Framework.Point"/> components by <paramref name="b"/>
        /// </summary>
        /// <param name="a">Point</param>
        /// <param name="b">Multiply value</param>
        /// <returns><paramref name="a"/> with components multiplied by <paramref name="b"/></returns>
        public static Point MultiplyBy(this Point a, float b) { a.X = (int)(a.X * b); a.Y = (int)(a.Y* b); return a; }
        /// <summary>
        /// Divides <see cref="Microsoft.Xna.Framework.Point"/> components by <paramref name="b"/>
        /// </summary>
        /// <param name="a">Point</param>
        /// <param name="b">Divide value</param>
        /// <returns><paramref name="a"/> with components divided by <paramref name="b"/></returns>
        public static Point DivideBy(this Point a, int b) { a.X /= b; a.Y /= b; return a; }
        /// <summary>
        /// Describes <see cref="Microsoft.Xna.Framework.Vector3"/> as <see cref="Microsoft.Xna.Framework.Vector2"/>
        /// </summary>
        /// <param name="a">Vector3 that should be described as vector2</param>
        /// <param name="allocatedVector2">Some allocated vector2</param>
        public static Vector2 AsVector2(this Vector3 a, Vector2 allocatedVector2) { allocatedVector2.X = a.X; allocatedVector2.Y = a.Y; return allocatedVector2; }
        private static Matrix _empty = new Matrix();
        public static Matrix MatrixEmpty => _empty;
        /// <summary>
        /// Describes <see cref="Microsoft.Xna.Framework.Vector2"/> as <see cref="Microsoft.Xna.Framework.Vector3"/>
        /// </summary>
        /// <param name="a">Vector2 that should be described as vector3</param>
        /// <param name="allocatedVector2">Some allocated vector3</param>
        public static Vector3 AsVector3(this Vector2 a, Vector3 allocatedVector3) { allocatedVector3.X = a.X; allocatedVector3.Y = a.Y; return allocatedVector3; }
        private static byte[] empty = { 0, 0, 0, 0 };
        public static byte[] ToByte(this Vector2 a)
        {
            empty[1] = (byte)((int)a.X >> 8);
            empty[0] = (byte)((int)a.X & 255);
            empty[3] = (byte)((int)a.Y >> 8);
            empty[2] = (byte)((int)a.Y & 255);
            return empty;
        }
        public static Vector2 AsVector2(this byte[] b)
        {
            var result = Vector2.Zero;
            result.X = (ushort)((b[1] << 8) + b[0]);
            result.Y = (ushort)((b[3] << 8) + b[2]);
            return result;
        }
    }
}