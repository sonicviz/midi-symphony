using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;

namespace Studio
{

    public class Drummer : Clip
    {
        protected override void AddMessage(OutputDevice outputDevice, int channel, float time, float noteLength, float tempo, int note, List<Message> messagesForOneMeasure)
        {
            messagesForOneMeasure.Add(new PercussionMessage(outputDevice, (Percussion)note,
                                        volume, time * tempo));
        }
    }

    public class BassGuitar : Clip
    {

        protected override void AddMessage(OutputDevice outputDevice, int channel, float time, float noteLength, float tempo, int note, List<Message> messagesForOneMeasure)
        {

            messagesForOneMeasure.Add(new NoteOnMessage(outputDevice, (Channel)channel, (Pitch)note,
                                        volume, time * tempo));
            messagesForOneMeasure.Add(new NoteOffMessage(outputDevice, (Channel)channel, (Pitch)note,
                                        volume, (time + noteLength * 1.0f) * tempo));
        }
    }
}
