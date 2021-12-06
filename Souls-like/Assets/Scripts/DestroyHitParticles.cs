using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHitParticles : MonoBehaviour
{
    public float destroyTimer;

    void Update()
    {
        if (destroyTimer > 0)
        {
            destroyTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
