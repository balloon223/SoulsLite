using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EnemyStats : CharacterStats
    {
        AudioSource audioSource;
        EnemyBossManager enemyBossManager;
        Animator animator;

        public UIEnemyHealthBar enemyHealthBar;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            animator = GetComponentInChildren<Animator>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        void Start()
        {
            enemyHealthBar.SetMaxHealth(maxHealth);
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
            audioSource.Play();


            currentHealth = currentHealth - damage;
            enemyBossManager.UpdateBossHealthBar(currentHealth);
            //enemyHealthBar.SetHealth(currentHealth);

            if (currentHealth > 0)
            {
                animator.Play("Damage_01");
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