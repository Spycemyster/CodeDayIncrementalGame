#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

#endregion

//-----------------------------------------------------------------
//	Author:				Spencer Chang, Ryan Niu
//	Date:				November 11, 2017
//	Notes:				
//-----------------------------------------------------------------

namespace CodeDay_Project
{
    ///	<summary>
    ///	A projectile.
    ///	</summary>
    public class Projectile
    {
        #region Fields
        /// <summary>
        /// The velocity for the projectile.
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// The position of the projectile.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The texture of the projectile.
        /// </summary>
        public Texture2D Texture
        {
            get;
            set;
        }

        /// <summary>
        /// The dimension of the projectile.
        /// </summary>
        public Point Dimensions
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of damage on collision.
        /// </summary>
        public float CollisionalDamage
        {
            get;
            set;
        }

        /// <summary>
        /// The rectangle that the projectile is drawn in.
        /// </summary>
        public Rectangle DrawRectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Dimensions.X, Dimensions.Y); }
        }
        #endregion

        #region Constructor
        ///	<summary>
        ///	Creates a new instance of <c>Projectile</c>.
        ///	</summary>
        public Projectile()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the projectile.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            Position += Velocity;
        }

        /// <summary>
        /// Draws the projectile to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DrawRectangle, Color.White);

        }
        #endregion
    }
}
