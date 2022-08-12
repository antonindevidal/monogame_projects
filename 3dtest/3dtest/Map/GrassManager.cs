using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3dtest.Map
{
    class GrassManager
    {
        /// <summary>
        /// Number of grass blades
        /// </summary>
        private int nbGrass ;

        /// <summary>
        /// Number of blades in square of the map
        /// </summary>
        private const int DENSITY = 3000;

        /// <summary>
        /// base element
        /// </summary>
        private GrassElement grassElement;

        /// <summary>
        /// Effect for grass blades
        /// </summary>
        private Effect effect;

        /// <summary>
        /// Buffer for all the instances of grass
        /// </summary>
        private VertexBuffer instancesBuffer;

        /// <summary>


        //Struct to pass to the gpu for hardware insatncing
        private struct GrassInfo
        {
            public Vector4 World;
            public Vector3 Rotation;
        };

        /// <summary>
        /// Binding for shader
        /// </summary>
        private VertexBufferBinding[] bindings;

        /// <summary>
        /// declaration of parameters for the shader
        /// </summary>
        private VertexDeclaration instanceVertexDeclaration;

        public void Initialize(GraphicsDevice graphicsDevice,int mapSize, int squareSize, float[,] noise)
        {
            InitializeInstanceVertexBuffer();

            grassElement = new GrassElement();
            grassElement.Initialize(graphicsDevice);

            

            Random rnd = new Random();
            nbGrass = DENSITY * (mapSize - 1) * (mapSize - 1);
            GrassInfo[] pos = new GrassInfo[nbGrass];

            //generate all the instances of the blade
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

            
            //create instance buffer
            instancesBuffer = new VertexBuffer(graphicsDevice, instanceVertexDeclaration, nbGrass, BufferUsage.WriteOnly);
            instancesBuffer.SetData(pos);

            //Create binding for shader
            bindings = new VertexBufferBinding[2];
            bindings[0] = new VertexBufferBinding(grassElement.vertexBuffer);
            bindings[1] = new VertexBufferBinding(instancesBuffer, 0, 1);
        }

        /// <summary>
        /// Create vertex declaration buffer for gpu
        /// </summary>
        private void InitializeInstanceVertexBuffer()
        {
            //We pass two elements to the gpu, the position of the instance and it rotation

            VertexElement[] _instanceStreamElements = new VertexElement[2];

            //Position
            _instanceStreamElements[0] = new VertexElement(0, VertexElementFormat.Vector4,
                        VertexElementUsage.Position, 1);
            //Rotation
            _instanceStreamElements[1] = new VertexElement(sizeof(float)*4, VertexElementFormat.Vector3,
                        VertexElementUsage.Position, 2);

            instanceVertexDeclaration = new VertexDeclaration(_instanceStreamElements);
        }



        public void LoadContent(ContentManager content)
        {
            //Load shader
            effect = content.Load<Effect>("effects/instance_effect");
        }



        public void Draw(GameTime gameTime, GraphicsDevice graphicsDevice,Matrix view,Matrix projection)
        {
            // Set the effect technique and parameters
            effect.CurrentTechnique = effect.Techniques["Instancing"];
            effect.Parameters["WorldViewProjection"].SetValue(view * projection);
            effect.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);

            graphicsDevice.Indices = grassElement.indexBuffer;

            effect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.SetVertexBuffers(bindings);

            //Draw all the instances
            graphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList ,0,0,grassElement.indices.Length /3 ,nbGrass);

        }
    }
}
