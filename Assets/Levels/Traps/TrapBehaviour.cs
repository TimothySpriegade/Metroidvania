using System;
using System.Collections;
using SOEventSystem.Events;
using UnityEngine;

namespace Levels.Traps
{
    public class TrapBehaviour : MonoBehaviour
    {
        [SerializeField] private VoidEvent onSpikeHit;
        [SerializeField] private VoidEvent onSpikeLeave;
        private float lastHit;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                onSpikeHit.Invoke();
            } 
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                onSpikeLeave.Invoke();
            } 
        }
    }
}
