using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Item/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string Right_Hand_Idle;
        public string Left_Hand_Idle;
        public string th_idle;

        [Header("One Handed Attack Animations")]
        //Light Attacks
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string TH_Light_Attack_1;
        public string TH_Light_Attack_2;

        //Heavy Attacks
        public string OH_Heavy_Attack_1;
        public string OH_Heavy_Attack_2;

        //Ultimate
        public string Backstep;
        public string GroundSmash;

        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;
    }
}