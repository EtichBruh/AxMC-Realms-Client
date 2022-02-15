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
        /// Describes <see cref="Microsoft.Xna.Framework.Vector3"/> as <see cref="Microsoft.Xna.Framework.Vector2"/>
        /// </summary>
        /// <param name="a">Vector3 that should be described as vector2</param>
        /// <param name="allocatedVector2">Some allocated vector2</param>
        public static Vector2 AsVector2(this Vector3 a, Vector2 allocatedVector2) { allocatedVector2.X = a.X; allocatedVector2.Y = a.Y; return allocatedVector2; }
    }
}