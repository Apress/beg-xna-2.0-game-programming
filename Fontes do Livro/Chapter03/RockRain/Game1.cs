using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace RockRain
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        // Graphical stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D backgroundTexture, meteorTexture;

        // Audio Stuff
        private AudioComponent audioComponent;

        // Game stuff
        private Ship player;
        private int lastTickCount;
        private KeyboardState keyboard;
        private GamePadState gamepadstatus;
        private SpriteFont gameFont;
        private int rockCount;
        private const int STARTMETEORCOUNT = 10;
        private const int ADDMETEORTIME = 5000;

        // Rumble Effect
        private SimpleRumblePad rumblePad;

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
            // Create the basics game objects
            audioComponent = new AudioComponent(this);
            Components.Add(audioComponent);
            rumblePad = new SimpleRumblePad(this);
            Components.Add(rumblePad);

            // Initialize all other components
            base.Initialize();

            // Start the background music
            audioComponent.PlayCue("backmusic");

            this.Window.Title = "RockRain";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Add the spritebatch service
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            // Load all textures
            backgroundTexture = Content.Load<Texture2D>("SpaceBackground");
            meteorTexture = Content.Load<Texture2D>("RockRain");
            // Load game font
            gameFont = Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// Remove all meteors
        /// </summary>
        private void RemoveAllMeteors()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is Meteor)
                {
                    Components.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            gamepadstatus = GamePad.GetState(PlayerIndex.One);
            keyboard = Keyboard.GetState();
            if ((gamepadstatus.Buttons.Back == ButtonState.Pressed) || (keyboard.IsKeyDown(Keys.Escape)))
            {
                Exit();
            }

            // Start if not started yet
            if (player == null)
            {
                Start();
            }

            // Fun never ends..
            DoGameLogic();

            // Update all other components
            base.Update(gameTime);
        }

        /// <summary>
        /// Run the game logic
        /// </summary>
        private void DoGameLogic()
        {
            // Check collisions
            bool hasColision = false;
            Rectangle shipRectangle = player.GetBounds();
            foreach (GameComponent gc in Components)
            {
                if (gc is Meteor)
                {
                    hasColision = ((Meteor)gc).CheckCollision(shipRectangle);
                    if (hasColision)
                    {
                        // BOOM!
                        audioComponent.PlayCue("explosion");
                        // Shake!
                        rumblePad.RumblePad(500, 1.0f, 1.0f);
                        // Remove all previous meteors
                        RemoveAllMeteors();
                        // Let's start again
                        Start();

                        break;
                    }
                }
            }

            // Add a new meteor if is time
            CheckforNewMeteor();
        }

        /// <summary>
        /// Check if is a moment for a new rock!
        /// </summary>
        private void CheckforNewMeteor()
        {
            // Add a rock each ADDMETEORTIME
            if ((System.Environment.TickCount - lastTickCount) > ADDMETEORTIME)
            {
                lastTickCount = System.Environment.TickCount;
                Components.Add(new Meteor(this, ref meteorTexture));
                audioComponent.PlayCue("newmeteor");
                rockCount++;
            }
        }

        /// <summary>
        /// Initialize the game round
        /// </summary>
        private void Start()
        {
            // Add the meteors
            for (int i = 0; i < STARTMETEORCOUNT; i++)
            {
                Components.Add(new Meteor(this, ref meteorTexture));
            }

            // Create (if necessary) and put the player in start position
            if (player == null)
            {
                // Add the player component
                player = new Ship(this, ref meteorTexture);
                Components.Add(player);
            }
            player.PutinStartPosition();

            // Initialize a counter
            lastTickCount = System.Environment.TickCount;
            rockCount = STARTMETEORCOUNT;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // Draw background texture in a seperate pass
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 
                graphics.GraphicsDevice.DisplayMode.Width, 
                graphics.GraphicsDevice.DisplayMode.Height), 
                Color.LightGray);
            spriteBatch.End();

            // Start rendering sprites
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            // Draw the game components (sprites included)
            base.Draw(gameTime);
            // End rendering sprites
            spriteBatch.End();

            // Draw Score
            spriteBatch.Begin();
#if XBOX360
            spriteBatch.DrawString(gameFont, "Rocks: " + rockCount.ToString(), new Vector2(40, 30), Color.YellowGreen);
#else
            spriteBatch.DrawString(gameFont, "Rocks: " + rockCount.ToString(), new Vector2(15, 15), Color.YellowGreen);
#endif
            spriteBatch.End();
        }
    }
}
