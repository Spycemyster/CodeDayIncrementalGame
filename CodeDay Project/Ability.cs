#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

//-----------------------------------------------------------------
//	Author:				Spencer Chang, Ryan Niu
//	Date:				November 11, 2017
//	Notes:				
//-----------------------------------------------------------------

namespace CodeDay_Project
{
    ///	<summary>
    ///	
    ///	</summary>
    public class Ability
    {
        #region Fields
        /// <summary>
        /// The amount of raw damage the ability inflicts (if any).
        /// </summary>
        public float Damage
        {
            get { return holder.AbilityPower * DamageScaling; }
        }

        public float Duration
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of time needed to wait for the ability to be ready
        /// to use again.
        /// </summary>
        public float Cooldown
        {
            get;
            set;
        }

        /// <summary>
        /// Timer for the counting down cooldowns.
        /// </summary>
        public float Timer
        {
            get;
            set;
        }

        /// <summary>
        /// The texture of the icon.
        /// </summary>
        public Texture2D Texture
        {
            get;
            set;
        }

        /// <summary>
        /// The cost of this ability
        /// </summary>
        public readonly float ManaCost;

        /// <summary>
        /// The entity that holds this ability.
        /// </summary>
        public readonly Entity holder;

        /// <summary>
        /// The scaling of the ability.
        /// </summary>
        public readonly float DamageScaling;
        #endregion

        #region Constructor
        ///	<summary>
        ///	Creates a new instance of <c>Ability</c>.
        ///	</summary>
        public Ability(Entity caster, float cost, float scalings)
        {
            holder = caster;
            ManaCost = cost;
            DamageScaling = scalings;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the instance of the ability.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (Timer <= Cooldown * 1000f)
            {
                Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Inflicts the ability on an entity.
        /// </summary>
        /// <param name="e"></param>
        public void InflictOn(Entity e)
        {
            Timer = 0f;
        }
        #endregion
    }
}
