using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;   //   for Texture2D
using Microsoft.Xna.Framework;  //  for Vector2

namespace XNADemo
{
    class clsSprite
    {
        //  Sprite texture 
        public Texture2D texture;
        public Vector2 position;
        public Vector2 size;
        public Vector2 velocity;
        private Vector2 screenSize;


        public clsSprite (Texture2D newTexture, Vector2 newPosition, Vector2 newSize, int ScreenWidth, int ScreenHeight){
            texture = newTexture;
            position = newPosition;
            size = newSize;
            screenSize = new Vector2(ScreenWidth, ScreenHeight);
        }

        public bool Collides(clsSprite otherSprite)
        {
            // check if two sprites intersect
            if (this.position.X + this.size.X > otherSprite.position.X &&
                    this.position.X < otherSprite.position.X + otherSprite.size.X &&
                    this.position.Y + this.size.Y > otherSprite.position.Y &&
                    this.position.Y < otherSprite.position.Y + otherSprite.size.Y)
                return true;
            else
                return false;
        }

        public void Move()
        {
            //  if we´ll move out of the screen, invert velocity

            //  checking right boundary
            if(position.X + size.X + velocity.X > screenSize.X)
                velocity.X = -velocity.X;
            //  checking bottom boundary
            if (position.Y + size.Y + velocity.Y > screenSize.Y)
                velocity.Y = -velocity.Y;
            //  checking left boundary
            if (position.X + velocity.X < 0)
                velocity.X = -velocity.X;
            //  checking bottom boundary
            if (position.Y + velocity.Y < 0)
                velocity.Y = -velocity.Y;

            //  since we adjusted the velocity, just add it to the current position
            position += velocity;
        }
    }
}



