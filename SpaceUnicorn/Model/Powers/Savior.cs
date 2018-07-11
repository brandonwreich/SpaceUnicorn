using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model.Powers
{
    class Savior
    {
        #region Variables

        // Animation representing the power
        public Animation _saviorAnimation;
        public Animation SaviorAnimation
        {
            get { return _saviorAnimation; }
            set { _saviorAnimation = value; }
        }

        // Position of the power relative to the upper left side of the screen
        public Vector2 Position;

        // State of the power
        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        // Represents the viewable boundary of the game
        private Viewport _viewport;

        // Get the width of the power
        public int Width
        {
            get { return SaviorAnimation.FrameWidth; }
        }

        // Get the height of the power
        public int Height
        {
            get { return SaviorAnimation.FrameHeight; }
        }

        // Determines how fast the power moves
        private float _saviorPowerUpMoveSpeed;

        #endregion

        public Savior()
        {

        }

        #region Initialize

        public void Initialize(Animation animation, Vector2 position)
        {
            _saviorAnimation = animation;

            Position = position;

            _active = true;

            _saviorPowerUpMoveSpeed = 6f;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            Position.X -= _saviorPowerUpMoveSpeed;

            _saviorAnimation.Position = Position;

            _saviorAnimation.Update(gameTime);

            if (Position.X < Width)
            {
                Active = false;
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            _saviorAnimation.Draw(spriteBatch);
        }

        #endregion
    }
}
