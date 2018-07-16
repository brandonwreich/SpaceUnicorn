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
        private Texture2D _texture;

		// An array of positions of the parallaxing background
		private Vector2[] _positions;

		// The speed which the background is moving
		private int _speed;

        #endregion

        #region Initialize

        public void Initialize(ContentManager content, String texturePath, int screenWidth, int speed)
		{
			// Load background
			_texture = content.Load<Texture2D>(texturePath);

			// Set speed
			_speed = speed;

			_positions = new Vector2[screenWidth / _texture.Width + 1];

			for (int i = 0; i < _positions.Length; i++)
			{
				_positions[i] = new Vector2(i * _texture.Width, 0);
			}
		}

        #endregion

        #region Update

        public void Update()
		{
			for (int i = 0; i < _positions.Length; i++)
			{
				_positions[i].X += _speed;

				if (_speed <= 0)
				{
					if (_positions[i].X <= -_texture.Width)
					{
						_positions[i].X = _texture.Width * (_positions.Length - 1);
					}
				}
				else
				{
					if (_positions[i].X >= _texture.Width * (_positions.Length - 1))
					{
						_positions[i].X = -_texture.Width;
					}
				}
			}
		}

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < _positions.Length; i++)
			{
				spriteBatch.Draw(_texture, _positions[i], Color.White);
			}
		}

        #endregion
    }
}