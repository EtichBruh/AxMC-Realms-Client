using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace nekoT
{
    public class Block3D
    {
        private readonly VertexPositionColorTexture[] _triangleVertices = new VertexPositionColorTexture[8];
        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;

        private readonly ushort[] _triangleIndices =
            {
                0,1,2, // Front side
                2,3,0,

                6,5,4, // Back side
                4,7,6,

                4,0,3, // Left Side
                3,7,4,

                1,5,6, // Rigth side
                6,2,1,

                4,5,1, // Up
                1,0,4,

                3,2,6, // Down
                6,7,3,
            };
        public BasicEffect BasiceCubeEff;
        public void BlockInit(GraphicsDevice gd)
        {
            const float x = 25f;
            const float y = 25f;
            const float z = 25f;
            _triangleVertices[0] = new VertexPositionColorTexture(new Vector3(x, -y, -z), Color.White, Vector2.UnitX);
            _triangleVertices[1] = new(new(-x, -y, -z), Color.White, Vector2.Zero);
            _triangleVertices[2] = new(new(-x, y, -z), Color.White, Vector2.UnitY);
            _triangleVertices[3] = new(new(x, y, -z), Color.White, Vector2.One);

            _triangleVertices[4] = new(-_triangleVertices[2].Position, Color.White, Vector2.One);
            _triangleVertices[5] = new(-_triangleVertices[3].Position, Color.White, Vector2.UnitY);
            _triangleVertices[6] = new(-_triangleVertices[0].Position, Color.White, Vector2.Zero);
            _triangleVertices[7] = new(-_triangleVertices[1].Position, Color.White, Vector2.UnitX);

            VertexBuffer = new(gd, typeof(VertexPositionColorTexture), _triangleVertices.Length, BufferUsage.None);
            VertexBuffer.SetData(_triangleVertices);

            BasiceCubeEff = new(gd) { VertexColorEnabled = true, TextureEnabled = true };

            IndexBuffer = new IndexBuffer(gd, typeof(ushort), _triangleIndices.Length, BufferUsage.WriteOnly);// init buffer of indexes
            IndexBuffer.SetData(_triangleIndices);
           // World = Matrix.CreateWorld(new Vector3(-3 * 50 +25, -5 * 50+25, 0), Vector3.Forward, Vector3.Up);
           // World = Matrix.CreateTranslation(new Vector3(-3 * 50 +25, 5 * 50+25, 0));
        }
    }
}