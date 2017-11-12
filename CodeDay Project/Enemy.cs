#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
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
        private float attackTimer;
        private bool isAttacked;
        public bool hasAttacked;
        private float damageTimer;
        private Player player;
        #endregion

        #region Constructor
        ///	<summary>
        ///	Creates a new instance of <c>Enemy</c>.
        ///	</summary>
        public Enemy(float Speed, Player player) {
            this.player = player;
            this.Speed = Speed;
            attackTimer = -500f;
            hasAttacked = false;
            isAlive = true;
            isAttacked = false;
        }
        #endregion

        #region Methods
        public void Attack() {
            player.Damage(AbilityPower);
            hasAttacked = true;
        }

        /// <summary>
        /// Updates the enemy.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) {
            attackTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            offset = 2 * -Math.Max((int)(32 * Math.Sin(attackTimer * (2 * Math.PI / Speed))) - 16, 0);
            if (!hasAttacked && offset < -16) Attack();
            if (attackTimer >= Speed) {
                attackTimer = 0f;
                hasAttacked = false;
            }

            if (isAttacked) {
                damageTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (damageTimer >= 100f) {
                    isAttacked = false;
                    damageTimer = 0f;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the enemy to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch) {
            base.Draw(spriteBatch);
            Color c = Color.White;
            if (isAttacked)
                c = Color.Red;
            spriteBatch.Draw(Texture, new Rectangle(DrawRectangle.X + offset, DrawRectangle.Y, DrawRectangle.Width, DrawRectangle.Height), c);
        }

        /// <summary>
        /// Damages the enemy.
        /// </summary>
        /// <param name="amount"></param>
        public override void Damage(float amount) {
            base.Damage(amount);
            isAttacked = true;
        }

        public void collision(List<Projectile> projectiles) {
            for (int i = 0; i < projectiles.Count; i++) {
                if (projectiles[i].DrawRectangle.Intersects(DrawRectangle)) {
                    projectiles.RemoveAt(i--);
                    Damage(0);
                }
            }
        }
        #endregion
    }
}
