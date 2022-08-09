using _3dtest.Camera;
using _3dtest.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace _3dtest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;




        Model model;
        BasicMap map;
        BasicCamera camera;


        SpriteFont font;
        SpriteBatch _spriteBatch;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();
            map = new BasicMap();
            map.Initialize(GraphicsDevice);
            camera = new BasicCamera();
            camera.Initialize(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            model = Content.Load<Model>("MonoCube/MonoCube");
            font = Content.Load<SpriteFont>("font/roboto");
            map.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);
            map.Draw(gameTime,GraphicsDevice, camera.projectionMatrix, camera.viewMatrix, camera.worldMatrix);

            //foreach (ModelMesh mesh in  model.Meshes)
            //{
            //    foreach(BasicEffect effect in mesh.Effects)
            //    {
            //        effect.View = viewMatrix;
            //        effect.World = worldMatrix;
            //        effect.Projection = projectionMatrix;
            //    }
            //    mesh.Draw();
            //}
/*
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "" + MathF.Sin((int)((int)gameTime.TotalGameTime.TotalMilliseconds * 0.002)), new Vector2(0, 0), Color.Black);
            _spriteBatch.End();*/



            base.Draw(gameTime);
        }
    }
}