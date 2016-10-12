#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

#endregion

namespace RockRainEnhanced.Core
{
    /// <summary>
    /// Handle the audio in the game
    /// </summary>
    public class AudioComponent : GameComponent
    {
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;

        public AudioComponent(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to 
        /// before starting to run.  This is where it can query for any required
        ///  services and load content.
        /// </summary>
        public override void Initialize()
        {
            // Initialize sound engine
            audioEngine = new AudioEngine("Content\\audio.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Wave Bank.xwb");
            if (waveBank != null)
            {
                soundBank = new SoundBank(audioEngine, "Content\\Sound Bank.xsb");
            }

            base.Initialize();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            audioEngine.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// Play a cue
        /// </summary>
        /// <param name="cue">cue to be played</param>
        public void PlayCue(string cue)
        {
            soundBank.PlayCue(cue);
        }

        public Cue GetCue(string cue)
        {
            return soundBank.GetCue(cue);
        }
    }
}