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
            wrenchPrefab = null; //add prefab

            ProjectileImpactEvent wrenchImpact = wrenchPrefab.GetComponent<ProjectileImpactEvent>();
            
        }
        private static void StartImpactEvent(ProjectileSingleTargetImpact projectileImpactEvent)
        {
            projectileImpactEvent.projectileDamage = 


            if (projectileImpactEvent.GetComponent<HealthComponent>().shield > 0)
            {
                
            }
           


        }
    }
}
