using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public abstract class ItemAnimation : MonoBehaviour {

        public enum ToDestroy
        {
            Immediate,
            EndOfNextTurn,  // Lock
            EndOfCycle, // GroundSpike
            EndOfTargetPlayerTurn   // Cage
        }

        [SerializeField]
        private ToDestroy toDestroy;
        public ToDestroy WhenToDestroy
        {
            get { return toDestroy; }
        }

        public bool AnimationCompleted { get; private set; }

        // Use this for initialization
        public void StartAnimation()
        {
            AnimationCompleted = false;
            StartCoroutine(Animate());
        }

        public abstract void InitAnimation(StateReference refData);
        protected abstract IEnumerator Animate();

        public void CompleteAnimation()
        {
            AnimationCompleted = true;
        }
    }

}