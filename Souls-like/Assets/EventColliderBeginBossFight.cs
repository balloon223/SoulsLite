using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EventColliderBeginBossFight : MonoBehaviour
    {
        WorldEventManager worldEventManager;
        AudioSource audio;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
            audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                worldEventManager.ActivateBossFight();
                audio.Play();
            }
        }

        public void Update()
        {
            if (worldEventManager.bossFightIsActive)
            {
                audio.Play();
            }
            else
            {
                audio.Stop();
            }
        }
    }
}
