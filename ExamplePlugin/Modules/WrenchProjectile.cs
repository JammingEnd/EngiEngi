using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace EngiEgni.Modules
{
    internal static class WrenchProjectile
    {
        internal static GameObject wrenchPrefab;



        internal static void CreateProjectile()
        {

        }

        private static void CreateWrench()
        {
            wrenchPrefab = ClonePrefab("CommandoGrenadeProjectile", "EngineerWrench"); //add prefab

            ProjectileSingleTargetImpact wrenchImpact = wrenchPrefab.GetComponent<ProjectileSingleTargetImpact>();
            StartImpactEvent(wrenchImpact);
        }
        private static void AddBATComponentOnAddDeployableHook(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot)
        {
            orig(self, deployable, slot);

            if (slot == DeployableSlot.EngiTurret)
            {
               // var badAssTurret = deployable.gameObject.AddComponent<MyCustomTurret>();
            }
        }
        private static void StartImpactEvent(ProjectileSingleTargetImpact projectileImpactEvent)
        {
            float projDamage = 15;
           
            ProjectileImpactInfo whatIHit = new ProjectileImpactInfo();
            if (whatIHit.collider.gameObject.GetComponent<UpgradeTurretComp>() != null)
            {
               
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
        private static GameObject ClonePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        } 

    }
}
