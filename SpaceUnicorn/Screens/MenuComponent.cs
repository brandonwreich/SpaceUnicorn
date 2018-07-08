using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceUnicorn
{
	public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
	{
        #region Variables

        private string[] _menuItems;
		private int _selectedIndex;

		private Color _normal = Color.LightBlue;
		private Color _hilite = Color.Yellow;

		private KeyboardState _keyboardState;
		private KeyboardState _previousKeyboardState;

		private SpriteBatch _spriteBatch;
		private SpriteFont _spriteFont;

		private Vector2 _position;

		private float _width = 0f;
		private float _height = 0f;

        public int SelectedIndex
		{
			get { return _selectedIndex; }
			set
			{
				_selectedIndex = value;

				if (_selectedIndex < 0)
				{
					_selectedIndex = 0;
				}
				if (_selectedIndex >= _menuItems.Length)
				{
					_selectedIndex = _menuItems.Length - 1;
				}
			}
		}

        #endregion

        public MenuComponent(Game game, SpriteBatch spriteBatch, SpriteFont spriteFont, string[] menuItems) : base(game)
		{
			this._spriteBatch = spriteBatch;
			this._spriteFont = spriteFont;
			this._menuItems = menuItems;

			MeasureMenu();
		}

		public override void Initialize()
		{
			base.Initialize();
		}

        #region Update

        public override void Update(GameTime gameTime)
		{
			_keyboardState = Keyboard.GetState();

			if (CheckKey(Keys.Down))
			{
				_selectedIndex++;

				if (_selectedIndex == _menuItems.Length)
				{
					_selectedIndex = 0;
				}
			}

			if (CheckKey(Keys.Up))
			{
				_selectedIndex--;

				if (_selectedIndex < 0)
				{
					_selectedIndex = _menuItems.Length - 1;
				}
			}

			base.Update(gameTime);

			_previousKeyboardState = _keyboardState;
		}

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			Vector2 location = _position;
			Color tint;

			for (int i = 0; i < _menuItems.Length; i++)
			{
				if (i == _selectedIndex)
				{
					tint = _hilite;
				}
				else
				{
					tint = _normal;
					_spriteBatch.DrawString(_spriteFont, _menuItems[i], location, tint);

					location.Y += _spriteFont.LineSpacing + 5;
				}
			}
		}

        #endregion

        #region Helper Methods

	    private bool CheckKey(Keys theKey)
	    {
	        return _keyboardState.IsKeyUp(theKey) && _previousKeyboardState.IsKeyDown(theKey);
	    }

	    private void MeasureMenu()
	    {
	        _height = 0;
	        _width = 0;

	        foreach (string item in _menuItems)
	        {
	            Vector2 size = _spriteFont.MeasureString(item);

	            if (size.X > _width)
	            {
	                _width = size.X;
	            }

	            _height += _spriteFont.LineSpacing + 5;
	        }

	        _position = new Vector2((Game.Window.ClientBounds.Width - _width) / 2, (Game.Window.ClientBounds.Height - _height) / 2);
	    }

        #endregion
    }
}