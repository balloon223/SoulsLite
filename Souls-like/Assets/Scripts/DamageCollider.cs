using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamageCollider : MonoBehaviour
    {
        PlayerManager playerManager;

        Collider damageCollider;
        public GameObject hitParticles;
        public GameObject absorbParticles;
        public Transform weaponTip;

        public bool isAttacking;

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            playerManager = FindObjectOfType<PlayerManager>();

            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            isAttacking = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            isAttacking = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            Vector3 closestHitPoint = collision.ClosestPoint(transform.position);
            if (collision.tag == "Player")
            {
                if (playerManager.isInvulnerable == false)
                {
                    Instantiate(hitParticles, closestHitPoint, Quaternion.identity);
                    PlayerStats playerStats = collision.GetComponent<PlayerStats>();

                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(currentWeaponDamage);
                    }
                }
                else
                {
                    Instantiate(absorbParticles, collision.transform.position, Quaternion.identity);
                }
            }
        }
    }
}