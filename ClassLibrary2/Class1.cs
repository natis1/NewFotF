using System.Reflection;
using GlobalEnums;
using Modding;

namespace ExampleMod1
{
    /// <summary>
    /// The main mod class
    /// </summary>
    /// <remarks>This configuration has settings that are save specific</remarks>
    public class ExampleMod1 : Mod
    {
        private int _hitCounter;

        private int _tempNailDamage;

        /// <summary>
        /// Represents this Mod's instance.
        /// </summary>
        internal static ExampleMod1 Instance;

        /// <summary>
        /// Fetches the Mod Version From AssemblyInfo.AssemblyVersion
        /// </summary>
        /// <returns>Mod's Version</returns>
        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Called after the class has been constructed.
        /// </summary>
        public override void Initialize()
        {
            //Assign the Instance to the instantiated mod.
            Instance = this;

            Log("Initializing");

            //Here we are hooking into the AttackHook so we can modify the damage for the attack.
            ModHooks.Instance.AttackHook += OnAttack;

            //Here want to hook into the AfterAttackHook to do something at the end of the attack animation.
            ModHooks.Instance.AfterAttackHook += OnAfterAttack;
            Log("Initialized");
        }


        /// <summary>
        /// Calculates Crits on attack
        /// </summary>
        /// <remarks>
        /// This checks if we have FoTF. If we do Damage is calculated based on lost masks. otherwise we revert back to normal
        /// </remarks>
        /// <param name="dir"></param>
        public void OnAttack(AttackDirection dir)
        {
            LogDebug("Attacking");
            if (PlayerData.instance.equippedCharm_6)
            {
                LogDebug("Critical hit!");

                _tempNailDamage = PlayerData.instance.nailDamage; //Store the current nail damage.

                LogDebug("Set _tempNailDamage to " + _tempNailDamage);

                PlayerData.instance.nailDamage *= 1 + ((PlayerData.instance.maxHealth - PlayerData.instance.health) / PlayerData.instance.maxHealth); //Double the nail damage
                

                return;
            }
        }

        /// <summary>
        /// Reverts damage
        /// </summary>
        /// <remarks>After the attack is over, we need to reset the nail damage back to what it was.</remarks>
        /// <param name="dir"></param>
        private void OnAfterAttack(AttackDirection dir)
        {
            LogDebug("Attacked!");
            PlayerData.instance.nailDamage = _tempNailDamage; //Attacking is done, we need to set the nail damage back to what it was before we crit.
        }
    }

}