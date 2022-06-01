using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerStats : CharacterStats
    {
        AudioSource audioSource;
        PlayerManager playerManager;
        public HealthBar_Player healthbar;
        public StaminaBar staminabar;
        AnimatorHandler animatorHandler;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenerationTimer = 0;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            playerManager = GetComponent<PlayerManager>();
            healthbar = FindObjectOfType<HealthBar_Player>();
            staminabar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminabar.SetMaxStamina(maxStamina);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            audioSource.Play();

            currentHealth = currentHealth - damage;
            healthbar.SetCurrentHealth(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death_01", true);
                isDead = true;
                //HANDLE PLAYER DEATH
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminabar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0;
            }
            else
            {
                staminaRegenerationTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenerationTimer > 0.1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminabar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Water")
            {
                animatorHandler.PlayTargetAnimation("Death_01", true);
            }
        }
    }
}