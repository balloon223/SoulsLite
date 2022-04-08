using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class RollingHeal : MonoBehaviour
    {
        PlayerStats playerStats;
        public HealthBar healthbar;

        // Start is called before the first frame update
        void Start()
        {
            playerStats = FindObjectOfType<PlayerStats>();
            healthbar = FindObjectOfType<HealthBar>();

            playerStats.currentHealth += 5;
            healthbar.SetCurrentHealth(playerStats.currentHealth);
        }

        // Update is called once per frame
        void Update()
        {
            if (playerStats.currentHealth >= 100)
            {
                playerStats.currentHealth = 100;
            }
        }
    }
}