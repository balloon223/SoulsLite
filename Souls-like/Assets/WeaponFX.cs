using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("Weapon FX")]
        public ParticleSystem normalWeaponTrail;
        DamageCollider damageCollider;

        public void Awake()
        {
            damageCollider = FindObjectOfType<DamageCollider>();
        }


        public void Update()
        {
            if (damageCollider.isAttacking == true)
            {
                normalWeaponTrail.Play();
            }

            else
            {
                normalWeaponTrail.Stop();
            }
        }

    }
}