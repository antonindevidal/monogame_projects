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

        Vector3 camPosition;

        float pitch = 0f;
        float yaw = 0f;
        Vector3 direction = Vector3.UnitX;

        float speed = 0.5f;
        float rotationSpeed = 0.3f;
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
            camPosition = new Vector3(0, 200, 0);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90f), graphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);

            viewMatrix = Matrix.CreateLookAt(camPosition, camPosition+ direction, Vector3.Up);
            worldMatrix = Matrix.CreateWorld(camPosition + direction, Vector3.Forward, Vector3.Up);
            Mouse.SetPosition(graphicsDevice.Viewport.Width/2, graphicsDevice.Viewport.Height/2);
            originalMousePos = Mouse.GetState();
            screenMidleHeight = graphicsDevice.Viewport.Height / 2;
            screenMidleWidth = graphicsDevice.Viewport.Width / 2;
        }

        public void Update(GameTime gameTime)
        {

            float movAngle=0;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                camPosition.Z -= MathF.Cos(MathHelper.ToRadians(yaw+90)) * speed * gameTime.ElapsedGameTime.Milliseconds;
                camPosition.X -= MathF.Sin(MathHelper.ToRadians(yaw + 90)) * speed * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                camPosition.Z += MathF.Cos(MathHelper.ToRadians(yaw + 90)) * speed * gameTime.ElapsedGameTime.Milliseconds;
                camPosition.X += MathF.Sin(MathHelper.ToRadians(yaw + 90)) * speed * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                camPosition.Z += MathF.Cos(MathHelper.ToRadians(yaw)) * speed * gameTime.ElapsedGameTime.Milliseconds;
                camPosition.X += MathF.Sin(MathHelper.ToRadians(yaw)) * speed * gameTime.ElapsedGameTime.Milliseconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                camPosition.Z -= MathF.Cos(MathHelper.ToRadians(yaw)) * speed * gameTime.ElapsedGameTime.Milliseconds;
                camPosition.X -= MathF.Sin(MathHelper.ToRadians(yaw)) * speed * gameTime.ElapsedGameTime.Milliseconds;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                camPosition.Y += 2;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                camPosition.Y -= 2;
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



            //mouse position for rotation
            MouseState newMouse = Mouse.GetState();
            if(newMouse != originalMousePos && !Keyboard.GetState().IsKeyDown(Keys.E))
            {
                float xDiff = originalMousePos.X - newMouse.X;
                float yDiff = originalMousePos.Y - newMouse.Y;
                

                yaw+=xDiff* rotationSpeed;
                pitch+=yDiff* rotationSpeed;
                if (pitch > 89.0f)
                    pitch = 89.0f;
                if (pitch < -89.0f)
                    pitch = -89.0f;

                direction.X = MathF.Sin(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));
                direction.Z = MathF.Cos(MathHelper.ToRadians(yaw)) * MathF.Cos(MathHelper.ToRadians(pitch));
                direction.Y = MathF.Sin(MathHelper.ToRadians(pitch));
                direction.Normalize();

                System.Diagnostics.Debug.WriteLine(pitch);
                Mouse.SetPosition(screenMidleWidth, screenMidleHeight);
            }

            viewMatrix = Matrix.CreateLookAt(camPosition, direction + camPosition, Vector3.Up);
        }
    }
}
