using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model.Powers {
    class AddFairy {
        #region Variables

        // Animation representing the power
        private Animation _addFairyAnimation;
        public Animation AddFairyAnimation {
            get { return _addFairyAnimation; }
            set { _addFairyAnimation = value; }
        }

        // Position of the power relative to the upper left side of the screen
        public Vector2 Position;

        // State of the power
        private bool _active;
        public bool Active {
            get { return _active; }
            set { _active = value; }
        }

        // Get the width of the power
        public int Width {
            get { return AddFairyAnimation.FrameWidth; }
        }

        // Get the height of the power
        public int Height {
            get { return AddFairyAnimation.FrameHeight; }
        }

        // Determines how fast the power moves
        private float _addFairyPowerMoveSpeed;

        #endregion

        #region Initialize

        public void Initialize(Animation animation, Vector2 position) {
            _addFairyAnimation = animation;
            Position = position;
            _active = true;
            _addFairyPowerMoveSpeed = 6f;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime) {
            Position.X -= _addFairyPowerMoveSpeed;
            _addFairyAnimation.Position = Position;
            _addFairyAnimation.Update(gameTime);

            if (Position.X < -Width) {
                Active = false;
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch) {
            _addFairyAnimation.Draw(spriteBatch);
        }

        #endregion
    }
}