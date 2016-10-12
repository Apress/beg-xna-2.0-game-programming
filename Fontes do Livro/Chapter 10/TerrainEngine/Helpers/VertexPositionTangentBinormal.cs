using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrainEngine.Helpers
{
    public struct VertexPositionNormalTangentBinormal
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;
        public Vector3 Tanget;
        public Vector3 Binormal;

        #region Properties
        public static int SizeInBytes
        {
            get
            {
                return (3 + 3 + 2 + 3 + 3) * sizeof(float);
            }
        }
        #endregion

        public static VertexElement[] VertexElements = new VertexElement[] {
            new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default,
                VertexElementUsage.Position, 0),
            new VertexElement(0, 12, VertexElementFormat.Vector3, VertexElementMethod.Default,
                VertexElementUsage.Normal, 0),
            new VertexElement(0, 24, VertexElementFormat.Vector2, VertexElementMethod.Default,
                VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(0, 32, VertexElementFormat.Vector3, VertexElementMethod.Default,
                VertexElementUsage.Tangent, 0),
            new VertexElement(0, 44, VertexElementFormat.Vector3, VertexElementMethod.Default,
                VertexElementUsage.Binormal, 0)
        };
    }
}
