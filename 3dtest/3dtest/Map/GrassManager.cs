using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dtest.Map
{
    class GrassManager
    {

        private int NBGRASS ;
        private const int DENSITY = 3000;

        GrassElement grassElement;
        Effect effect;

        VertexBuffer instancesBuffer;
        IndexBuffer indexBuffer;
        struct GrassInfo
        {
            public Vector4 World;
            public Vector3 Rotation;
        };

        VertexBufferBinding[] bindings;
        private VertexDeclaration instanceVertexDeclaration;

        public void Initialize(GraphicsDevice graphicsDevice,int mapSize, int squareSize, float[,] noise)
        {
            InitializeInstanceVertexBuffer();

            grassElement = new GrassElement();
            grassElement.Initialize(graphicsDevice);

            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), grassElement.indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(grassElement.indices);

            Random rnd = new Random();
            GrassInfo[] pos = new GrassInfo[DENSITY*(mapSize - 1) * (mapSize - 1)];

            for(int i =0;i < (mapSize-1); i++)
            {
                for (int j = 0; j < (mapSize - 1); j++)
                {
                    for (int k = 0; k < DENSITY; k++)
                    {
                        float x = (float)rnd.NextDouble() * squareSize + squareSize * (i % (mapSize - 1));
                        float z = (float)rnd.NextDouble() * squareSize + squareSize * (j % (mapSize - 1));
                        pos[i * (mapSize - 1) * (mapSize - 1) + j * (mapSize - 1) + k].World = new Vector4(x - mapSize * squareSize / 2, noise[(int)MathF.Floor(x), (int)MathF.Floor(z)], z - mapSize * squareSize / 2,0); ;
                        pos[i * (mapSize - 1) * (mapSize - 1) + j * (mapSize - 1) + k].Rotation = new Vector3(0,MathHelper.ToRadians(rnd.Next(360)),0);
                    }
                }
            }

            NBGRASS = DENSITY*(mapSize - 1) * (mapSize - 1);

            instancesBuffer = new VertexBuffer(graphicsDevice, instanceVertexDeclaration, NBGRASS, BufferUsage.WriteOnly);
            instancesBuffer.SetData(pos);


            bindings = new VertexBufferBinding[2];
            bindings[0] = new VertexBufferBinding(grassElement.vertexBuffer);
            bindings[1] = new VertexBufferBinding(instancesBuffer, 0, 1);
        }

        private void InitializeInstanceVertexBuffer()
        {
            VertexElement[] _instanceStreamElements = new VertexElement[2];

            // Position
            _instanceStreamElements[0] = new VertexElement(0, VertexElementFormat.Vector4,
                        VertexElementUsage.Position, 1);
            _instanceStreamElements[1] = new VertexElement(sizeof(float)*4, VertexElementFormat.Vector3,
                        VertexElementUsage.Position, 2);

            instanceVertexDeclaration = new VertexDeclaration(_instanceStreamElements);
        }



        public void LoadContent(ContentManager content)
        {
            effect = content.Load<Effect>("effects/instance_effect");
        }



        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice,Matrix view,Matrix projection)
        {
            // Set the effect technique and parameters
            effect.CurrentTechnique = effect.Techniques["Instancing"];
            effect.Parameters["WorldViewProjection"].SetValue(view * projection);
            effect.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);
            //System.Diagnostics.Debug.WriteLine(""+ (float)gameTime.TotalGameTime.TotalMilliseconds);
            graphicsDevice.Indices = indexBuffer;

            effect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.SetVertexBuffers(bindings);
            graphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList ,0,0,grassElement.indices.Length /3 ,NBGRASS);

        }
    }
}
