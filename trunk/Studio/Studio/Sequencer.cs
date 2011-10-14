using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Midi;

namespace Studio
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SequencerPanel : Actor
    {
        
        public SequencerPanel()
        {
            
            Button playButton = new Button(new Vector3(0, -0.900f, 0), Game2.Instance.tri, Color.Green);
            playButton.OnClick = Play;
            this.children.Add(playButton);

            Button stopButton = new Button(new Vector3(.66f, -0.900f, 0), Game2.Instance.quad, Color.LightGray);
            stopButton.OnClick = Stop;
            this.children.Add(stopButton);

            CreateDrums();
            CreateArpeggios();
        }

        private void CreateDrums()
        {
            Drummer d = new Drummer();
            d.signature = new Fraction(4, 4);
            d.notes = new System.Collections.Generic.List<Note>(new Note[] { 
                new Note(35, new Fraction(0,4), new Fraction(1,4)),
                new Note(44, new Fraction(1,4), new Fraction(1,4)),
                new Note(40, new Fraction(2,4), new Fraction(1,4)),
                new Note(44, new Fraction(3,4), new Fraction(1,4)),
            });
            d.Init(Instrument.Accordion, null);

            tracks.Add(d);
            children.Add(d);
        }
        private void CreateArpeggios()
        {
            BassGuitar d = new BassGuitar();
            d.signature = new Fraction(4, 4);
            d.notes = new System.Collections.Generic.List<Note>(new Note[] { 
                new Note(0, new Fraction(0,4), new Fraction(1,8)),
                new Note(1, new Fraction(1,4), new Fraction(1,8)),
                new Note(2, new Fraction(2,4), new Fraction(1,8)),
                new Note(3, new Fraction(3,4), new Fraction(1,8)),
            });
            d.Init(Instrument.AcousticGrandPiano, new Scale(new Midi.Note('E'), Scale.HarmonicMinor));

            tracks.Add(d);
            children.Add(d);
        }

        List<Clip> tracks = new List<Clip>();

        public void Stop()
        {
            if (!SoundEngine.Instance.clock.IsRunning)
            {
                SoundEngine.Instance.clock.Reset();
                return;
            }

            SoundEngine.Instance.clock.Stop();
            SoundEngine.Instance.clock.Reset();
        }

        public void Play()
        {
            if (SoundEngine.Instance.clock.IsRunning)
            {
                Stop();
            }

            foreach (Clip c in tracks)
            {
                SoundEngine.Instance.clock.Schedule(c.messagesForOneMeasure, 0);
            }

            SoundEngine.Instance.clock.Schedule(new CallbackMessage(Callback, 4));

            SoundEngine.Instance.clock.Start();
            //if (inputDevice != null)
            //{
            //    inputDevice.StartReceiving(SoundEngine.Instance.clock);
            //}
        }

        public void Callback(float time)
        {
            //SoundEngine.Instance.clock.shouldStop = true;
            SoundEngine.Instance.clock.shouldLoop = true;

            foreach (Clip c in tracks)
            {
                SoundEngine.Instance.clock.Schedule(c.messagesForOneMeasure, 0);
            }

            SoundEngine.Instance.clock.Schedule(new CallbackMessage(Callback, 4));
        }

        public override void Draw(Matrix parent)
        {
            //float scale = 32.0f;
            //float noteScale = 0.01f;
            //Vector2 parentPos = new Vector2(0, 0.5f);

            Game2.Instance.effect.World = Matrix.CreateScale(1.99f, 0.05f, 1.0f) * Matrix.CreateTranslation(new Vector3(-1, 0, 0)) * parent;
            Game2.Instance.effect.LightingEnabled = false;
            Game2.Instance.effect.DiffuseColor = Vector3.One;
            Game2.Instance.effect.CurrentTechnique.Passes[0].Apply();
            Game2.Instance.GraphicsDevice.DrawUserPrimitives(Game2.Instance.quad.primitiveType, Game2.Instance.quad.data, 0, 4);

            DrawBar(parent, new Vector3(0, 0, 0), SoundEngine.Instance.clock.Time / 32.0f, Color.Green);

            DrawRect(parent, new Vector3(0.95f, 0.0f, 0), new Vector2(0.05f, 2.0f), Color.LightGray);

            if (vb != null)
            {
                Game2.Instance.effect.World = Matrix.CreateScale(0.05f) * local * parent;
                Game2.Instance.effect.LightingEnabled = false;
                Game2.Instance.effect.DiffuseColor = c.ToVector3();
                Game2.Instance.effect.CurrentTechnique.Passes[0].Apply();
                Game2.Instance.GraphicsDevice.DrawUserPrimitives(vb.primitiveType, vb.data, 0, 3);
            }

            base.Draw(parent);
        }


        public class Button : Actor
        {
            public bool Hovering;
            public bool Clicking;

            public delegate void CallbackFunc();
            public CallbackFunc OnClick;

            public Button(Vector3 position, VertexBuffer<VertexPositionColor> inVb, Color color)
            {
                this.local = Matrix.CreateTranslation(position);
                c = color;
                vb = inVb;
                Width = 0.1f;
                Height = 0.1f;
            }

            public override void HandleMouse(MouseController ourMouse, MouseState mouseState)
            {
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

            public override void Draw(Matrix parent)
            {
                base.Draw(parent);

                //For every button in our list, draw it
                //b.Draw(batch);
                Game2.Instance.effect.World = Matrix.CreateScale(0.2f) * local * parent;
                Game2.Instance.effect.LightingEnabled = false;
                if (Clicking)
                {
                    Game2.Instance.effect.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f);
                }
                else if (Hovering)
                {
                    Game2.Instance.effect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else
                {
                    Game2.Instance.effect.DiffuseColor = c.ToVector3();
                }
                Game2.Instance.effect.CurrentTechnique.Passes[0].Apply();
                Game2.Instance.GraphicsDevice.DrawUserPrimitives(vb.primitiveType, vb.data, 0, 3);
            }


        }


    }
}
