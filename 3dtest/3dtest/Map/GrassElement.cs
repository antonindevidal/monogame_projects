using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3dtest.Map
{
    /// <summary>
    /// One grass blade element with index and vertex buffer
    /// </summary>
    class GrassElement
    {

        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        //Position of the vertices
        public readonly VertexPositionColor[] vertices = new VertexPositionColor[]
        {
            new VertexPositionColor(new Vector3(-0.5f,0,0),new Color(0,51,0,255)),
            new VertexPositionColor(new Vector3(0.5f,0,0),new Color(0,51,0,255)),

            new VertexPositionColor(new Vector3(-0.4f,2,0),new Color(0,77,0,255)),
            new VertexPositionColor(new Vector3(0.4f,2,0),new Color(0,77,0,255)),

            new VertexPositionColor(new Vector3(0,5,0),new Color(0,102,0,255)),
        };

        //Indices of the blade
        public readonly short[] indices = new short[]
        {
            0,1,2,
            1,2,3,
            2,3,4
        };


        public void Initialize(GraphicsDevice graphicsDevice)
        {
            //Create vertex buffer
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(
                VertexPositionColor), vertices.Length, BufferUsage.
                WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            //Create index buffer
            indexBuffer = new IndexBuffer(graphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }
        
    }
}
