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
    class BasicMap
    {

        BasicEffect basicEffect;
        Effect basicPositionEffect;
        GrassManager grassManager;
        //Geometric info
        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;
        short[] indices;
        float[,] noise;


        private const int MAPSIZE = 51;
        private const int TRIANGLESIZE = 10;

        GrassElement grassElement;
        Cube cube;


        public void Initialize(GraphicsDevice graphicsDevice)
        {
            //Basic effect
            //BasicEffect
            basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.Alpha = 1f;

            // Want to see the colors of the vertices, this needs to be on
            basicEffect.VertexColorEnabled = true;

            //Lighting requires normal information which VertexPositionColor does not have
            //If you want to use lighting and VPC you need to create a custom def
            basicEffect.LightingEnabled = false;
;

            noise = SimplexNoise.Noise.Calc2D(MAPSIZE* TRIANGLESIZE, MAPSIZE* TRIANGLESIZE, 0.0005f);


            //Geometry  - a simple triangle about the origin
            triangleVertices = new VertexPositionColor[MAPSIZE * MAPSIZE];

            for(int i =0;i < MAPSIZE; i++)
            {
                for (int j = 0; j < MAPSIZE; j++)
                {
                    triangleVertices[i + MAPSIZE * j] = new VertexPositionColor(new Vector3(i * TRIANGLESIZE -MAPSIZE*TRIANGLESIZE/2, noise[i* TRIANGLESIZE,j*TRIANGLESIZE],j*TRIANGLESIZE - MAPSIZE * TRIANGLESIZE / 2),new Color(0,26,4));
                }
            }


            //Vert buffer
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(
                           VertexPositionColor), triangleVertices.Length, BufferUsage.
                           WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);

            indices = new short[(MAPSIZE-1)*(MAPSIZE-1) *6];
            int cpt = 0;
            for (int i = 0; i < MAPSIZE-1; i++)
            {
                for (int j = 0; j < MAPSIZE-1; j++)
                {
                    //upper left triangle
                    indices[(cpt * 6)] = (short)(i + MAPSIZE * j);
                    indices[(cpt * 6) + 1] = (short)(i + MAPSIZE * j +1);
                    indices[(cpt * 6) + 2] = (short)(i + MAPSIZE * j + MAPSIZE);

                    //Lower right traingle
                    indices[(cpt * 6) + 3] = (short)(i + MAPSIZE * j + 1 );
                    indices[(cpt * 6) + 4] = (short)(i + MAPSIZE * j + 1 + MAPSIZE);
                    indices[(cpt * 6) + 5] = (short)(i + MAPSIZE * j + MAPSIZE);
                    cpt++;
                }
            }

            grassManager = new GrassManager();
            grassManager.Initialize(graphicsDevice,MAPSIZE,TRIANGLESIZE,noise);

            grassElement = new GrassElement();
            grassElement.Initialize(graphicsDevice);

            cube = new Cube();
            cube.Initialize(graphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            basicPositionEffect = content.Load<Effect>("effects/position");
            grassManager.LoadContent(content);

            basicPositionEffect.Parameters["pos"].SetValue(new Vector3(0,noise[(MAPSIZE * TRIANGLESIZE) / 2, (MAPSIZE * TRIANGLESIZE) / 2] -1,0));
        }


        public void Draw(GameTime gameTime,GraphicsDevice graphicsDevice,Matrix projectionMatrix, Matrix viewMatrix, Matrix worldMatrix)
        {
            basicPositionEffect.Parameters["WorldViewProjection"].SetValue(viewMatrix * projectionMatrix);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;

            graphicsDevice.Clear(Color.CornflowerBlue);
            //graphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling so we see both sides of our rendered  triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.
                    Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, triangleVertices, 0, triangleVertices.Length, indices, 0, (MAPSIZE - 1) * (MAPSIZE - 1) * 2);

            }
            
            foreach (EffectPass pass in basicPositionEffect.CurrentTechnique.
                    Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, cube.vertices, 0,cube.vertices.Length,cube.indices,0,12);

            }

            grassManager.Draw(gameTime,graphicsDevice,viewMatrix,projectionMatrix);

            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, grassElement.vertices, 0, 1);


        }

    }


}
