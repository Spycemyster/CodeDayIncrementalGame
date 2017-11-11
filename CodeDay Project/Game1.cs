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

        private Ability[] abilities; //electricity, fire, ice, earth;
        private Player player;

        /// <summary>
        /// A blank static texture. 1x1 pixel
        /// </summary>
        public static Texture2D Blank;

        /// <summary>
        /// The default font for all text.
        /// </summary>
        public static SpriteFont Font;

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
            Font = Content.Load<SpriteFont>("Font");

            player = new Player();
            player.Texture = Content.Load<Texture2D>("resources/wizardAndStaff/wizard0");
            player.AbilityPower = 10;
            player.CurrentHealth = 100;
            player.MaxHealth = 100;
            player.CurrentMana = 50;
            player.MaxMana = 50;
            
            abilities = new Ability[4];
            abilities[0] = new Ability(player, 10, 0.5f);
            abilities[0].Cooldown = 5f;
            abilities[0].Texture = Content.Load<Texture2D>("resources/abilities/ability_0");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            
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

            for (int i = 0; i < abilities.Length; i++)
                abilities[i]?.Update(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointClamp, null, null, null, null);


            // players and entities
            player.Draw(spriteBatch);

            // UI
            spriteBatch.Draw(Blank, new Rectangle(0, WINDOW_HEIGHT - WINDOW_HEIGHT / 4 - 60, WINDOW_WIDTH, 60), Color.Gray);
            spriteBatch.Draw(Blank, new Rectangle(0, WINDOW_HEIGHT - WINDOW_HEIGHT / 4, WINDOW_WIDTH - WINDOW_WIDTH * 2 / 7, WINDOW_HEIGHT / 4), Color.LightGray);
            spriteBatch.Draw(Blank, new Rectangle(WINDOW_WIDTH - WINDOW_WIDTH * 2 / 7, 0, WINDOW_WIDTH * 2 / 7, WINDOW_HEIGHT), Color.White);
            int width = WINDOW_WIDTH - WINDOW_WIDTH * 2 / 7;
            int border = 16;
            int iconWidth = (width - border * 6) / 5 - 16;

            for (int i = 0; i < abilities.Length; i++)
            {
                if (abilities[i] != null)
                {
                    Color c = Color.White;
                    float ability = 0f;
                    if (abilities[i].Timer < abilities[i].Cooldown * 1000f)
                    {
                        c = Color.Gray;
                        ability = abilities[i].Timer / 1000f;
                    }
                    int y = WINDOW_HEIGHT - WINDOW_HEIGHT / 4 + border;
                    int x = (iconWidth + border) * i;
                    spriteBatch.Draw(abilities[i].Texture, new Rectangle(x, y, iconWidth, iconWidth), c);
                    if (abilities[i].Timer < abilities[i].Cooldown * 1000f)
                    {
                        string text = "" + (int)(abilities[i].Cooldown - ability + 1);
                        spriteBatch.DrawString(Font, text, 
                            new Vector2(x + iconWidth / 2 - Font.MeasureString(text).X / 2,
                            y + iconWidth / 2 - Font.MeasureString(text).Y / 2), Color.LightGray);
                    }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
