using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Midi;
using Microsoft.Xna.Framework.Input;

namespace Studio
{
    public class Actor
    {
        public List<Actor> children = new List<Actor>();

        public Matrix local = Matrix.Identity;
        public float Width;
        public float Height;

        public VertexBuffer<VertexPositionColor> vb;
        public Color c;

        public Matrix cachedParent = Matrix.Identity;
        public Matrix cachedWorld = Matrix.Identity;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public virtual void Draw(Matrix parent)
        {
            cachedParent = parent;
            BlendState bs = new BlendState();
            Game2.Instance.GraphicsDevice.BlendState = BlendState.Opaque;
            // TODO: Add your drawing code here

            cachedWorld = local * parent;

            DrawRect(cachedWorld, new Vector3(0, 0, 0), new Vector2(1, 2), Color.Blue);
            //Between batch.Begin() and batch.End() we are allowed to draw.
            foreach (Actor b in children) //For every button run through the code between the {}
            {
                b.Draw(cachedWorld);
            }

            //foreach (Actor c in children)
            //{
            //    c.Draw();
            //    parentPos.Y += 0.2f;
            //}

        }

        public virtual void HandleMouse(MouseController ourMouse, MouseState mouseState) 
        {
            foreach (Actor a in children)
            {
                a.HandleMouse(ourMouse, mouseState);
            } 
        }

        public Vector3 WorldPos
        {
            get
            {
                return (cachedWorld).Translation;
            }
        }


        public void DrawBar(Matrix parent, Vector3 start, float pct, Color c)
        {
            DrawRect(parent, start, new Vector2(pct, 0.05f), c);
        }

        public void DrawRect(Matrix parent, Vector3 pos, Vector2 size, Color c)
        {

            Game2.Instance.effect.World = Matrix.CreateScale(new Vector3(size.X * 2, size.Y, 1.0f)) * Matrix.CreateTranslation(new Vector3(-1, -1, 0) + pos * 2.0f) * parent;

            Game2.Instance.effect.LightingEnabled = false;
            Game2.Instance.effect.DiffuseColor = c.ToVector3();
            Game2.Instance.effect.CurrentTechnique.Passes[0].Apply();
            Game2.Instance.GraphicsDevice.DrawUserPrimitives(Game2.Instance.quad.primitiveType, Game2.Instance.quad.data, 0, 4);
        }




        public Vector3 Position
        {
            get
            {
                return local.Translation;
            }
            set
            {
                local.Translation = value;
            }
        }
    }
}
