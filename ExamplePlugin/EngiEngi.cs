using BepInEx;
using EntityStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using BepInEx.Logging;
using System;
using UnityEngine;

namespace EngiEngi
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(
        "com.JammingEnd.EngiEngi",
        "EngiestEngineer",
        "1.0.0")]
    [R2APISubmoduleDependency(nameof(LoadoutAPI), nameof(SurvivorAPI), nameof(LanguageAPI))]
    public class AdditionalSkill : BaseUnityPlugin
    {
        public static ManualLogSource logger;

        public void Awake()
        {
            logger = base.Logger;

            // myCharacter should either be
            // Resources.Load<GameObject>("prefabs/characterbodies/BanditBody");
            // or BodyCatalog.FindBodyIndex("BanditBody");
            var myCharacter = Resources.Load<GameObject>("prefabs/characterbodies/EngiBody");
            var skillLocator = myCharacter.GetComponent<SkillLocator>();
            // If you're confused about the language tokens, they're the proper way to add any strings used by the game.
            // We use LanguageAPI for that
           /* LanguageAPI.Add("MYENGI_DESCRIPTION", "this is engineer, i solve problems" + Environment.NewLine);

            var mySurvivorDef = new SurvivorDef
            {
                //We're finding the body prefab here,
                bodyPrefab = myCharacter,
                //Description
                descriptionToken = "MYENGI_DESCRIPTION",
                //Display
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/EngiDisplay"),
                //Color on select screen
                primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                //Unlockable name
                unlockableName = "Engineer",
            };
            SurvivorAPI.AddSurvivor(mySurvivorDef);
           */
            LanguageAPI.Add("CHARACTERNAME_SKILLSLOT_SKILLNAME_NAME", "Wrench");
            LanguageAPI.Add("CHARACTERNAME_SKILLSLOT_SKILLNAME_DESCRIPTION", "fires wrenches that deal increased damage to shield and are able to upgrade your turret.");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(EngiEngi.MyEntityStates.WrenchFire));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            // mySkillDef.is = true;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            //  mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            // mySkillDef.icon = Resources.Load<Sprite>("NotAnActualPath");
            mySkillDef.skillDescriptionToken = "fire wrenches that deal increased damage to shields and upgrades your turrets";
            mySkillDef.skillName = "WrenchSkill";
            mySkillDef.skillNameToken = "Wrench";

            LoadoutAPI.AddSkillDef(mySkillDef);

            //If this is an alternate skill, use this code.
            //Note; you can change component.primary to component.secondary , component.utility and component.special
            // Here, we add our skill as a variant to the exisiting Skill Family.

            var skillFamily1 = skillLocator.primary.skillFamily;
            Array.Resize(ref skillFamily1.variants, skillFamily1.variants.Length + 1);
            skillFamily1.variants[skillFamily1.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "Wrench!",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
            //This adds our skilldef. If you don't do this, the skill will not work.

            //Note; if your character does not originally have a skill family for this, use the following:
            //skillLocator.special = gameObject.AddComponent<GenericSkill>();
            //var newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            //LoadoutAPI.AddSkillFamily(newFamily);
            //skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            //var specialSkillFamily = skillLocator.special.skillFamily;

            //Note; if your character does not originally have a skill family for this, use the following:
            //skillFamily.variants = new SkillFamily.Variant[1]; // substitute 1 for the number of skill variants you are implementing

            //If this is the default/first skill, copy this code and remove the //,
            //skillFamily.variants[0] = new SkillFamily.Variant
            //{
            //    skillDef = mySkillDef,
            //    unlockableName = "",
            //    viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            //};

           
            EngiEgni.Modules.WrenchProjectile.CreateProjectile(logger);
        }
        private void Start()
        {
            On.RoR2.CharacterMaster.AddDeployable += AddBATComponentOnAddDeployableHook;
            
        }
        private static void AddBATComponentOnAddDeployableHook(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot)
        {
            logger.LogMessage("Hook called, attempting AddComponent on engineer turrert");
            orig(self, deployable, slot);

            if (slot == DeployableSlot.EngiTurret)
            {
               var Engiturret = deployable.gameObject.AddComponent(typeof(EngiEgni.Modules.UpgradeTurretComp));
                logger.LogMessage("Hook complete, engineer turret should have the component");
                logger.LogMessage($"{Engiturret.GetComponent<EngiEgni.Modules.UpgradeTurretComp>().name}");
            }
        }
    }
}