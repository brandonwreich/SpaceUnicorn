using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceUnicorn.View
{
	public class ParallaxingBackground
	{
        #region Variables

        // The image representing the parallaxing background
        private Texture2D texture;

		// An array of positions of the parallaxing background
		private Vector2[] positions;

		// The speed which the background is moving
		private int speed;

        #endregion

        public ParallaxingBackground()
		{

		}

        #region Initialize

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
		{
			// Load background
			texture = content.Load<Texture2D>(texturePath);

			// Set speed
			this.speed = speed;

			positions = new Vector2[screenWidth / texture.Width + 1];

			for (int i = 0; i < positions.Length; i++)
			{
				positions[i] = new Vector2(i * texture.Width, 0);
			}
		}

        #endregion

        #region Update

        public void Update()
		{
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i].X += speed;

				if (speed <= 0)
				{
					if (positions[i].X <= -texture.Width)
					{
						positions[i].X = texture.Width * (positions.Length - 1);
					}
				}
				else
				{
					if (positions[i].X >= texture.Width * (positions.Length - 1))
					{
						positions[i].X = -texture.Width;
					}
				}
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < positions.Length; i++)
			{
				spriteBatch.Draw(texture, positions[i], Color.White);
			}
		}

        #endregion
    }
}