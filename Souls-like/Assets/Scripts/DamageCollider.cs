using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        public GameObject hitParticles;
        public Transform weaponTip;

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                Instantiate(hitParticles, collision.transform.position, Quaternion.identity);
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            
                if(playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }

            if(collision.tag == "Enemy")
            {
                Instantiate(hitParticles, weaponTip.transform.position, Quaternion.identity);
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

                if(enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                }
            }
        }
    }
}