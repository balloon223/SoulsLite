using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class TriggerCaveController : MonoBehaviour
    {
        public bool hasInteracted = false;

        private Collider collider;
        ObjectStats objectStats;
        public AudioSource audio;
        [SerializeField] private Animator myButton = null;

        [SerializeField] private bool openTrigger = false;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            objectStats = GetComponent<ObjectStats>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "attacks")
            {
                myButton.Play("rock_shake", 0, 0.0f);
            }

            /*   if (hasInteracted == false)
               {
                   if (other.CompareTag("Player"))
                   {
                       myButton.Play("button_pressed", 0, 0.0f);

                       hasInteracted = true;
                   }
               }
            */
        }
    }
}