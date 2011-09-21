using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace SongCraft
{


	class Program
	{
		static InputDevice inputDevice;
		static Music m;

		static void Main(string[] args)
		{
			Init();

			//Arpeggiator arpeggiator = new Arpeggiator(inputDevice, outputDevice, clock);
			m = new Music();
			m.Generate(new Scope());


			SoundEngine.Instance.clock.Start();
			if (inputDevice != null)
			{
				inputDevice.StartReceiving(SoundEngine.Instance.clock);
			}

			m.Start();
			bool done = false;
			while (!done)
			{

				done = Tick(inputDevice, done);
			}

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
				PressAnyKeyToContinue();
				return;
			}
			SoundEngine.Instance.outputDevice.Open();

			// Prompt user to choose an input device (or if there is only one, use that one).
			inputDevice = ChooseInputDeviceFromConsole();
			if (inputDevice != null)
			{
				inputDevice.Open();
			}
		}

		private static bool Tick(InputDevice inputDevice, bool done)
		{
			Console.Clear();
			foreach (Clip i in m.clips)
			{
				//Print the name of the instrument
				Console.Out.Write(i.ToString() + "\t");
				foreach(NoteStream.Note n in i.cachedNotes[0].notes)
				{
					Console.Out.Write((char)((int)'a' + ((n.note+81)%7)));
				}
				Console.Out.WriteLine();
			}
			Console.Out.WriteLine();
			//foreach (Composition.Part p in composition.parts)
			//{
			//    foreach (Stream s in streams)
			//    {
			//        for (int idx = 0; idx < s.notes.Length; idx++)
			//        {
			//            Console.Out.Write(s.notes[idx] + "\t");
			//        }
			//        Console.Out.WriteLine();
			//    }
			//}

			ConsoleKey key = Console.ReadKey(true).Key;
			Pitch pitch;
			if (key == ConsoleKey.Escape)
			{
				done = true;
			}
			else if (key == ConsoleKey.DownArrow)
			{
				SoundEngine.Instance.clock.BeatsPerMinute -= 2;
			}
			else if (key == ConsoleKey.UpArrow)
			{
				SoundEngine.Instance.clock.BeatsPerMinute += 2;
			}
			else if (key == ConsoleKey.RightArrow)
			{

				//arpeggiator.Change(1);
			}
			else if (key == ConsoleKey.LeftArrow)
			{
				//arpeggiator.Change(-1);
			}
			else if (key == ConsoleKey.Spacebar)
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
				else
				{
					SoundEngine.Instance.clock.Start();
					if (inputDevice != null)
					{
						inputDevice.StartReceiving(SoundEngine.Instance.clock);
					}
				}
			}
			else if (key == ConsoleKey.Enter)
			{
				//arpeggiator.ToggleMode();
			}
			//else if (IsMockPitch(key, out pitch))
			//{
			//    // We've hit a QUERTY key which is meant to simulate a MIDI note, so
			//    // send the Note On to the output device and tell the arpeggiator.
			//    NoteOnMessage noteOn = new NoteOnMessage(SoundEngine.Instance.outputDevice, 0, pitch, 100,
			//        SoundEngine.Instance.clock.Time);
			//    SoundEngine.Instance.clock.Schedule(noteOn);
			//    //arpeggiator.NoteOn(noteOn);
			//    // We don't get key release events for the console, so schedule a
			//    // simulated Note Off one beat from now.
			//    NoteOffMessage noteOff = new NoteOffMessage(SoundEngine.Instance.outputDevice, 0, pitch, 100,
			//        SoundEngine.Instance.clock.Time + 1);
			//    CallbackMessage.CallbackType noteOffCallback = beatTime =>
			//    {
			//        arpeggiator.NoteOff(noteOff);
			//    };
			//    SoundEngine.Instance.clock.Schedule(new CallbackMessage(beatTime => arpeggiator.NoteOff(noteOff),
			//        noteOff.Time));
			//}
			return done;
		}


        /// <summary>
        /// Chooses an input device, possibly prompting the user at the console.
        /// </summary>
        /// <returns>The chosen input device, or null if none could be chosen.</returns>
        /// If there is exactly one input device, that one is chosen without prompting the user.
        public static InputDevice ChooseInputDeviceFromConsole()
        {
            if (InputDevice.InstalledDevices.Count == 0)
            {
                return null;
            }
            if (InputDevice.InstalledDevices.Count == 1)
            {
                return InputDevice.InstalledDevices[0];
            }
            Console.WriteLine("Input Devices:");
            for (int i = 0; i < InputDevice.InstalledDevices.Count; ++i)
            {
                Console.WriteLine("   {0}: {1}", i, InputDevice.InstalledDevices[i]);
            }
            Console.Write("Choose the id of an input device...");
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                int deviceId = (int)keyInfo.Key - (int)ConsoleKey.D0;
                if (deviceId >= 0 && deviceId < InputDevice.InstalledDevices.Count)
                {
                    return InputDevice.InstalledDevices[deviceId];
                }
            }
        }

        /// <summary>
        /// Prints "Press any key to continue." with a newline, then waits for a key to be pressed.
        /// </summary>
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Key mappings for mock MIDI keys on the QWERTY keyboard.
        /// </summary>
        private static Dictionary<ConsoleKey, int> mockKeys = new Dictionary<ConsoleKey,int>
        {
            {ConsoleKey.Q,        53},
            {ConsoleKey.D2,       54},
            {ConsoleKey.W,        55},
            {ConsoleKey.D3,       56},
            {ConsoleKey.E,        57},
            {ConsoleKey.D4,       58},
            {ConsoleKey.R,        59},
            {ConsoleKey.T,        60},
            {ConsoleKey.D6,       61},
            {ConsoleKey.Y,        62},
            {ConsoleKey.D7,       63},
            {ConsoleKey.U,        64},
            {ConsoleKey.I,        65},
            {ConsoleKey.D9,       66},
            {ConsoleKey.O,        67},
            {ConsoleKey.D0,       68},
            {ConsoleKey.P,        69},
            {ConsoleKey.OemMinus, 70},
            {ConsoleKey.Oem4,     71},
            {ConsoleKey.Oem6,     72}
        };

        /// <summary>
        /// If the specified key is one of the computer keys used for mock MIDI input, returns true
        /// and sets pitch to the value.
        /// </summary>
        /// <param name="key">The computer key pressed.</param>
        /// <param name="pitch">The pitch it mocks.</param>
        /// <returns></returns>
        public static bool IsMockPitch(ConsoleKey key, out Pitch pitch)
        {
            if (mockKeys.ContainsKey(key))
            {
                pitch = (Pitch)mockKeys[key];
                return true;
            }
            pitch = 0;
            return false;
        }
	}
}
