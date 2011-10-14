using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Studio
{
    public class MouseController : DrawableGameComponent
    {
        public MouseHandler ourMouse;
        private int cachedScrollWheelValue;

        public MouseController(Game g)
            : base(g)
        {
            ourMouse = new MouseHandler(new Vector2(0, 0), g);
        }

        private void HandleMouse()
        {
            mouseState = Mouse.GetState(); //Get the current state of the mouse

            foreach (Actor a in Game2.Instance.actors)
            {
                a.HandleMouse(this, mouseState);
            }

            if (mouseState.ScrollWheelValue != cachedScrollWheelValue)
            {
                int delta = mouseState.ScrollWheelValue - cachedScrollWheelValue;
                cachedScrollWheelValue = mouseState.ScrollWheelValue;

                Game2.Instance.AdjustZoom(delta);
            }
        }



        public MouseState mouseState { get; set; }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //Draw the mouse.

            DrawCursor(Game2.Instance.effect, Game2.Instance.tri);

        }

        private void DrawCursor(BasicEffect effect, VertexBuffer<VertexPositionColor> tri)
        {
            effect.View = Matrix.Identity;
            effect.Projection = Matrix.Identity;
            effect.World = Matrix.CreateScale(0.01f) * Matrix.CreateTranslation(ourMouse.WorldPos);
            effect.LightingEnabled = false;
            effect.DiffuseColor = Vector3.One;
            effect.CurrentTechnique.Passes[0].Apply();
            Game.GraphicsDevice.DrawUserPrimitives(tri.primitiveType, tri.data, 0, 3);
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Game.Exit();

            HandleMouse(); //Check clicking
            ourMouse.Update(); //Update the mouse's position.

            Console.Out.WriteLine(ourMouse.WorldPos);

            base.Update(gameTime);
        }

        public class MouseHandler
        {
            public Vector2 pos;
            private MouseState mouseState;
            public Game g;
            //We create variables

            public MouseHandler(Vector2 pos, Game inGame)
            {
                g = inGame;
                this.pos = pos; //Inital pos (0,0)
            }
            //On Update we will call this function
            public void Update()
            {
                mouseState = Mouse.GetState(); //Needed to find the most current mouse states.
                this.pos.X = mouseState.X; //Change x pos to mouseX
                this.pos.Y = mouseState.Y; //Change y pos to mouseY
            }

            public bool ButtonClick(Actor b)
            {
                if (WorldPos.X >= b.WorldPos.X // To the right of the left side
                && WorldPos.X < b.WorldPos.X + b.Width //To the left of the right side
                && WorldPos.Y > b.WorldPos.Y //Below the top side
                && WorldPos.Y < b.WorldPos.Y + b.Height) //Above the bottom side
                {
                    return true; //We are; return true.
                }
                else
                    return false; //We're not; return false.
            }

            public Vector3 WorldPos
            {
                get
                {
                    return new Vector3(-1 + pos.X * 2.0f / (float)g.GraphicsDevice.Viewport.Width, 1 - pos.Y * 2.0f / (float)g.GraphicsDevice.Viewport.Height, 0.5f);
                }
            }
        }


    }
}
