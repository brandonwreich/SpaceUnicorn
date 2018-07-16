using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceUnicorn.View;

namespace SpaceUnicorn.Model
{
    class Fairy
    {
        #region Variables

        private Animation _fairyAnimation;
        public Animation FairyAnimation
        {
            get { return _fairyAnimation; }
            set { _fairyAnimation = value; }
        }

        public Vector2 Position;

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        private int _health;
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        private int _damage;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        public int Width
        {
            get { return FairyAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return FairyAnimation.FrameHeight; }
        }

        #endregion

        public Fairy()
        {

        }

        #region Initialize

        public void Initialize(Animation animation, Vector2 position)
        {
            _fairyAnimation = animation;
            Position = position;
            Active = true;
            Health = 10;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            _fairyAnimation.Position = Position;
            _fairyAnimation.Update(gameTime);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            _fairyAnimation.Draw(spriteBatch);
        }

        #endregion
    }
}
