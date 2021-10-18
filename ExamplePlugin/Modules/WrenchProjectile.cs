using R2API;
using RoR2;
using RoR2.Projectile;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.Networking;

namespace EngiEgni.Modules
{
  

    internal static class WrenchProjectile
    {
        internal static GameObject wrenchPrefab;
        public static ManualLogSource logger;
      internal static void CreateProjectile(ManualLogSource log)
        {
            logger = log;
            CreateWrench();
            
        }

        private static void CreateWrench()
        {
          
            wrenchPrefab = ClonePrefab("CommandoGrenadeProjectile", "EngineerWrench"); //add prefab
            Object.Destroy(wrenchPrefab.GetComponent<ProjectileExplosion>());
            wrenchPrefab.AddComponent<ProjectileImpactEventCaller>();
            wrenchPrefab.AddComponent<ProjectileSingleTargetImpact>();

            logger.LogMessage("wrench created");
            AddListener();

        }
        private static void AddListener()
        {
            if (NetworkServer.active)
            {
                var impactEventCaller = wrenchPrefab.GetComponent<ProjectileImpactEventCaller>();
                if (impactEventCaller)
                {
                    impactEventCaller.impactEvent.AddListener(OnImpactListener);
                    logger.LogMessage("Hit something");
                }
            }
        }
        private static void OnImpactListener(ProjectileImpactInfo whatIHit)
        {
          
            var projectileDamage = wrenchPrefab.GetComponent<ProjectileDamage>();
            logger.LogMessage($"{whatIHit.collider.name}");
            float projDamage = 2;

            if (whatIHit.collider.gameObject.GetComponent<UpgradeTurretComp>() != null)
            {
                UpgradeTurretComp addStack = whatIHit.collider.gameObject.GetComponent<UpgradeTurretComp>();

                addStack.addBuffAndUpdateInt();
                logger.LogMessage("You hit the turret, it has the component");
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

        private static GameObject ClonePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}