using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class ObjectStats : CharacterStats
    {
        Level1Manager level1Manager;
        private Collider collider;

        AudioSource audioSource;
        EnemyBossManager enemyBossManager;
        Animator animator;

        public bool isBoss;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            audioSource = GetComponent<AudioSource>();
            animator = GetComponentInChildren<Animator>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        void Start()
        {

        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            audioSource.Play();

            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                audioSource.Play();
                collider.enabled = !collider.enabled;
                //HANDLE OBJECT DEATH
            }
        }
    }
}