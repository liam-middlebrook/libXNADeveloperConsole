using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GemGameDemo
{
    class Gem
    {
        #region Fields

        private Texture2D texture;

        private Vector2 position;

        private Color color;

        #endregion

        #region Properties

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        #endregion

        public Gem(Vector2 position, Texture2D texture, Color color)
        {
            this.position = position;
            this.texture = texture;
            this.color = color;
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
