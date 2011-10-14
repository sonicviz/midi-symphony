using System;
using Microsoft.Xna.Framework.Graphics;
using Midi;
using Microsoft.Xna.Framework;

namespace Studio
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Studio.GameInstance.Run();
        }
    }
#endif

    public class Studio
    {
        public Game game;

		static InputDevice inputDevice;

        public void Callback(float time)
        {
            //game.Exit();
        }

        public static void Stop()
        {
			if (SoundEngine.Instance.clock.IsRunning)
			{
				SoundEngine.Instance.clock.Stop();
				if (inputDevice != null)
				{
					inputDevice.StopReceiving();
				}
				SoundEngine.Instance.outputDevice.SilenceAllNotes();
			}

			SoundEngine.Instance.outputDevice.Close();
			if (inputDevice != null)
			{
				inputDevice.Close();
				inputDevice.RemoveAllEventHandlers();
			}

			// All done.
		}

		private static void Init()
		{
			new SoundEngine();
			if (SoundEngine.Instance.outputDevice == null)
			{
				Console.WriteLine("No output devices, so can't run this example.");
				//PressAnyKeyToContinue();
				return;
			}
			SoundEngine.Instance.outputDevice.Open();
        }

        public void Run()
        {
            Init();

            using (game = new Game2())
            {
                game.Run();
            }
            Stop();
        }

        public static Studio GameInstance = new Studio();
        public GraphicsDevice GraphicsDevice
        {
            get { return game.GraphicsDevice; }
        }
    }
}

