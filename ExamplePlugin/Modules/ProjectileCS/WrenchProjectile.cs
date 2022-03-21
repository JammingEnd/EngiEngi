using BepInEx.Logging;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

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
            wrenchPrefab.AddComponent<ProjectileSingleTargetImpact>();
            On.RoR2.Projectile.ProjectileSingleTargetImpact.OnProjectileImpact += ProjectileSingleTargetImpact_OnProjectileImpact;
            logger.LogMessage("wrench created");
        }

        private static void ProjectileSingleTargetImpact_OnProjectileImpact(On.RoR2.Projectile.ProjectileSingleTargetImpact.orig_OnProjectileImpact orig, ProjectileSingleTargetImpact self, ProjectileImpactInfo whatIHit)
        {
            orig(self, whatIHit);
            if (self.gameObject.name.Contains("EngineerWrench") && whatIHit.collider)
            {
                var projectileDamage = wrenchPrefab.GetComponent<ProjectileDamage>();

                float projDamage = 2;
                var hurtbox = whatIHit.collider.gameObject.GetComponent<HurtBox>();
                if (hurtbox && hurtbox.healthComponent && hurtbox.healthComponent.body)
                {
                    var isTurret = hurtbox.healthComponent.body.master;
                    logger.LogMessage($"you hit {whatIHit.collider.gameObject.name}");
                    if (isTurret.GetComponent<UpgradeTurretComp>() != null)
                    {
                        UpgradeTurretComp addStack = isTurret.GetComponent<UpgradeTurretComp>();
                        addStack.AssignCB(hurtbox.healthComponent.body);
                        addStack.addBuffAndUpdateInt();
                        logger.LogMessage("You hit the turret, it has the component");
                        return;

                    }
                    else
                    {
                        logger.LogMessage($"you did not hit a turret, noob");
                        return;
                    }

                    logger.LogMessage($"you did damage");
                    if (hurtbox.healthComponent.shield > 0)
                    {
                        projDamage = 0.75f;
                    }
                    else
                    {
                        projDamage = 0.5f;
                    }
                }
                projectileDamage.damage *= projDamage / 10;

                projectileDamage.damageType = DamageType.Generic;
            }
        }

        private static GameObject ClonePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}