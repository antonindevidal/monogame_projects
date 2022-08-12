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


            base.Draw(gameTime);
        }
    }
}