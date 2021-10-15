using R2API;
using RoR2;

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

            On.RoR2.Projectile.ProjectileImpactEventCaller.OnProjectileImpact += ProjectileImpactEventCaller_OnProjectileImpact;
        }

        private static void ProjectileImpactEventCaller_OnProjectileImpact(On.RoR2.Projectile.ProjectileImpactEventCaller.orig_OnProjectileImpact orig, ProjectileImpactEventCaller self, ProjectileImpactInfo whatIHit)
        {
            orig(self, whatIHit);
            ProjectileSingleTargetImpact projectileImpactEvent = self.GetComponent<ProjectileSingleTargetImpact>();

            float projDamage = 15;

            if (whatIHit.collider.gameObject.GetComponent<UpgradeTurretComp>() != null)
            {
                UpgradeTurretComp addStack = whatIHit.collider.gameObject.GetComponent<UpgradeTurretComp>();
                addStack.addBuffAndUpdateInt();
            }

            if (whatIHit.collider.GetComponent<HealthComponent>().shield > 0)
            {
                projDamage = 0.75f;
            }
            else
            {
                projDamage = 0.5f;
            }

            projectileImpactEvent.projectileDamage.damage = projDamage;
            projectileImpactEvent.projectileDamage.crit = false;
            projectileImpactEvent.projectileDamage.force = 2;
            projectileImpactEvent.projectileDamage.damageType = DamageType.Generic;
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