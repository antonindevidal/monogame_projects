using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dtest.Map
{
    class Cube
    {
        public readonly VertexPositionColor[] vertices = new VertexPositionColor[]
       {
            new VertexPositionColor(new Vector3(-10,0,-10),Color.Wheat),
            new VertexPositionColor(new Vector3(-10,0,10),Color.Wheat),
            new VertexPositionColor(new Vector3(10,0,-10),Color.Wheat),
            new VertexPositionColor(new Vector3(10,0,10),Color.Wheat),


            new VertexPositionColor(new Vector3(-10,20,-10),Color.Wheat),
            new VertexPositionColor(new Vector3(-10,20,10),Color.Wheat),
            new VertexPositionColor(new Vector3(10,20,-10),Color.Wheat),
            new VertexPositionColor(new Vector3(10,20,10),Color.Wheat),
            
       };

        public VertexBuffer vertexBuffer;

        public readonly short[] indices = new short[]
        {
            0,1,2,
            1,3,2,
            7,5,1,
            7,1,3,
            6,7,3,
            6,3,2,
            4,6,2,
            4,2,0,
            5,4,0,
            5,0,1,
            4,5,7,
            4,7,6,
            
        };


        public void Initialize(GraphicsDevice graphicsDevice)
        {
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(
                VertexPositionColor), vertices.Length, BufferUsage.
                WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);
        }
    }
}
