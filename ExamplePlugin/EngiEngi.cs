using System;
using BepInEx;
using EntityStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace EngiEngi
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(
        "com.JammingEnd.EngiEngi",
        "EngiEngi",
        "1.0.0")]
    [R2APISubmoduleDependency(nameof(LoadoutAPI), nameof(SurvivorAPI), nameof(LanguageAPI))]
    public class AdditionalSkill : BaseUnityPlugin
    {
        public void Awake()
        {
            // myCharacter should either be
            // Resources.Load<GameObject>("prefabs/characterbodies/BanditBody");
            // or BodyCatalog.FindBodyIndex("BanditBody");
            var myCharacter = Resources.Load<GameObject>("prefabs/characterbodies/Engi");

            // If you're confused about the language tokens, they're the proper way to add any strings used by the game.
            // We use LanguageAPI for that
            LanguageAPI.Add("MYBANDIT_DESCRIPTION", "The description of my survivor" + Environment.NewLine);

            var mySurvivorDef = new SurvivorDef
            {
                //We're finding the body prefab here,
                bodyPrefab = myCharacter,
                //Description
                descriptionToken = "MYBANDIT_DESCRIPTION",
                //Display 
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/BanditDisplay"),
                //Color on select screen
                primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                //Unlockable name
                unlockableName = "",
            };
            SurvivorAPI.AddSurvivor(mySurvivorDef);

            LanguageAPI.Add("CHARACTERNAME_SKILLSLOT_SKILLNAME_NAME", "The name of this skill");
            LanguageAPI.Add("CHARACTERNAME_SKILLSLOT_SKILLNAME_DESCRIPTION", "The description of this skill.");

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
            mySkillDef.icon = Resources.Load<Sprite>("NotAnActualPath");
            mySkillDef.skillDescriptionToken = "fire wrenches that deal increased damage to shields";
            mySkillDef.skillName = "Wrench";
            mySkillDef.skillNameToken = "Wrench";

            LoadoutAPI.AddSkillDef(mySkillDef);
            //This adds our skilldef. If you don't do this, the skill will not work.

            var skillLocator = myCharacter.GetComponent<SkillLocator>();

            //Note; if your character does not originally have a skill family for this, use the following:
            //skillLocator.special = gameObject.AddComponent<GenericSkill>();
            //var newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            //LoadoutAPI.AddSkillFamily(newFamily);
            //skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            //var specialSkillFamily = skillLocator.special.skillFamily;


            //Note; you can change component.primary to component.secondary , component.utility and component.special
            var skillFamily = skillLocator.primary.skillFamily;

            //If this is an alternate skill, use this code.
            // Here, we add our skill as a variant to the exisiting Skill Family.
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)

            };

            //Note; if your character does not originally have a skill family for this, use the following:
            //skillFamily.variants = new SkillFamily.Variant[1]; // substitute 1 for the number of skill variants you are implementing

            //If this is the default/first skill, copy this code and remove the //,
            //skillFamily.variants[0] = new SkillFamily.Variant
            //{
            //    skillDef = mySkillDef,
            //    unlockableName = "",
            //    viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            //};

            On.RoR2.CharacterMaster.AddDeployable += AddBATComponentOnAddDeployableHook;
            

            
        }

      

        private static void AddBATComponentOnAddDeployableHook(On.RoR2.CharacterMaster.orig_AddDeployable orig, CharacterMaster self, Deployable deployable, DeployableSlot slot)
        {
            orig(self, deployable, slot);

            if (slot == DeployableSlot.EngiTurret)
            {
                var badAssTurret = deployable.gameObject.AddComponent(typeof(EngiEgni.Modules.UpgradeTurretComp));
            }
        }
    }
}
