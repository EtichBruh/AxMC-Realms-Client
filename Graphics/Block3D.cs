using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace nekoT
{
    public class Block3D
    {
        private readonly VertexPositionColorTexture[] _tVertices = new VertexPositionColorTexture[36];
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
        /*
        // Our vertices. Three consecutive floats give a 3D vertex; Three consecutive vertices give a triangle.
// A cube has 6 faces with 2 triangles each, so this makes 6*2=12 triangles, and 12*3 vertices
static const GLfloat g_vertex_buffer_data[] = {
 [0]  -1.0f,-1.0f,-1.0f, // triangle 1 : begin
 [1]   -1.0f,-1.0f, 1.0f,
 [2]   -1.0f, 1.0f, 1.0f, // triangle 1 : end
 [3]   1.0f, 1.0f,-1.0f, // triangle 2 : begin
 [4]   [0]
 [5]   -1.0f, 1.0f,-1.0f, // triangle 2 : end
 [6]   -[5]
 [7]   [0]
 [8]   -[2]
 [9]   [3]
[10]  -[2]
[11]    [0]
[12]    [0]
[13]    [2]
[14]    [5]
[15]    -[5]
[16]    [1]
[17]    [0]
[18]    [2]
[19]    [1]
[20]    -[5]
[21]    -[0]
[22]    -[2]
[23]    [3]
[24]    -[2]
[25]    -[0]
[26]    -[5]
[27]    -[0]
[28]    [3]
[29]    [5]
[30]    [21]
[31]    [15]
[32]    [2]
[33]  [21]
[34]    [2]
[35]    [15]

            0.583f,  0.771f,  0.014f,
    0.609f,  0.115f,  0.436f,
    0.327f,  0.483f,  0.844f,
    0.822f,  0.569f,  0.201f,
    0.435f,  0.602f,  0.223f,
    0.310f,  0.747f,  0.185f,
    0.597f,  0.770f,  0.761f,
    0.559f,  0.436f,  0.730f,
    0.359f,  0.583f,  0.152f,
    0.483f,  0.596f,  0.789f,
    0.559f,  0.861f,  0.639f,
    0.195f,  0.548f,  0.859f,
    0.014f,  0.184f,  0.576f,
    0.771f,  0.328f,  0.970f,
    0.406f,  0.615f,  0.116f,
    0.676f,  0.977f,  0.133f,
    0.971f,  0.572f,  0.833f,
    0.140f,  0.616f,  0.489f,
    0.997f,  0.513f,  0.064f,
    0.945f,  0.719f,  0.592f,
    0.543f,  0.021f,  0.978f,
    0.279f,  0.317f,  0.505f,
    0.167f,  0.620f,  0.077f,
    0.347f,  0.857f,  0.137f,
    0.055f,  0.953f,  0.042f,
    0.714f,  0.505f,  0.345f,
    0.783f,  0.290f,  0.734f,
    0.722f,  0.645f,  0.174f,
    0.302f,  0.455f,  0.848f,
    0.225f,  0.587f,  0.040f,
    0.517f,  0.713f,  0.338f,
    0.053f,  0.959f,  0.120f,
    0.393f,  0.621f,  0.362f,
    0.673f,  0.211f,  0.457f,
    0.820f,  0.883f,  0.371f,
    0.982f,  0.099f,  0.879f
};
        */
        public void BlockInit(GraphicsDevice gd)
        {
            const float x = 25f;
            const float y = 25f;
            const float z = 25f;
            _tVertices[0] = new VertexPositionColorTexture(new Vector3(-x, -y, -z), Color.White, Vector2.One);
            _tVertices[1] = new(new(-x, -y, z), Color.White, Vector2.One);
            _tVertices[2] = new(new(-x, y, z), Color.White, Vector2.UnitX);

            _tVertices[3] = new(new(x, y, -z), Color.White, Vector2.Zero);
            _tVertices[4] = new(_tVertices[0].Position, Color.White, Vector2.One);
            _tVertices[5] = new(new(-x, y, -z), Color.White, Vector2.UnitX);

            _tVertices[6] = new(-_tVertices[5].Position, Color.White, Vector2.UnitY);//
            _tVertices[7] = new(_tVertices[0].Position, Color.White, Vector2.One);//
            _tVertices[8] = new(-_tVertices[2].Position, Color.White, Vector2.UnitY);//

            _tVertices[9] = new(_tVertices[3].Position, Color.White, Vector2.One);//
            _tVertices[10] = new(_tVertices[8].Position, Color.White, Vector2.UnitX);//
            _tVertices[11] = new(_tVertices[0].Position, Color.White, Vector2.One);//

            _tVertices[12] = new(_tVertices[0].Position, Color.White, Vector2.One);//
            _tVertices[13] = new(_tVertices[2].Position, Color.White, Vector2.UnitY);//
            _tVertices[14] = new(_tVertices[5].Position, Color.White, Vector2.UnitX);//

            _tVertices[15] = new(_tVertices[6].Position, Color.White, Vector2.UnitX);//
            _tVertices[16] = new(_tVertices[1].Position, Color.White, Vector2.Zero);//
            _tVertices[17] = new(_tVertices[0].Position, Color.White, Vector2.Zero);//

            _tVertices[18] = new(_tVertices[2].Position, Color.White, Vector2.UnitY);//
            _tVertices[19] = new(_tVertices[1].Position, Color.White, Vector2.Zero);//
            _tVertices[20] = new(_tVertices[6].Position, Color.White, Vector2.Zero);//

            _tVertices[21] = new(-_tVertices[0].Position, Color.White, Vector2.One); //
            _tVertices[22] = new(_tVertices[8].Position, Color.White, Vector2.UnitX); //
            _tVertices[23] = new(_tVertices[3].Position, Color.White, Vector2.One); //

            _tVertices[24] = new(_tVertices[8].Position, Color.White, Vector2.UnitX); //
            _tVertices[25] = new(_tVertices[21].Position, Color.White, Vector2.One); //
            _tVertices[26] = new(_tVertices[6].Position, Color.White, Vector2.UnitX); //

            _tVertices[27] = new(_tVertices[21].Position, Color.White, Vector2.One); //
            _tVertices[28] = new(_tVertices[3].Position, Color.White, Vector2.One); //
            _tVertices[29] = new(_tVertices[5].Position, Color.White, Vector2.UnitX); //

            _tVertices[30] = new(_tVertices[21].Position, Color.White, Vector2.One); //
            _tVertices[31] = new(_tVertices[6].Position, Color.White, Vector2.Zero); //
            _tVertices[32] = new(_tVertices[2].Position, Color.White, Vector2.UnitY); //

            _tVertices[33] = new(_tVertices[21].Position, Color.White, Vector2.One); //
            _tVertices[34] = new(_tVertices[2].Position, Color.White, Vector2.UnitY); //
            _tVertices[35] = new(_tVertices[6].Position, Color.White, Vector2.Zero); //

            VertexBuffer = new(gd, typeof(VertexPositionColorTexture), _tVertices.Length, BufferUsage.None);
            VertexBuffer.SetData(_tVertices);

            BasiceCubeEff = new(gd) { VertexColorEnabled = true, TextureEnabled = true };

            IndexBuffer = new IndexBuffer(gd, typeof(ushort), _triangleIndices.Length, BufferUsage.WriteOnly);// init buffer of indexes
            IndexBuffer.SetData(_triangleIndices);
        }
    }
}