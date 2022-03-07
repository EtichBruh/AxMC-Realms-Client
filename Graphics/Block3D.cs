using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace nekoT
{
    public class Block3D
    {
        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;

        public BasicEffect BasiceCubeEff;
        public static int SmthIndicesDevidedByThree;
        public Block3D(GraphicsDevice gd)
        {
            const float x = 25f;
            const float y = 50f;
            const float z = 25f;
            Vector3[] Vertices = {
                new Vector3(-x, y, z), new Vector3(x, y, z),new Vector3(x, -y, z),new Vector3(-x, -y, z),
                new(-x, y, -z),new(x, y, -z),new(x, -y, -z),new(-x, -y, -z),
                new(-x, y, z), new(x, y, z), new(x, y, -z), new(-x, y, -z),
                new(-x, -y, z),new(x, -y, z),new(x, -y, -z),new(-x, -y, -z),
                new(-x, y, -z),new(-x, y, z),new(-x, -y, z),new(-x, -y, -z),
                new(x, y, -z), new(x, y, z), new(x, -y, z), new(x, -y, -z)
        };
            Vector2[] TextCoords = {
                new Vector2(0, 0),new Vector2(1, 0),new Vector2(1, 1),new Vector2(0, 1),
                new(1, 0),new(0, 0),new(0, 1),new(1, 1),
                new(0, 1),new(1, 1),new(1, 0),new(0, 0),
                new(0, 0),new(1, 0),new(1, 1),new(0, 1),
                new(0, 0),new(1, 0),new(1, 1),new(0, 1),
                new(1, 0),new(0, 0),new(0, 1),new(1, 1),
        };
            Vector2[] uvMap =
            {
                new Vector2(0, 0),new Vector2(.25f, 0),new Vector2(.25f, .25f),new Vector2(0, .25f), // Front Face: Top left texture (white)
                new Vector2(.5f, 0),new Vector2(.25f, 0),new Vector2(.25f, .25f),new Vector2(.5f, .25f), // Back Face: dark oak wood log side
                new Vector2(.75f, 0),new Vector2(.5f, 0),new Vector2(.5f, .25f),new Vector2(.75f, .25f), // Top Face: wood log top
                new Vector2(.75f, .25f),new Vector2(1, .25f),new Vector2(1, .5f),new Vector2(.75f, .5f), // Bottom Face :   Yellow
                new Vector2(0, .5f),new Vector2(.25f, .5f),new Vector2(.25f, .75f),new Vector2(0, .75f), // LEft Face : Green
                new Vector2(1, 0),new Vector2(.75f, 0),new Vector2(.75f, .25f),new Vector2(1, .25f), // Right Face : Dark Red
            };
            Vector3[] Normals =
            {
                Vector3.Backward,Vector3.Backward,Vector3.Backward,Vector3.Backward,
                Vector3.Forward,Vector3.Forward,Vector3.Forward,Vector3.Forward,
                Vector3.Up,Vector3.Up,Vector3.Up,Vector3.Up,
                Vector3.Down,Vector3.Down,Vector3.Down,Vector3.Down,
                Vector3.Left,Vector3.Left,Vector3.Left,Vector3.Left,
                Vector3.Right,Vector3.Right,Vector3.Right,Vector3.Right,
            };
            ushort[] _triangleIndices =
            {
                0, 1, 2, 2, 3, 0, // Front
                4, 7, 6, 6, 5, 4, // Back
                8, 11, 10, 10, 9, 8, // Top
                12, 13, 14, 14, 15, 12, // Bottom
                16, 17, 18, 18, 19, 16, // Left
                20, 23, 22, 22, 21, 20, // Right
            };
            VertexPositionNormalTexture[] _tVertices = new VertexPositionNormalTexture[Vertices.Length];
            for (int v = 0; v < Vertices.Length; v++)
                _tVertices[v] = new VertexPositionNormalTexture(Vertices[v], Normals[v], uvMap[v]);

            VertexBuffer = new(gd, typeof(VertexPositionNormalTexture), _tVertices.Length, BufferUsage.None);
            VertexBuffer.SetData(_tVertices);

            BasiceCubeEff = new(gd) { TextureEnabled = true };

            IndexBuffer = new IndexBuffer(gd, typeof(ushort), _triangleIndices.Length, BufferUsage.WriteOnly);// init buffer of indexes
            IndexBuffer.SetData(_triangleIndices);
            SmthIndicesDevidedByThree = _triangleIndices.Length / 3;
        }
    }
}