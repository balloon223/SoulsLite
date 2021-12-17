using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
        }

       /* private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidBody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = -0.1f;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidBody.velocity = velocity;
        } */
        
        private void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                enemyManager.isPerformingAction = false;
                var velocity = anim.deltaPosition / Time.deltaTime;

                // Keep y part of velocity.
                velocity.y = enemyManager.enemyRigidBody.velocity.y;
                enemyManager.enemyRigidBody.velocity = velocity;
            }
        }
    }
}