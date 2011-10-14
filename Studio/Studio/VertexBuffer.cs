using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Studio
{

    public class VertexBuffer<T> where T : struct, IVertexType
    {
        public Color color;
        public VertexBuffer buffer;
        public VertexDeclaration vertexDecl;
        public int vertCount;

        public T[] data;

        public Texture2D myTexture;
        public PrimitiveType primitiveType;
        public int vertsPerPrimitive;
        public FillMode fillMode;

        public VertexBuffer()
        {
            this.color = Color.DeepPink;
            this.fillMode = FillMode.WireFrame;
        }

        public void OnCreateDevice()
        {
            if (vertCount > 0)
            {
                buffer = new VertexBuffer(Studio.GameInstance.GraphicsDevice, typeof(T), vertCount, BufferUsage.WriteOnly);
                if (vertexDecl != null)
                {
                    buffer.SetData(vertexDecl.GetVertexElements());

                }
            }
        }

        protected virtual void Dispose(bool all)
        {
            if (buffer != null)
            {
                buffer.Dispose();
                buffer = null;
            }

            if (vertexDecl != null)
            {
                vertexDecl.Dispose();
                vertexDecl = null;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        internal void Init(T[] d)
        {
            vertsPerPrimitive = 2;
            primitiveType = PrimitiveType.LineList;
            vertCount = d.Length;


            if (vertCount > 0)
            {
                if (data == null || data.Length != d.Length)
                {
                    OnCreateDevice();
                    data = d;
                    buffer.SetData(d);
                    color = Color.White;
                    OnCreateDevice();
                }
            }
        }
    }
}
