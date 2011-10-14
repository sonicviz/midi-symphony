using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Studio
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game2 : Microsoft.Xna.Framework.Game
    {
        public static Game2 Instance
        {
            get;
            set;
        }

        public List<Actor> actors = new List<Actor>();


        GraphicsDeviceManager graphics;

        public BasicEffect effect;
        //Model myModel;
        bool paused = false;
        Vector3 
            cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f), 
            modelPosition, 
            modelVelocity;
        
        float 
            modelRotation = MathHelper.PiOver2,
            acceleration = 1.0f,
            deceleration = 0.95f,
            rotRate = 0.10f,
            clipDist = 10000.0f,
            aspectRatio;

        //use these accessors with the console to change the way the ship moves
        public float ModelRotation
        {
            get { return modelRotation; }
            set { modelRotation = value; }
        }
        public float Acceleration
        {
            get { return acceleration; }
            set { acceleration = value;}
        }
        public float Deceleration
        {
            get { return deceleration; }
            set { deceleration = value; }
        }
        public float RotRate
        {
            get { return rotRate; }
            set { rotRate = value; }
        }
        public float ClipDist
        {
            get { return clipDist; }
            set { clipDist = value; }
        }
        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }
        public Vector3 CameraPosition
        {
            get { return cameraPosition; }
            set { cameraPosition = value; }
        }
        public Vector3 ModelPosition
        {
            get { return modelPosition; }
            set { modelPosition = value; }
        }
        public Vector3 ModelVelocity
        {
            get { return modelVelocity; }
            set { modelVelocity = value; }
        }
        
        public Game2()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            input = new InputManager(this);
        }

        public void ResetShip()
        {
            modelPosition = Vector3.Zero;
            modelVelocity = Vector3.Zero;
            modelRotation = MathHelper.PiOver2;
        }

        public void Reset()
        {
            ResetShip();
            acceleration = 1.0f;
            deceleration = 0.95f;
            rotRate = 0.10f;
            cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
            aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width /
                (float)graphics.GraphicsDevice.Viewport.Height;
        }

        public void Pause()
        {
            paused = true;
        }
        public void UnPause()
        {
            paused = false;
        }

        InputManager input;
        StudioConsole console;
        public VertexBuffer<VertexPositionColor> quad;
        public VertexBuffer<VertexPositionColor> tri;
        public VertexBuffer<VertexPositionColor> line;

        protected override void Initialize()
        {
            console = new StudioConsole(this, Content.Load<SpriteFont>("ConsoleFont"));
            Components.Add(new MouseController(this));
            input.AssignControl("RotVelocity", Pad.ThumbSticks.Left.X);
            input.AssignControl("Accel", Pad.Triggers.Right);

            camera = Matrix.CreateTranslation(new Vector3(1, 1, 0));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //myModel = Content.Load<Model>("Models\\p1_wedge");

            effect = new BasicEffect(GraphicsDevice);
            effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),
                (float)graphics.GraphicsDevice.Viewport.Width /
                    (float)graphics.GraphicsDevice.Viewport.Height,
                0.01f,
                clipDist);
            aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width /
                (float)graphics.GraphicsDevice.Viewport.Height;

            InitQuad();
            InitTri();
            InitLine();

            SequencerPanel p = new SequencerPanel();
            actors.Add(p);
        }

        private void InitLine()
        {
            line = new VertexBuffer<VertexPositionColor>();

            VertexPositionColor[] data = new VertexPositionColor[]{
                new VertexPositionColor(new Vector3(0, 0, 0.5f), Color.White),
                new VertexPositionColor(new Vector3(1, 0, 0.5f), Color.White),
            };
            line.Init(data);
        }

        private void InitTri()
        {
            tri = new VertexBuffer<VertexPositionColor>();

            VertexPositionColor[] data = new VertexPositionColor[]{
                new VertexPositionColor(new Vector3(0, 0, 0.5f), Color.White),
                new VertexPositionColor(new Vector3(1, 0, 0.5f), Color.White),

                new VertexPositionColor(new Vector3(1, 0, 0.5f), Color.White),
                new VertexPositionColor(new Vector3(1, 1, 0.5f), Color.White),

                new VertexPositionColor(new Vector3(1, 1, 0.5f), Color.White),
                new VertexPositionColor(new Vector3(0, 0, 0.5f), Color.White),

            };
            tri.Init(data);
        }

        private void InitQuad()
        {
            quad = new VertexBuffer<VertexPositionColor>();

            VertexPositionColor[] data = new VertexPositionColor[]{
                new VertexPositionColor(new Vector3(0, 0, 0.5f), Color.Red),
                new VertexPositionColor(new Vector3(1, 0, 0.5f), Color.Red),

                new VertexPositionColor(new Vector3(1, 0, 0.5f), Color.Blue),
                new VertexPositionColor(new Vector3(1, 1, 0.5f), Color.Blue),

                new VertexPositionColor(new Vector3(1, 1, 0.5f), Color.Green),
                new VertexPositionColor(new Vector3(0, 1, 0.5f), Color.Green),

                new VertexPositionColor(new Vector3(0, 1, 0.5f), Color.White),
                new VertexPositionColor(new Vector3(0, 0, 0.5f), Color.White),
            };
            quad.Init(data);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// get input from user
        /// </summary>
        protected void UpdateInput()
        {
            if (input.GamePads[PlayerIndex.One].IsConnected)
            {
                // Rotate the model using the left thumbstick, and scale it down.
                modelRotation -= input.ControlState(PlayerIndex.One, "RotVelocity") * rotRate;

                // Create some velocity if the right trigger is down.
                Vector3 modelVelocityAdd = Vector3.Zero;

                // Find out what direction we should be thrusting, using rotation.
                modelVelocityAdd.X = -(float)Math.Sin(modelRotation);
                modelVelocityAdd.Z = -(float)Math.Cos(modelRotation);

                // Now scale our direction by how hard the trigger is down.
                float accel = input.ControlState(PlayerIndex.One, "Accel");
                modelVelocityAdd *= accel;

                // Finally, add this vector to our velocity.
                modelVelocity += modelVelocityAdd * acceleration;

                
                GamePad.SetVibration(PlayerIndex.One, accel,
                    accel);


                // In case you get lost, press A to warp back to the center.
                if (input.GamePads[PlayerIndex.One].JustPressed(Buttons.A))
                {
                    ResetShip();
                }
                // In case you mess up something badly, or just want to start over, press start to reset everything
                if (input.GamePads[PlayerIndex.One].JustPressed(Buttons.Start))
                {
                    Reset();
                }

            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!paused)
            {
                // Get some input.
                UpdateInput();

                // Add velocity to the current position.
                modelPosition += modelVelocity;

                // Bleed off velocity over time.
                modelVelocity *= deceleration;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            //graphics.GraphicsDevice.RenderState.DepthBufferEnable = false;

            //// Copy any parent transforms.
            //Matrix[] transforms = new Matrix[myModel.Bones.Count];
            //myModel.CopyAbsoluteBoneTransformsTo(transforms);

            //// Draw the model. A model can have multiple meshes, so loop.
            //foreach (ModelMesh mesh in myModel.Meshes)
            //{
            //    // This is where the mesh orientation is set, as well as our camera and projection.
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();
            //        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation)
            //            * Matrix.CreateTranslation(modelPosition);
            //        effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            //        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
            //            aspectRatio, 1.0f, 10000.0f);
            //    }
            //    // Draw the mesh, using the effects set above.
            //    mesh.Draw();
            //}

            Game2.Instance.effect.Projection = Matrix.Identity;
            Game2.Instance.effect.View = Matrix.Identity;

            foreach (Actor a in actors)
            {
                a.Draw(camera);
            }
            base.Draw(gameTime);

        }
        public Matrix camera = Matrix.Identity;


        public void AdjustZoom(int delta)
        {
            float factor = (float)Math.Exp(delta*0.001f);
            camera *= Matrix.CreateScale(factor);
        }
    }
}
