using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyActions
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 360;
        public float minimumAttackAngle = -250;

        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;
    }
}