using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class triggerGateController : MonoBehaviour
    {
        public bool hasInteracted = false;

        public GameObject myDam;
        public AudioSource audio;
        [SerializeField] private Animator myButton = null;

        [SerializeField] private bool openTrigger = false;

        private void Awake()
        {
            audio = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasInteracted == false)
            {
                if (other.CompareTag("Player"))
                {
                    Destroy(myDam);
                    myButton.Play("button_pressed", 0, 0.0f);
                    audio.Play();
                    hasInteracted = true;
                }
            }
        }
    }
}