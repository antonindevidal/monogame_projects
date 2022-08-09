using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3dtest.Camera
{
    class BasicCamera
    {

        Vector3 camTarget;
        Vector3 camPosition;
        public Matrix projectionMatrix { get;private set; }
        public Matrix worldMatrix { get; private set; }
        public Matrix viewMatrix { get; private set; }

        private int lastScrollWheelValue;
        bool orbit;

        MouseState originalMousePos;

        int screenMidleWidth;
        int screenMidleHeight;

        public void Initialize(GraphicsDevice graphicsDevice)
        {
            //Setup camera
            camTarget = Vector3.Zero;
            camPosition = new Vector3(200f, 500, 0);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90f), graphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);
            Mouse.SetPosition(graphicsDevice.Viewport.Width/2, graphicsDevice.Viewport.Height/2);
            originalMousePos = Mouse.GetState();
            screenMidleHeight = graphicsDevice.Viewport.Height / 2;
            screenMidleWidth = graphicsDevice.Viewport.Width / 2;
        }

        public void Update(GameTime gameTime)
        {


            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camPosition.X -= 1;
                camTarget.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                camPosition.X += 1;
                camTarget.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                camPosition.Z += 1;
                camTarget.Z += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                camPosition.Z -= 1;
                camTarget.Z -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                camPosition.Y += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                camPosition.Y -= 1;
            }


            if (Mouse.GetState().ScrollWheelValue != lastScrollWheelValue)
            {
                int zoom = lastScrollWheelValue > Mouse.GetState().ScrollWheelValue ? -1 : 1;
                camPosition.Z += zoom;

            }
            lastScrollWheelValue = Mouse.GetState().ScrollWheelValue;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                orbit = !orbit;
            }
            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                camPosition = Vector3.Transform(camPosition, rotationMatrix);
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);


            //mouse position for rotation
            MouseState newMouse = Mouse.GetState();
            if(newMouse != originalMousePos && Keyboard.GetState().IsKeyDown(Keys.E))
            {
                //float xDiff = originalMousePos.X - newMouse.X;
                float yDiff = originalMousePos.Y - newMouse.Y;
                //camTarget.X += xDiff;
                camTarget.Y += yDiff;
                Mouse.SetPosition(screenMidleWidth, screenMidleHeight);
            }
            

        }
    }
}
