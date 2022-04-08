using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class FlameHandler : MonoBehaviour
    {
        Collider collider;
        PlayerLocomotion playerLocomotion;
        // Start is called before the first frame update
        void Start()
        {
            playerLocomotion = FindObjectOfType<PlayerLocomotion>();
            collider = GetComponent<Collider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerLocomotion.isRolling)
            {
                collider.enabled = false;
            }
            else
            {
                collider.enabled = true;
            }
        }
    }
}