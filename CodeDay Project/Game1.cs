using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CodeDay_Project {
    /// <summary>
    /// Author: Spencer Chang, Ryan Niu
    /// Date: November 11, 2017
    /// This is the main type for your game.
    /// </summary>
    [Serializable]
    public class Game1 : Game
    {
        [NonSerialized]
        Random rand;
        [NonSerialized]
        GraphicsDeviceManager graphics;
        [NonSerialized]
        SpriteBatch spriteBatch;

        [Serializable]
        private enum ProjectileType
        {
            ELECTRICTY,
            FIRE,
            ICE,
            GROUND,
        }
        private ProjectileType type;
        private Ability[] abilities; //electricity, fire, ice, earth;
        private Player player;
        [NonSerialized]
        private Texture2D abilityBorder;
        [NonSerialized]
        private Texture2D shopBorder, coinTexture;
        private bool isDimmingUp;
        private float dimTimer;
        private float dimOpacity;
        private const float DIM_INCREMENT = 0.05f;
        [NonSerialized]
        private Rectangle[] guiRectangles;
        [NonSerialized]
        private Texture2D[] shopIcons;
        private float[] shopCosts;
        [NonSerialized]
        private Color[] shopColors;
        [NonSerialized]
        private Rectangle[] shopRectangles;
        [NonSerialized]
        private Rectangle healthBar, manaBar, cHealthBar, cManaBar, coinRectangle, eHealth, ecHealth;

        /// <summary>
        /// A blank static texture. 1x1 pixel
        /// </summary>
        public static Texture2D Blank;

        /// <summary>
        /// The default font for all text.
        /// </summary>
        public static SpriteFont Font;

        /// <summary>
        /// The default small font for all text.
        /// </summary>
        public static SpriteFont SmallFont;

        /// <summary>
        /// Width dimension for the window.
        /// </summary>
        public const int WINDOW_WIDTH = 1280;

        /// <summary>
        /// Height dimension for the window.
        /// </summary>
        public const int WINDOW_HEIGHT = 720;

        private List<Projectile> projectiles;
        private const int MAX_LENGTH = 400;
        private const int MAX_LENGTH2 = 400;
        [NonSerialized]
        private Texture2D electricityProjectile, fireProjectile, iceProjectile, groundProjectile;
        [NonSerialized]
        private Texture2D[] backgrounds;
        private Enemy currentEnemy;
        private int floor = 1;
        private int currentBackground;
        private bool hasChangedStances, selfBuffActive;
        private float money, selfBuffTimer, autoSaveTimer;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            rand = new Random();
            IsFixedTimeStep = false;

            //WINDOW_HEIGHT = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height * 2 / 3;
            //WINDOW_WIDTH = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width * 2 / 3;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            IsMouseVisible = true;
            Window.Title = "Memecremental";
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            selfBuffActive = false;
            selfBuffTimer = 0f;
            isDimmingUp = true;
            hasChangedStances = false;
            Blank = Content.Load<Texture2D>("Blank");
            Font = Content.Load<SpriteFont>("Font");
            SmallFont = Content.Load<SpriteFont>("RegularFont");
            backgrounds = new Texture2D[3];
            backgrounds[0] = Content.Load<Texture2D>("resources/backgrounds/inside");
            backgrounds[1] = Content.Load<Texture2D>("resources/backgrounds/outside");
            backgrounds[2] = Content.Load<Texture2D>("resources/backgrounds/space");
            coinTexture = Content.Load<Texture2D>("resources/GUI/coin");
            autoSaveTimer = 0f;
            money = 100000f;

            projectiles = new List<Projectile>();

            type = ProjectileType.ELECTRICTY;

            guiRectangles = new Rectangle[3];
            guiRectangles[0] = new Rectangle(0, WINDOW_HEIGHT - WINDOW_HEIGHT / 4 - 60, WINDOW_WIDTH, 60);
            guiRectangles[1] = new Rectangle(0, WINDOW_HEIGHT - WINDOW_HEIGHT / 4, WINDOW_WIDTH - WINDOW_WIDTH * 2 / 7, WINDOW_HEIGHT / 4);
            guiRectangles[2] = new Rectangle(WINDOW_WIDTH - WINDOW_WIDTH * 2 / 7, 0, WINDOW_WIDTH * 2 / 7, WINDOW_HEIGHT);

            electricityProjectile = Content.Load<Texture2D>("resources/particles/particle_0");
            fireProjectile = Content.Load<Texture2D>("resources/particles/particle_1");
            iceProjectile = Content.Load<Texture2D>("resources/particles/particle_2");
            groundProjectile = Content.Load<Texture2D>("resources/particles/particle_3");

            player = new Player(500f);
            player.Name = "Player";
            player.Texture = Content.Load<Texture2D>("resources/wizardAndStaff/wizard0");
            player.DrawRectangle = new Rectangle(42, guiRectangles[0].Y - 306, 129, 306);
            player.AbilityPower = 10;
            player.CurrentHealth = 100;
            player.MaxHealth = 100;
            player.CurrentMana = 50;
            player.MaxMana = 50;
            player.ManaRegen = 5;
            player.HealthRegen = 10;
            abilityBorder = Content.Load<Texture2D>("resources/abilities/abilityBorder");
            shopBorder = Content.Load<Texture2D>("resources/backgrounds/shopBackground");

            abilities = new Ability[5];
            abilities[0] = new Ability(player, 10, 0.5f);
            abilities[0].Cost = 4;
            abilities[0].Cooldown = 5f;
            abilities[0].Timer = 5000f;
            abilities[0].Texture = Content.Load<Texture2D>("resources/abilities/ability_0");
            abilities[1] = new Ability(player, 15, 0.6f);
            abilities[1].Cost = 12;
            abilities[1].Cooldown = 7f;
            abilities[1].Timer = 7000f;
            abilities[1].Texture = Content.Load<Texture2D>("resources/abilities/ability_1");
            abilities[2] = new Ability(player, 15, 0.75f);
            abilities[2].Cost = 10;
            abilities[2].Cooldown = 8f;
            abilities[2].Timer = 8000f;
            abilities[2].Texture = Content.Load<Texture2D>("resources/abilities/ability_2");
            abilities[3] = new Ability(player, 15, 0.8f);
            abilities[3].Cost = 8;
            abilities[3].Cooldown = 9f;
            abilities[3].Timer = 9000f;
            abilities[3].Texture = Content.Load<Texture2D>("resources/abilities/ability_3");
            abilities[4] = new Ability(player, 15, 0.95f);
            abilities[4].Cost = 5;
            abilities[4].Cooldown = 15f;
            abilities[4].Timer = 15000f;
            abilities[4].Texture = Content.Load<Texture2D>("resources/abilities/ability_4");

            shopIcons = new Texture2D[4];
            shopIcons[0] = Content.Load<Texture2D>("resources/GUI/shopIcon_0");
            shopIcons[1] = Content.Load<Texture2D>("resources/GUI/shopIcon_1");
            shopIcons[2] = Content.Load<Texture2D>("resources/GUI/shopIcon_2");
            shopIcons[3] = Content.Load<Texture2D>("resources/GUI/shopIcon_3");
            shopCosts = new float[4];
            shopCosts[0] = 500f;
            shopCosts[1] = 500f;
            shopCosts[2] = 500f;
            shopCosts[3] = 500f;
            shopColors = new Color[4];
            shopColors[0] = Color.White;
            shopColors[1] = Color.White;
            shopColors[2] = Color.White;
            shopColors[3] = Color.White;
            shopRectangles = new Rectangle[4];
            int sizeIcons = 86;
            for (int i = 0; i < 4; i++)
            {
                int offset = 112 - sizeIcons;
                shopRectangles[i] = new Rectangle(guiRectangles[2].X + offset, (148 + i * 112) + offset / 2, sizeIcons, sizeIcons);
                //shopRectangles[0] = new Rectangle(guiRectangles[2].X + 16, 152, sizeIcons, sizeIcons);
                //shopRectangles[1] = new Rectangle(guiRectangles[2].X + 16, 268, sizeIcons, sizeIcons);
                //shopRectangles[2] = new Rectangle(guiRectangles[2].X + 16, 364, sizeIcons, sizeIcons);
                //shopRectangles[3] = new Rectangle(guiRectangles[2].X + 16, 500, sizeIcons, sizeIcons);
            }

            int width = WINDOW_WIDTH - WINDOW_WIDTH * 2 / 7;
            int border = 32;
            int iconWidth = (width - border * 6) / 5 - border;
            int border2 = 12;

            healthBar = new Rectangle(guiRectangles[0].X + border2, guiRectangles[0].Y + border2, MAX_LENGTH, 14);
            manaBar = new Rectangle(guiRectangles[0].X + border2, guiRectangles[0].Y + guiRectangles[0].Height - 14 - border2, MAX_LENGTH, 14);
            cHealthBar = new Rectangle(guiRectangles[0].X + border2, guiRectangles[0].Y + border2, MAX_LENGTH, 14);
            cManaBar = new Rectangle(guiRectangles[0].X + border2, guiRectangles[0].Y + guiRectangles[0].Height - 14 - border2, MAX_LENGTH, 14);
            eHealth = new Rectangle(guiRectangles[2].X - MAX_LENGTH2 - 16, healthBar.Y, MAX_LENGTH2, 14);
            ecHealth = new Rectangle(guiRectangles[2].X - MAX_LENGTH2 - 16, healthBar.Y, MAX_LENGTH2, 14);
            coinRectangle = new Rectangle(guiRectangles[2].X + 26, guiRectangles[2].Y + 48, 64, 64);

            for (int i = 0; i < abilities.Length; i++) {
                int y = WINDOW_HEIGHT - WINDOW_HEIGHT / 4 + border;
                int x = (iconWidth + border) * i + border;
                abilities[i].DrawRectangle = new Rectangle(x, y, iconWidth, iconWidth);
            }
            generateRandomEnemy(false);
            currentBackground = rand.Next(0, 2);
            player.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.HasActiveBuff = selfBuffActive;
            int border2 = 12;
            float percentHealth = player.CurrentHealth / player.MaxHealth;
            float percentMana = player.CurrentMana / player.MaxMana;
            float percentHealth2 = currentEnemy.CurrentHealth / currentEnemy.MaxHealth;
            ecHealth = new Rectangle(guiRectangles[2].X - MAX_LENGTH2 - 16, healthBar.Y, (int)(MAX_LENGTH2 * percentHealth2), 14);
            cHealthBar = new Rectangle(guiRectangles[0].X + border2, guiRectangles[0].Y + border2, (int)(MAX_LENGTH * percentHealth), 14);
            cManaBar = new Rectangle(guiRectangles[0].X + border2, guiRectangles[0].Y + guiRectangles[0].Height - 14 - border2,
                (int)(MAX_LENGTH * percentMana), 14);

            for (int i = 0; i < shopIcons.Length; i++)
            {
                shopColors[i] = Color.White;
                if (shopRectangles[i].Contains(Mouse.GetState().Position))
                {
                    shopColors[i] = Color.DarkGray;
                    if (InputManager.Instance.leftMouseButtonClicked() && money >= shopCosts[i])
                    {
                        money -= shopCosts[i];
                        shopCosts[i] *= 1.3f;

                        switch (i)
                        {
                            case 0:
                                player.CurrentHealth += 20f;
                                player.MaxHealth += 20f;
                                break;

                            case 1:
                                player.Defense += 1.5f;
                                break;

                            case 2:
                                player.AbilityPower += 7f;
                                break;

                            case 3:
                                player.Speed -= 20f;
                                break;

                        }

                    }
                }
            }

            if (selfBuffActive)
            {
                selfBuffTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (selfBuffTimer >= 8000f)
                {
                    selfBuffTimer = 0f;
                    selfBuffActive = false;
                }
            }

            if (!player.isAlive)
            {
                dimTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (isDimmingUp)
                {
                    if (dimTimer >= 50f)
                    {
                        dimTimer = 0f;
                        dimOpacity += DIM_INCREMENT;
                    }
                    if (dimOpacity >= 1.0f)
                    {
                        floor = (floor - 1) / 10 * 10 + 1;
                        generateRandomEnemy(floor % 10 == 0);
                        player.CurrentHealth = player.MaxHealth;
                        player.CurrentMana = player.MaxMana;
                        projectiles.Clear();
                        isDimmingUp = false;
                    }
                }
                else
                {
                    if (dimTimer >= 50f)
                    {
                        dimOpacity -= DIM_INCREMENT;
                        dimTimer = 0f;
                    }
                    if (dimOpacity <= 0f)
                    {
                        player.isAlive = true;
                        isDimmingUp = true;
                    }
                }
            }
            else
            {
                player.Update(gameTime);
                if (player.hasAttacked)
                {
                    Projectile p = new Projectile();
                    float mult = (hasChangedStances ? 1.5f : 1);
                    int mult2 = (hasChangedStances ? 2 : 1);
                    switch (type)
                    {
                        case ProjectileType.ELECTRICTY:
                            p.Dimensions = new Point(120 * mult2, 96 * mult2);
                            p.Position = new Vector2(223 - (mult2 - 1) * 120, 250 - (mult2 - 1) * 60);
                            p.Texture = electricityProjectile;
                            p.Velocity = new Vector2(10, 0);
                            p.CollisionalDamage = player.AbilityPower * abilities[0].DamageScaling * mult;
                            break;
                        case ProjectileType.FIRE:
                            p.Dimensions = new Point(128 * mult2, 96 * mult2);
                            p.Position = new Vector2(223 - (mult2 - 1) * 120, 250 - (mult2 - 1) * 60);
                            p.Texture = fireProjectile;
                            p.Velocity = new Vector2(10, 0);
                            p.CollisionalDamage = player.AbilityPower * abilities[1].DamageScaling * mult;
                            break;
                        case ProjectileType.GROUND:
                            p.Dimensions = new Point(128 * mult2, 96 * mult2);
                            p.Position = new Vector2(223 - (mult2 - 1) * 120, 250 - (mult2 - 1) * 60);
                            p.Texture = groundProjectile;
                            p.Velocity = new Vector2(10, 0);
                            p.CollisionalDamage = player.AbilityPower * abilities[2].DamageScaling * mult;
                            break;
                        case ProjectileType.ICE:
                            p.Dimensions = new Point(128 * mult2, 96 * mult2);
                            p.Position = new Vector2(223 - (mult2 - 1) * 120, 250 - (mult2 - 1) * 60);
                            p.Texture = iceProjectile;
                            p.Velocity = new Vector2(10, 0);
                            p.CollisionalDamage = player.AbilityPower * abilities[3].DamageScaling * mult;
                            break;
                    }
                    hasChangedStances = false;
                    projectiles.Add(p);
                    player.hasAttacked = false;
                }
                for (int i = 0; i < abilities.Length; i++)
                {
                    if (abilities[i] != null)
                    {
                        if (abilities[i].DrawRectangle.Contains(Mouse.GetState().Position) &&
                            InputManager.Instance.leftMouseButtonClicked() && player.CurrentMana >= 
                            abilities[i].Cost && abilities[i].Cooldown * 1000 <= abilities[i].Timer)
                        {
                            switch (i)
                            {
                                case 0:
                                    type = ProjectileType.ELECTRICTY;
                                    break;
                                case 1:
                                    type = ProjectileType.FIRE;
                                    break;
                                case 2:
                                    type = ProjectileType.ICE;
                                    break;
                                case 3:
                                    type = ProjectileType.GROUND;
                                    break;
                                case 4:
                                    selfBuffActive = true;
                                    break;
                            }
                            if (i != 4)
                                hasChangedStances = true;
                            player.CurrentMana -= abilities[i].Cost;
                            abilities[i].InflictOn(currentEnemy);
                        }
                        abilities[i].Update(gameTime);
                    }
                }
                currentEnemy.Update(gameTime);

                if (player.isAlive)
                {
                    for (int i = 0; i < projectiles.Count; i++)
                    {
                        Vector2 pos = new Vector2(223, 250);
                        if (projectiles[i].DrawRectangle.Intersects(currentEnemy.DrawRectangle))
                        {
                            currentEnemy.Damage(projectiles[i].CollisionalDamage);
                            projectiles.RemoveAt(i--);
                        }
                    }
                }

                if (currentEnemy != null && !currentEnemy.isAlive)
                {
                    generateRandomEnemy(++floor % 10 == 0);
                    money += 5 + (float)Math.Log10(floor + 1) * 10 * (((floor - 1) % 10) == 0 ? 10 : 1);

                    if (floor % 10 == 1 && floor != 1)
                        currentBackground = rand.Next(0, 3);
                }
            }

            if (player.isAlive)
            {
                foreach (Projectile p in projectiles)
                    p.Update(gameTime);
            }
            InputManager.Instance.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(backgrounds[currentBackground], new Rectangle(0, 0, backgrounds[currentBackground].Width, backgrounds[currentBackground].Height), Color.White);

            // players and entities
            foreach (Projectile p in projectiles)
                p.Draw(spriteBatch);
            player.Draw(spriteBatch);
            currentEnemy.Draw(spriteBatch);
            spriteBatch.Draw(Blank, new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT), Color.Black * dimOpacity);

            // UI
            spriteBatch.DrawString(Font, "Floor " + floor, new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(SmallFont, " - ", new Vector2(Font.MeasureString("Floor " + floor).X,
                Font.MeasureString("Floor " + floor).Y / 2 - SmallFont.MeasureString(" - ").Y / 2), Color.Black);
            spriteBatch.DrawString(SmallFont, currentEnemy.Name, new Vector2(Font.MeasureString("Floor " + floor).X + SmallFont.MeasureString(" - ").X,
                Font.MeasureString("Floor " + floor).Y / 2 - SmallFont.MeasureString(currentEnemy.Name).Y / 2), Color.Black);
            spriteBatch.DrawString(SmallFont, player.Name, new Vector2(player.DrawRectangle.Width / 2 + SmallFont.MeasureString(player.Name).X / 2, player.DrawRectangle.Y - SmallFont.MeasureString(player.Name).Y - 10), Color.Black);
            spriteBatch.Draw(Blank, guiRectangles[0], Color.Gray);
            spriteBatch.Draw(Blank, guiRectangles[1], Color.LightGray);
            spriteBatch.Draw(abilityBorder, guiRectangles[1], Color.White);
            spriteBatch.Draw(shopBorder, guiRectangles[2], Color.White);
            spriteBatch.Draw(Blank, healthBar, Color.ForestGreen * 0.4f);
            spriteBatch.Draw(Blank, manaBar, Color.Blue * 0.4f);
            spriteBatch.Draw(Blank, cManaBar, Color.Blue);
            spriteBatch.Draw(Blank, cHealthBar, Color.ForestGreen);
            spriteBatch.Draw(Blank, eHealth, Color.Red * 0.4f);
            spriteBatch.Draw(Blank, ecHealth, Color.Red);
            spriteBatch.Draw(coinTexture, coinRectangle, Color.White);

            for (int i = 0; i < shopIcons.Length; i++)
            {
                spriteBatch.Draw(shopIcons[i], shopRectangles[i], shopColors[i]);
                if (shopColors[i] != Color.White)
                {
                    string text = shopCosts[i].ToString("C", new CultureInfo("en-US"));
                    spriteBatch.DrawString(SmallFont, text, Mouse.GetState().Position.ToVector2() + new Vector2(1, 1), Color.Black * 0.6f);
                    spriteBatch.DrawString(SmallFont, text, Mouse.GetState().Position.ToVector2(), Color.White);
                }
            }
            string uH = "Upgrade Health";
            string uA = "Upgrade Defense";
            string uD = "Upgrade Damage";
            string uS = "Upgrade Speed";
            spriteBatch.DrawString(SmallFont, uH, new Vector2(
                shopRectangles[0].X + shopRectangles[0].Width + 16 + 1, shopRectangles[0].Y +
                shopRectangles[0].Height / 2 - SmallFont.MeasureString(uH).Y / 2 + 1), Color.Black * 0.5f);
            spriteBatch.DrawString(SmallFont, uH, new Vector2(
                shopRectangles[0].X + shopRectangles[0].Width + 16, shopRectangles[0].Y +
                shopRectangles[0].Height / 2 - SmallFont.MeasureString(uH).Y / 2), Color.White);
            spriteBatch.DrawString(SmallFont, uA, new Vector2(
                shopRectangles[1].X + shopRectangles[0].Width + 17, shopRectangles[1].Y +
                shopRectangles[1].Height / 2 - SmallFont.MeasureString(uA).Y / 2 + 1), Color.Black * 0.5f);
            spriteBatch.DrawString(SmallFont, uA, new Vector2(
                shopRectangles[1].X + shopRectangles[0].Width + 16, shopRectangles[1].Y +
                shopRectangles[1].Height / 2 - SmallFont.MeasureString(uA).Y / 2), Color.White);
            spriteBatch.DrawString(SmallFont, uD, new Vector2(1 +
                shopRectangles[2].X + shopRectangles[0].Width + 16, 1 + shopRectangles[2].Y +
                shopRectangles[2].Height / 2 - SmallFont.MeasureString(uD).Y / 2), Color.Black * 0.5f);
            spriteBatch.DrawString(SmallFont, uD, new Vector2(
                shopRectangles[2].X + shopRectangles[0].Width + 16, shopRectangles[2].Y +
                shopRectangles[2].Height / 2 - SmallFont.MeasureString(uD).Y / 2), Color.White);
            spriteBatch.DrawString(SmallFont, uS, new Vector2(1 +
                shopRectangles[3].X + shopRectangles[0].Width + 16, 1 + shopRectangles[3].Y +
                shopRectangles[3].Height / 2 - SmallFont.MeasureString(uS).Y / 2), Color.Black * 0.5f);
            spriteBatch.DrawString(SmallFont, uS, new Vector2(
                shopRectangles[3].X + shopRectangles[0].Width + 16, shopRectangles[3].Y +
                shopRectangles[3].Height / 2 - SmallFont.MeasureString(uS).Y / 2), Color.White);

            string credits = "Memecremental Game\nSpencer Chang     Ryan Niu";
            spriteBatch.DrawString(SmallFont, credits, new Vector2(64 + WINDOW_WIDTH - guiRectangles[2].Width, 652 - SmallFont.MeasureString(credits).Y / 2), Color.Black);
            string moneyText = money.ToString("C", new CultureInfo("en-US"));

            spriteBatch.DrawString(SmallFont, moneyText, new Vector2(coinRectangle.X + coinRectangle.Width + 16, coinRectangle.Y + coinRectangle.Height / 2 - SmallFont.MeasureString(moneyText).Y / 2), Color.Black);
            string ch = (int)player.CurrentHealth + "/" + (int)player.MaxHealth;
            string cm = (int)player.CurrentMana + "/" + (int)player.MaxMana;
            spriteBatch.DrawString(SmallFont, ch, new Vector2(healthBar.X + healthBar.Width + 6, healthBar.Y + healthBar.Height / 2 - SmallFont.MeasureString(ch).Y / 2), Color.White);
            spriteBatch.DrawString(SmallFont, cm, new Vector2(manaBar.X + manaBar.Width + 6, manaBar.Y + manaBar.Height / 2 - SmallFont.MeasureString(cm).Y / 2), Color.White);

            for (int i = 0; i < abilities.Length; i++) {
                if (abilities[i] != null) {
                    Color c = Color.White;
                    float ability = 0f;
                    Rectangle rect = abilities[i].DrawRectangle;
                    if (rect.Contains(Mouse.GetState().Position))
                        c = Color.LightGray;
                    if (abilities[i].Timer < abilities[i].Cooldown * 1000f) {
                        c = Color.Gray;
                        ability = abilities[i].Timer / 1000f;

                        if (rect.Contains(Mouse.GetState().Position))
                            c = Color.DarkGray;
                    }
                    spriteBatch.Draw(abilities[i].Texture, rect, c);
                    if (abilities[i].Timer < abilities[i].Cooldown * 1000f) {
                        string text = "" + (int)(abilities[i].Cooldown - ability + 1);
                        spriteBatch.DrawString(Font, text,
                            new Vector2(rect.X + rect.Width / 2 - Font.MeasureString(text).X / 2,
                            rect.Y + rect.Height / 2 - Font.MeasureString(text).Y / 2), Color.LightGray);
                    }
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void generateRandomEnemy(bool isBoss) {
            int r = rand.Next(isBoss ? 0 : 5, isBoss ? 5 : 15);
            currentEnemy = new Enemy(500f, player);
            currentEnemy.Texture = Content.Load<Texture2D>("resources/EnemiesAndBosses/enemy_" + (r < 10 ? "0" + r : "" + r));
            float scaleFact = Math.Min(currentEnemy.Texture.Height * 3, WINDOW_HEIGHT - WINDOW_HEIGHT / 4 - 60) / (currentEnemy.Texture.Height * 3f);
            currentEnemy.DrawRectangle = new Rectangle(guiRectangles[2].X - (int)(scaleFact * currentEnemy.Texture.Width * 3),
                guiRectangles[0].Y - (int)(scaleFact * currentEnemy.Texture.Height * 3), (int)(scaleFact * currentEnemy.Texture.Width * 3), (int)(scaleFact * currentEnemy.Texture.Height * 3));
            currentEnemy.AbilityPower = 2 * (float)(Math.Log10(floor) + 1) * (isBoss ? 8 : 1);
            currentEnemy.CurrentHealth = 10 * (float)(Math.Log10(floor) + 1) * (isBoss ? 5 : 1);
            currentEnemy.MaxHealth = 10 * (float)(Math.Log10(floor) + 1) * (isBoss ? 5 : 1);
            currentEnemy.CurrentMana = 0;
            currentEnemy.MaxMana = 0;
            currentEnemy.Speed = 1500 * (isBoss ? 2f : 1);
            switch (r) {
                case 0:
                    currentEnemy.Name = "[BOSS] Evil Mage";
                    break;
                case 1:
                    currentEnemy.Name = "[BOSS] CodeBot";
                    break;
                case 2:
                    currentEnemy.Name = "[BOSS] Skeet";
                    break;
                case 3:
                    currentEnemy.Name = "[BOSS] Woodman";
                    break;
                case 4:
                    currentEnemy.Name = "[BOSS] 91*C Potato";
                    break;
                case 5:
                    currentEnemy.Name = "pls dont sue us";
                    break;
                case 6:
                    currentEnemy.Name = "null";
                    break;
                case 7:
                    currentEnemy.Name = "ACT";
                    break;
                case 8:
                    currentEnemy.Name = "*doot*\nSpooky Scary Skeleton\n*doot*";
                    break;
                case 9:
                    currentEnemy.Name = "Slime";
                    break;
                case 10:
                    currentEnemy.Name = "Cat";
                    break;
                case 11:
                    currentEnemy.Name = "Rock";
                    break;
                case 12:
                    currentEnemy.Name = "Spider";
                    break;
                case 13:
                    currentEnemy.Name = "Imp";
                    break;
                case 14:
                    currentEnemy.Name = "Orcoblin";
                    break;
            }
            player.CurrentEnemy = currentEnemy;
        }
    }
}
