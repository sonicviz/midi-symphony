using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace SongCraft
{
	public class SoundEngine
	{
		public static SoundEngine Instance
		{ get; set; }

		// Create a clock running at the specified beats per minute.
		int beatsPerMinute = 180;
		public Clock clock;

		// Prompt user to choose an output device (or if there is only one, use that one.
		public OutputDevice outputDevice;

		public SoundEngine()
		{
			clock = new Clock(beatsPerMinute);
			outputDevice = ChooseOutputDeviceFromConsole();
			Instance = this;
		}

		/// <summary>
		/// Chooses an output device, possibly prompting the user at the console.
		/// </summary>
		/// <returns>The chosen output device, or null if none could be chosen.</returns>
		/// If there is exactly one output device, that one is chosen without prompting the user.
		public static OutputDevice ChooseOutputDeviceFromConsole()
		{
			if (OutputDevice.InstalledDevices.Count == 0)
			{
				return null;
			}
			if (OutputDevice.InstalledDevices.Count == 1)
			{
				return OutputDevice.InstalledDevices[0];
			}
			Console.WriteLine("Output Devices:");
			for (int i = 0; i < OutputDevice.InstalledDevices.Count; ++i)
			{
				Console.WriteLine("   {0}: {1}", i, OutputDevice.InstalledDevices[i].Name);
			}
			Console.Write("Choose the id of an output device...");
			while (true)
			{
				ConsoleKeyInfo keyInfo = Console.ReadKey(true);
				int deviceId = (int)keyInfo.Key - (int)ConsoleKey.D0;
				if (deviceId >= 0 && deviceId < OutputDevice.InstalledDevices.Count)
				{
					return OutputDevice.InstalledDevices[deviceId];
				}
			}
		}

	}
}
