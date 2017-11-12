#region Using Statements
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

namespace CodeDay_Project {
    ///	<summary>
    ///	An enemy character.
    ///	</summary>
    public class Enemy : Entity {
        #region Fields
        
        private const int SCALE = 3;
        private int offset;
        private float animationTimer, attackTimer;
        public bool isAttacking;
        public bool hasAttacked;
        #endregion

        #region Constructor
        ///	<summary>
        ///	Creates a new instance of <c>Enemy</c>.
        ///	</summary>
        public Enemy(float Speed) {
            this.Speed = Speed;
            isAttacking = false;
            animationTimer = attackTimer = 0f;
            attackTimer = Speed / 2;
            hasAttacked = false;
        }
        #endregion

        #region Methods
        public void Attack() {
            hasAttacked = true;
        }

        /// <summary>
        /// Updates the enemy.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            if (isAttacking) {
                attackTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                animationTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                offset = Math.Max((int)(32 * Math.Cos(animationTimer * (2 * Math.PI / Speed))) - 16,0);
                if (attackTimer >= Speed) {
                    Attack();
                    attackTimer = 0f;
                }
            }
            else {
                offset = 0;
                animationTimer = 0;
            }

            if (InputManager.Instance.KeyPressed(Keys.P))
                isAttacking = !isAttacking;
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the enemy to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, new Rectangle(DrawRectangle.X, DrawRectangle.Y + offset, DrawRectangle.Width, DrawRectangle.Height), Color.White);
        }
        #endregion
    }
}
