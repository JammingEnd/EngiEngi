using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace EngiEgni.Modules
{
    class UpgradeTurretComp
    {
        public float buffDegenCooldown = 2f;

        private int currentBuffsStack;

        public void addStack()
        {

        }
        
        IEnumerator CoolDownUpdater()
        {
            yield return new WaitForSeconds(buffDegenCooldown);
            currentBuffsStack--;

        }
        
    }
}
