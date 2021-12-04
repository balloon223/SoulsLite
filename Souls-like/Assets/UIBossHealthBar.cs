using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG
{
    public class UIBossHealthBar : MonoBehaviour
    {
        public Text bossName;
        public Slider slider;

        public void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            bossName = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            SetHealthBarToActive();
        }

        public void SetBossName(string name)
        {
            bossName.text = name;
        }

        public void SetHealthBarToActive()
        {
            slider.gameObject.SetActive(true);
        }

        public void SetHealthBarToInactive()
        {
            slider.gameObject.SetActive(false);
        }

        public void SetBossMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetBossCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }
    }
}