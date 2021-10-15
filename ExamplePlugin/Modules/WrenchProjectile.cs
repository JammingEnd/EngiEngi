using R2API;
using RoR2;
using UnityEngine.Networking;
using RoR2.Projectile;
using UnityEngine;

namespace EngiEgni.Modules
{
    internal static class WrenchProjectile
    {
        internal static GameObject wrenchPrefab;

        internal static void CreateProjectile()
        {
            CreateWrench();
        }

        private static void CreateWrench()
        {
            wrenchPrefab = ClonePrefab("CommandoGrenadeProjectile", "EngineerWrench"); //add prefab
            wrenchPrefab.AddComponent<ProjectileSingleTargetImpact>();
            if (NetworkServer.active)
            {
                var impactEventCaller = wrenchPrefab.GetComponent<ProjectileImpactEventCaller>();
                if ((bool)impactEventCaller)
                {
                    impactEventCaller.impactEvent.AddListener(OnImpactListener);
                }
            }
        }

        private static void OnImpactListener(ProjectileImpactInfo whatIHit)
        {

            var projectileDamage = wrenchPrefab.GetComponent<ProjectileDamage>();

            float projDamage = 15;
          
            if (whatIHit.collider.gameObject.GetComponent<UpgradeTurretComp>() != null)
            {
                UpgradeTurretComp addStack = whatIHit.collider.gameObject.GetComponent<UpgradeTurretComp>();
                addStack.addBuffAndUpdateInt();
            }
            if (whatIHit.collider.GetComponent<HealthComponent>())
            {
                if (whatIHit.collider.GetComponent<HealthComponent>().shield > 0)
                {
                    projDamage = 0.75f;
                }
                else
                {
                    projDamage = 0.5f;
                }
            }

            projectileDamage.damage *= projDamage;
           
            projectileDamage.damageType = DamageType.Generic;


        }

        private static void AddBATComponentOnAddDeployableHook(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot)
        {
            orig(self, deployable, slot);

            if (slot == DeployableSlot.EngiTurret)
            {
                var badAssTurret = deployable.gameObject.AddComponent<UpgradeTurretComp>();
            }
        }

        private static void StartImpactEvent()
        {
        }

        private static GameObject ClonePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}