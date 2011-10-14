using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Midi;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Studio
{
    public class Fraction
    {
        public Fraction()
        {
        }
        
        public Fraction(int inN, int inD)
        {
            n = inN;
            d = inD;
        }

        public int n;//Numerator
        public int d;//Denomenator

        public float Value { get { return (float)n/(float)d; }}
    }

    public class Note : Actor
    {
        public Note()
        {
        }
        public Note(int val, Fraction inStarttime, Fraction inDuration)
        {
            value = val;
            starttime = inStarttime;
            duration = inDuration;
        }

        public Fraction starttime;
        public Fraction duration;
        public int value;

        public override void Draw(Matrix parent)
        {
            base.Draw(parent);

            float scale = 32.0f;
            float noteScale = 0.01f;

            Vector3 pos = new Vector3(starttime.Value / scale, value * noteScale, 0);

            local.Translation = pos;

            Height = noteScale * 100.0f;
            Width = duration.Value *100.0f/ scale;
            if (Hovering)
            {
                DrawBar(local * parent, new Vector3(0,0,0), duration.Value / scale, Color.White);
            }
            else
            {
                DrawBar(local * parent, new Vector3(0, 0, 0), duration.Value / scale, Color.Brown);
            }


        }

        public override void HandleMouse(MouseController ourMouse, Microsoft.Xna.Framework.Input.MouseState mouseState)
        {
            base.HandleMouse(ourMouse, mouseState);

            if (ourMouse.ourMouse.ButtonClick(this)) //There's only one button, so we just hardcode it.
                //If we're hovering over the mouse
                Hovering = true; //We ARE hovering
            else
                Hovering = false; //Not hovering.
            //We don't even need to use ButtonClick() again if we know if we're hovering.
            if (mouseState.LeftButton == ButtonState.Pressed && Hovering) //If we're clicking with the Left Mouse Button and we're over the button.
            {
                if (!Clicking)
                {
                    Clicking = true; //We ARE clicking
                    OnClick();
                }
            }
            else
                Clicking = false; //Not clicking
        }

        public bool Hovering;
        public bool Clicking;

        public delegate void CallbackFunc();
        public CallbackFunc OnClick;


    }

	public abstract class Clip : Actor
	{
		public float startTime = 0;
		public float endTime = 64;

		public static float tempo = 2.0f;

		public float tempoScale = 1;
		public int volume = 127;

        public List<Note> notes;

		public List<Message> messagesForOneMeasure;
		public Fraction signature;
		int channelStart;
		static int channelsUsed;
        public int[] scale;

        int key = 0;
        int octave = 5;

		public void Init(Midi.Instrument inst, Scale inScale)
		{
			channelStart = channelsUsed;

			SoundEngine.Instance.outputDevice.SendProgramChange((Channel)channelStart, inst);
			channelsUsed += 1;

            if (inScale != null)
            {
                scale = inScale.Pattern.Ascent;
            }

			Update();
		}

		public void Update()
		{
			this.messagesForOneMeasure = new List<Message>();

            int numRepeats = 2;

            children = new List<Actor>();

			for (int repeat = 0; repeat < numRepeats; repeat++)
			{
                
			    for (int idx2 = 0; idx2 < notes.Count; idx2++)
			    {
    				Note note = notes[idx2];

                    if (note != null)
                    {
                        children.Add(note);

                        int playedNote = note.value;

                        if (scale != null)
                        {

                            if (note.value < 0)
                            {
                                int index = scale.Length + note.value;
                                playedNote = ((octave - 1) * 12) + key + scale[index % scale.Length];
                            }
                            else
                            {
                                playedNote = ((octave + (note.value / scale.Length)) * 12) + key + scale[note.value % scale.Length];
                            }
                        }
                        if (playedNote > 0)
                        {
                            AddMessage(SoundEngine.Instance.outputDevice, channelStart + idx2, (float)repeat + note.starttime.Value, note.duration.Value, tempo * tempoScale, playedNote, messagesForOneMeasure);
                        }
                    }
					
				}
			}
		}

        public override void Draw(Matrix parent)
        {
            base.Draw(parent);
            //foreach (Note n in notes)
            //{
            //    Matrix m = local * parent;
            //    n.Draw(m);
            //}
            
        }


		/// <summary>
		/// 
		/// </summary>
		/// <param name="outputDevice"></param>
		/// <param name="s"></param>
		/// <param name="time">between 0 and 1!</param>
		/// <param name="noteLength"></param>
		/// <param name="note"></param>
		/// <param name="messagesForOneMeasure"></param>
		protected abstract void AddMessage(OutputDevice outputDevice, int channel, float time, float noteLength, float tempo, int note, List<Message> messagesForOneMeasure);

	}
}
