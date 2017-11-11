#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

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
        public int Level
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        ///	<summary>
        ///	Creates a new instance of <c>Player</c>.
        ///	</summary>
        public Player()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Levels up the character's stats.
        /// </summary>
        public void LevelUp()
        {

        }

        /// <summary>
        /// Draws the player to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
        }
        #endregion
    }
}
