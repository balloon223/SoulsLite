using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EnemyStats : CharacterStats
    {
        AudioSource audioSource;

        Animator animator;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth = currentHealth - damage;

            if (currentHealth > 0)
            {
                animator.Play("Damage_01");
                audioSource.Play();
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Death_01");
                isDead = true;
                audioSource.Play();
                //HANDLE ENEMY DEATH
            }
        }
    }
}