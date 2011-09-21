using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace SongCraft
{
	//class Arpeggiator
	//{
	//    public Arpeggiator(InputDevice inputDevice, OutputDevice outputDevice, Clock clock)
	//    {
	//        this.inputDevice = inputDevice;
	//        this.outputDevice = outputDevice;
	//        this.clock = clock;
	//        this.currentChordPattern = 0;
	//        this.currentScalePattern = 0;
	//        this.playingChords = false;
	//        this.lastSequenceForPitch = new Dictionary<Pitch, List<Pitch>>();

	//        if (inputDevice != null)
	//        {
	//            inputDevice.NoteOn += new InputDevice.NoteOnHandler(this.NoteOn);
	//            inputDevice.NoteOff += new InputDevice.NoteOffHandler(this.NoteOff);
	//        }
	//    }

	//    /// <summary>
	//    /// String describing the arpeggiator's current configuration.
	//    /// </summary>
	//    public string Status
	//    {
	//        get
	//        {
	//            lock (this)
	//            {
	//                if (playingChords)
	//                {
	//                    return "Chord: " + Chord.Patterns[currentChordPattern].Name;
	//                }
	//                else
	//                {
	//                    return "Scale: " + Scale.Patterns[currentScalePattern].Name;
	//                }
	//            }
	//        }
	//    }

	//    /// <summary>
	//    /// Toggle between playing chords and playing scales.
	//    /// </summary>
	//    public void ToggleMode()
	//    {
	//        lock (this)
	//        {
	//            playingChords = !playingChords;
	//        }
	//    }

	//    /// <summary>
	//    /// Changes the current chord or scale, whichever is the current mode.
	//    /// </summary>
	//    public void Change(int delta)
	//    {
	//        lock (this)
	//        {
	//            if (playingChords)
	//            {
	//                currentChordPattern = currentChordPattern + delta;
	//                while (currentChordPattern < 0)
	//                {
	//                    currentChordPattern += Chord.Patterns.Length;
	//                }
	//                while (currentChordPattern >= Chord.Patterns.Length)
	//                {
	//                    currentChordPattern -= Chord.Patterns.Length;
	//                }
	//            }
	//            else
	//            {
	//                currentScalePattern = currentScalePattern + delta;
	//                while (currentScalePattern < 0)
	//                {
	//                    currentScalePattern += Scale.Patterns.Length;
	//                }
	//                while (currentScalePattern >= Scale.Patterns.Length)
	//                {
	//                    currentScalePattern -= Scale.Patterns.Length;
	//                }
	//            }
	//        }
	//    }

	//    public void NoteOn(NoteOnMessage msg)
	//    {
	//        lock (this)
	//        {
	//            List<Pitch> pitches = new List<Pitch>();
	//            if (playingChords)
	//            {
	//                Chord chord = new Chord(msg.Pitch.NotePreferringSharps(),
	//                    Chord.Patterns[currentChordPattern], 0);
	//                Pitch p = msg.Pitch;
	//                for (int i = 0; i < chord.NoteSequence.Length; ++i)
	//                {
	//                    p = chord.NoteSequence[i].PitchAtOrAbove(p);
	//                    pitches.Add(p);
	//                }
	//            }
	//            else
	//            {
	//                Scale scale = new Scale(msg.Pitch.NotePreferringSharps(),
	//                    Scale.Patterns[currentScalePattern]);
	//                Pitch p = msg.Pitch;
	//                for (int i = 0; i < scale.NoteSequence.Length; ++i)
	//                {
	//                    p = scale.NoteSequence[i].PitchAtOrAbove(p);
	//                    pitches.Add(p);
	//                }
	//                pitches.Add(msg.Pitch + 12);
	//            }
	//            lastSequenceForPitch[msg.Pitch] = pitches;
	//            for (int i = 1; i < pitches.Count; ++i)
	//            {
	//                clock.Schedule(new NoteOnMessage(outputDevice, msg.Channel,
	//                    pitches[i], msg.Velocity, msg.Time + i));
	//            }
	//        }
	//    }

	//    public void NoteOff(NoteOffMessage msg)
	//    {
	//        if (!lastSequenceForPitch.ContainsKey(msg.Pitch))
	//        {
	//            return;
	//        }
	//        List<Pitch> pitches = lastSequenceForPitch[msg.Pitch];
	//        lastSequenceForPitch.Remove(msg.Pitch);
	//        for (int i = 1; i < pitches.Count; ++i)
	//        {
	//            clock.Schedule(new NoteOffMessage(outputDevice, msg.Channel,
	//                pitches[i], msg.Velocity, msg.Time + i));
	//        }
	//    }

	//    private InputDevice inputDevice;
	//    private OutputDevice outputDevice;
	//    private Clock clock;
	//    private int currentChordPattern;
	//    private int currentScalePattern;
	//    private bool playingChords;
	//    private Dictionary<Pitch, List<Pitch>> lastSequenceForPitch;
	//}

}
