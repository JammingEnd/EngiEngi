using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace EngiEgni.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef enhancementBuff;

        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static void RegisterBuffs()
        {
            enhancementBuff = AddNewBuff("EngiEnhancementBuff", Resources.Load<Sprite>("Textures/BuffIcons/texBuffGenericShield"), Color.magenta, true, false);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;
           
            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}
