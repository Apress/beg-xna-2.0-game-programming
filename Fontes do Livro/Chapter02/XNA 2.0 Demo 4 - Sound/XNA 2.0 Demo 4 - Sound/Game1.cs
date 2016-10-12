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

namespace XNADemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        //  Sprite objects
        clsSprite mySprite1;
        clsSprite mySprite2;

        //  SpriteBatch which will draw (render) the sprite
        SpriteBatch spriteBatch;

        // Audio objects
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue myLoopingSound = null;

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
            audioEngine = new AudioEngine(@"Content\MySounds.xgs");

            //  Assume the default names for the wave and sound bank.  
            //   To change these names, change properties in XACT
            waveBank = new WaveBank(audioEngine, @"Content\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Sound Bank.xsb");

            myLoopingSound = soundBank.GetCue("notify");
            myLoopingSound.Play();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //  changing the back buffer size changes the window size (when in windowed mode)
            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 400;
            graphics.ApplyChanges();

            // Load a 2D texture sprite
            mySprite1 = new clsSprite(Content.Load<Texture2D>("xna_thumbnail"), new Vector2(0f, 0f), new Vector2(64f, 64f),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            mySprite2 = new clsSprite(Content.Load<Texture2D>("xna_thumbnail"), new Vector2(200f, 200f), new Vector2(64f, 64f),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            // Create a SpriteBatch to render the sprite 
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
               
            //  set the speed the sprites will move
            mySprite1.velocity = new Vector2(1, 1);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //  Free the previously alocated resources
            mySprite1.texture.Dispose();
            mySprite2.texture.Dispose();
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Move the sprite 
            mySprite1.Move();
            //  Change the sprite 2 position using the left thumbstick 
            mySprite2.position.X += GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            mySprite2.position.Y -= GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;

            //  Change the sprite 2 position using the keyboard
            //KeyboardState keyboardState = Keyboard.GetState();
            //if (keyboardState.IsKeyDown(Keys.Up))
            //    mySprite2.position.Y -= 1;
            //if (keyboardState.IsKeyDown(Keys.Down))
            //    mySprite2.position.Y += 1;
            //if (keyboardState.IsKeyDown(Keys.Left))
            //    mySprite2.position.X -= 1;
            //if (keyboardState.IsKeyDown(Keys.Right))
            //    mySprite2.position.X += 1;

            //  Change the sprite 2 position using the mouse 
            //if (mySprite2.position.X < Mouse.GetState().X)
            //    mySprite2.position.X += 1;
            //if (mySprite2.position.X > Mouse.GetState().X)
            //    mySprite2.position.X -= 1;
            //if (mySprite2.position.Y < Mouse.GetState().Y)
            //    mySprite2.position.Y += 1;
            //if (mySprite2.position.Y > Mouse.GetState().Y)
            //    mySprite2.position.Y -= 1;

            if (mySprite1.Collides(mySprite2))
            {
                mySprite1.velocity *= -1;
                GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                soundBank.PlayCue("chord");
            }
            else
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);

            // Play or stop an infinite looping sound when pressing the "B" button
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                if (myLoopingSound.IsPaused)
                    myLoopingSound.Resume();
                else
                    myLoopingSound.Pause();
            }

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // Draw the sprites
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            spriteBatch.Draw(mySprite1.texture, mySprite1.position, Color.White);
            spriteBatch.Draw(mySprite2.texture, mySprite2.position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
