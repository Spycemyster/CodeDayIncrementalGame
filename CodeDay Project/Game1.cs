using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CodeDay_Project
{
    /// <summary>
    /// Author: Spencer Chang, Ryan Niu
    /// Date: November 11, 2017
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// A blank static texture. 1x1 pixel
        /// </summary>
        public static Texture2D Blank;

        /// <summary>
        /// Width dimension for the window.
        /// </summary>
        public const int WINDOW_WIDTH = 1280;

        /// <summary>
        /// Height dimension for the window.
        /// </summary>
        public const int WINDOW_HEIGHT = 720;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsFixedTimeStep = false;

            //WINDOW_HEIGHT = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height * 2 / 3;
            //WINDOW_WIDTH = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width * 2 / 3;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            IsMouseVisible = true;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Blank = Content.Load<Texture2D>("Blank");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            spriteBatch.Begin();
            spriteBatch.Draw(Blank, new Rectangle(0, WINDOW_HEIGHT - WINDOW_HEIGHT / 4 - 60, WINDOW_WIDTH, 60), Color.Gray);
            spriteBatch.Draw(Blank, new Rectangle(0, WINDOW_HEIGHT - WINDOW_HEIGHT / 4, WINDOW_WIDTH, WINDOW_HEIGHT / 4), Color.LightGray);
            spriteBatch.Draw(Blank, new Rectangle(WINDOW_WIDTH - WINDOW_WIDTH * 2 / 7, 0, WINDOW_WIDTH * 2 / 7, WINDOW_HEIGHT), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
