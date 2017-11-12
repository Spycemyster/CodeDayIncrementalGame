#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    ///	The parent class for all entities
    ///	</summary>
    public class Entity
    {
        #region Fields
        private List<Effect> effects;

        /// <summary>
        /// The name of the entity.
        /// </summary>
        public String Name {
            get;
            set;
        }

        /// <summary>
        /// The amount of ability power.
        /// </summary>
        public float AbilityPower
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum health.
        /// </summary>
        public float MaxHealth
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum Mana.
        /// </summary>
        public float MaxMana
        {
            get;
            set;
        }

        /// <summary>
        /// Attack speed for the entity.
        /// </summary>
        public float Speed
        {
            get;
            set;
        }

        /// <summary>
        /// Current health for the entity.
        /// </summary>
        public float CurrentHealth
        {
            get;
            set;
        }

        /// <summary>
        /// Current mana pool for the entity.
        /// </summary>
        public float CurrentMana
        {
            get;
            set;
        }

        /// <summary>
        /// Health regen.
        /// </summary>
        public float HealthRegen
        {
            get;
            set;
        }

        /// <summary>
        /// Mana regen.
        /// </summary>
        public float ManaRegen {
            get;
            set;
        }

        /// <summary>
        /// The defense coefficient of the entity.
        /// </summary>
        public float Defense
        {
            get;
            set;
        }

        /// <summary>
        /// The texture of the entity.
        /// </summary>
        public Texture2D Texture
        {
            get;
            set;
        }

        /// <summary>
        /// A rectangle that the entity is drawn in.
        /// </summary>
        public Rectangle DrawRectangle
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the entity is alive or not.
        /// </summary>
        public bool isAlive
        {
            get;
            set;
        }

        /// <summary>
        /// The level of the entity.
        /// </summary>
        public int Level
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        ///	<summary>
        ///	Creates a new instance of <c>Entity</c>.
        ///	</summary>
        public Entity()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deals an amount of raw damage.
        /// </summary>
        /// <param name="amount"></param>
        public virtual void Damage(float amount)
        {
            CurrentHealth -= amount * (10 / (10 + Defense));

            if (CurrentHealth <= 0)
            {
                // DIE
                CurrentHealth = 0;
                isAlive = false;
            }
        }
        /// <summary>
        /// Loads the assets for the entity.
        /// </summary>
        /// <param name="Content"></param>
        public virtual void LoadContent(ContentManager Content)
        {

        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws the entity.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
        #endregion
    }

    /// <summary>
    /// Effects and stat changes for entities
    /// </summary>
    public class Effect
    {

    }
}
