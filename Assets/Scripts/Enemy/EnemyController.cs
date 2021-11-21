using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : Entity
    {
        const int PlayerLayerMask = ~(1 << 7);

        public EnemyObject enemyObject;

        private EnemyBehaviour MovementBehaviour { get; set; }

        public bool PlayerDetected { get; private set; }
        private PlayerController Player { get; set; }

        private Rigidbody2D Rb { get; set; }
        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
            MovementBehaviour = AI.GetBehaviourType(enemyObject.aiType, this);
            Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        private void Update()
        {
            if(!PlayerDetected)
                DetectPlayer();
            Rb.velocity = MovementBehaviour.GetVelocity(Player);
        }

        private void DetectPlayer()
        {
            if (PlayerDetected) return;
            
            var pos = transform.position;
            
            var hit = Physics2D.Raycast(pos, Player.transform.position - pos, enemyObject.detectionDistance, PlayerLayerMask);    
            var hitCollider = hit.collider;
            if ((object) hitCollider != null && hitCollider.CompareTag("Player"))
            {
                PlayerDetected = true;
            }
        }
    }
}