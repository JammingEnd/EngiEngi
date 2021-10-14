using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using RoR2;
using RoR2.Projectile;


namespace EngiEgni.Modules
{
   

    class UpgradeTurretComp : MonoBehaviour
    {
        private CharacterBody thisTurret;
        
       // private float thisTurretAIRange;

        public int thisTotalBuffStacks;
        public int requiredStacksTier2, requiredStacksTier3;

        private int turretTier;


        [Header("UpgradeStats (multiplier)")]
        private float rangeIncrease, damageIncrease, fireRIncrease, healthIncrease, regenIncrease, critDamageIncrease, procChanceIncrease, armorIncrease, bossDamageIncrease;


        void TestValues()
        {
            requiredStacksTier2 = 10;
            requiredStacksTier3 = 30;

            rangeIncrease = 2;
            damageIncrease = 2;
            fireRIncrease = 2;
            healthIncrease = 2;
            regenIncrease = 2;
            critDamageIncrease = 2;
            procChanceIncrease = 2;
            armorIncrease = 2;
            bossDamageIncrease = 2;
        }

        private void Awake()
        {
            thisTurret = this.gameObject.GetComponent<CharacterBody>();
            //thisTurretAIRange = EntityStates.Engi.EngiWeapon.PlaceTurret.turretRadius;
            turretTier = 1;
            TestValues();

        }
        private void Update()
        {
            if(this.thisTotalBuffStacks >= requiredStacksTier2)
            {
                UpgradeTurret(turretTier);
            }
        }
        public void addBuffAndUpdateInt()
        {
            thisTotalBuffStacks++;
            thisTurret.AddBuff(Buffs.enhancementBuff);
        }
        private void UpgradeTurret(int tier)
        {
            if(turretTier == 2)
            {
                thisTurret.baseDamage *= damageIncrease;
                thisTurret.baseAttackSpeed *= fireRIncrease;
                thisTurret.baseRegen *= regenIncrease;
                thisTurret.radius *= rangeIncrease;
                
                //boss damage increase
            }
            if(turretTier == 3)
            {
                thisTurret.radius *= rangeIncrease;
                thisTurret.baseArmor *= armorIncrease;
                thisTurret.maxHealth *= healthIncrease;
                //projectile.projectileController.procCoefficient *= procChanceIncrease;
                //increased proc chance
                //crit damage incr

            }
        }

    }
}
