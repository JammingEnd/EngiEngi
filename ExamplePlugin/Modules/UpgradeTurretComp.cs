using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using RoR2;
using RoR2.Social;
using RoR2.Projectile;


namespace EngiEgni.Modules
{
   

    class UpgradeTurretComp : MonoBehaviour
    {
        private CharacterBody thisTurret;
        
       // private float thisTurretAIRange;

        public int thisTotalBuffStacks;
        public int requiredStacksTier2, requiredStacksTier3;
        int maxtstacks;

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
           
            //thisTurretAIRange = EntityStates.Engi.EngiWeapon.PlaceTurret.turretRadius;
            turretTier = 1;
            TestValues();

        }
        void StoreStuffBCimlazy()
        {
            if (thisTotalBuffStacks < 2)
            {
                thisTurret.name = "Engineer turret T1";
            }
        }
        public void AssignCB(CharacterBody turretPrefab)
        {
            if(thisTurret == null)
            {
                thisTurret = turretPrefab;

            }
        }

        public void addBuffAndUpdateInt()
        {
            if (turretTier < 2)
            {
               
                if (thisTotalBuffStacks == requiredStacksTier2 || thisTotalBuffStacks !> requiredStacksTier2 + 1)
                {
                    turretTier = 2;
                    UpgradeTurret(turretTier);
                    return;
                }
            }
           if(turretTier == 2)
            {
                if (thisTotalBuffStacks == requiredStacksTier3 || thisTotalBuffStacks !> requiredStacksTier3 + 1)
                {
                    turretTier = 3;
                    UpgradeTurret(turretTier);
                    return;
                }
            }
          



            if (turretTier == 1)
            {
                maxtstacks = requiredStacksTier2 + 1;
            }
            if(turretTier == 2)
            {
                maxtstacks = requiredStacksTier3 + 1;
            }
            else
            {
                maxtstacks = 0;
            }
            if(turretTier < 3)
            {
                thisTotalBuffStacks++;
               //thisTurret.AddTimedBuff(Buffs.enhancementBuff, 2, maxtstacks);
            }
        }
        private void UpgradeTurret(int tier)
        {
            Chat.AddMessage($"Turret Upgraded to tier { tier }");
            if(turretTier == 2)
            {
                thisTurret.baseDamage *= damageIncrease;
                thisTurret.baseAttackSpeed *= fireRIncrease;
                thisTurret.baseRegen *= regenIncrease;
                thisTurret.radius *= rangeIncrease;

                //boss damage increase
                
                thisTotalBuffStacks = 0;
            }
            if(turretTier == 3)
            {
                thisTurret.radius *= rangeIncrease;
                thisTurret.baseArmor *= armorIncrease;
                thisTurret.maxHealth *= healthIncrease;
                //projectile.projectileController.procCoefficient *= procChanceIncrease;
                //increased proc chance
                //crit damage incr
                thisTotalBuffStacks = 0;

            }
            thisTurret.RecalculateStats();
        }

    }
}
