using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;  //  for Vector2

namespace XNADemo
{
    class clsSprite
    {
        public Texture2D texture;    //  sprite texture 
        public Vector2 position;     //  sprite position on screen
        public Vector2 size;         //  sprite size in pixels

        public clsSprite (Texture2D newTexture, Vector2 newPosition, Vector2 newSize){
            texture = newTexture;
            position = newPosition;
            size = newSize;
        }
    }
}
