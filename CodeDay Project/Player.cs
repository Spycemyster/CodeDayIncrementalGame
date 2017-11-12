﻿#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

//-----------------------------------------------------------------
//	Author:				Spencer Chang, Ryan Niu
//	Date:				November 11, 2017
//	Notes:				
//-----------------------------------------------------------------

namespace CodeDay_Project
{
    ///	<summary>
    ///	The character that the player of the game controls.
    ///	</summary>
    public class Player : Entity
    {
        #region Fields
        /// <summary>
        /// The texture of the staff.
        /// </summary>
        public Texture2D StaffTexture
        {
            get;
            set;
        }
        /// <summary>
        /// The level of the player
        /// </summary>
        public int Level
        {
            get;
            set;
        }
        private const int SCALE = 3;
        private float rotation;
        private float animationTimer, attackTimer;
        public bool isAttacking;
        public bool hasAttacked;
        #endregion

        #region Constructor
        ///	<summary>
        ///	Creates a new instance of <c>Player</c>.
        ///	</summary>
        public Player(float Speed)
        {
            this.Speed = Speed;
            isAttacking = false;
            animationTimer = attackTimer = 0f;
            attackTimer = Speed / 2;
            hasAttacked = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Levels up the character's stats.
        /// </summary>
        public void LevelUp()
        {

        }

        public void Attack()
        {
            hasAttacked = true;
        }

        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (isAttacking)
            {
                attackTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                animationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                rotation = (float)(-(Math.PI / 4) * Math.Cos(animationTimer * (2 * Math.PI / Speed)) + Math.PI / 6);
                if (attackTimer >= Speed)
                {
                    Attack();
                    attackTimer = 0f;
                }
            }
            else
            {
                rotation = 0;
                animationTimer = 0;
            }

            if (InputManager.Instance.KeyPressed(Keys.P))
                isAttacking = !isAttacking;
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the player to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Vector2 staffOrigin = new Vector2(StaffTexture.Width / 2, StaffTexture.Height / 2);
            spriteBatch.Draw(Texture, DrawRectangle, Color.White);
            spriteBatch.Draw(StaffTexture, new Vector2(DrawRectangle.X + DrawRectangle.Width + StaffTexture.Width + 32,
                DrawRectangle.Y + DrawRectangle.Height / 2), null, Color.White, rotation, staffOrigin, SCALE, SpriteEffects.None, 0f);
        }
        #endregion
    }
}
