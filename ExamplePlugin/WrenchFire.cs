using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EngiEngi.MyEntityStates
{
    public class WrenchFire : BaseSkillState
    {
        public static float damageCoefficient = 16f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.65f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = WrenchFire.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();

          //  base.PlayAnimation("Gesture, Override", "WrenchFire", "WrenchFire.playbackRate", this.duration);
        }
        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                //play sound

                if (base.isAuthority)
                {
                    Ray dirRay = base.GetAimRay();

                    ProjectileManager.instance.FireProjectile(EngiEgni.Modules.WrenchProjectile.wrenchPrefab,
                    dirRay.origin,
                    Util.QuaternionSafeLookRotation(dirRay.direction),
                    base.gameObject,
                    WrenchFire.damageCoefficient * this.damageStat,
                    4000f, 
                    base.RollCrit(), 
                    DamageColorIndex.Default, 
                    null, 
                    WrenchFire.throwForce);


                }
            }
        }



        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}

