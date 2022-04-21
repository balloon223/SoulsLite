using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerManager playerManager;
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerStats playerStats;
        public string lastAttack;


        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
            playerStats = GetComponent<PlayerStats>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (!playerManager.isInDialogue)
            {
                if (playerStats.currentStamina <= 0)
                    return;

                if (inputHandler.comboFlag)
                {
                    animatorHandler.anim.SetBool("canDoCombo", false);

                    if (lastAttack == weapon.OH_Light_Attack_1)
                    {
                        animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                    }
                    else if (lastAttack == weapon.TH_Light_Attack_1)
                    {
                        animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                    }

                    if (lastAttack == weapon.OH_Heavy_Attack_1)
                    {
                        animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_2, true);
                    }

                    if (lastAttack == weapon.Backstep)
                    {
                        animatorHandler.PlayTargetAnimation(weapon.GroundSmash, true);
                    }
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (!playerManager.isInDialogue)
            {
                if (playerStats.currentStamina <= 0)
                    return;

                weaponSlotManager.attackingWeapon = weapon;
                if (inputHandler.twoHandFlag)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
                    lastAttack = weapon.TH_Light_Attack_1;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                    lastAttack = weapon.OH_Light_Attack_1;
                }
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (!playerManager.isInDialogue)
            {
                if (playerStats.currentStamina <= 0)
                    return;

                weaponSlotManager.attackingWeapon = weapon;
                if (inputHandler.twoHandFlag)
                {

                }
                else
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                    lastAttack = weapon.OH_Heavy_Attack_1;
                }
            }
        }


    }
}