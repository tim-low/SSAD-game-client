using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay {
    public abstract class ItemTarget : MonoBehaviour {

        [SerializeField]
        private Collider thisCollider;

        private GameItemTargeter gameItemTargeter;
        public void SetGameItemTargeter(GameItemTargeter gameItemTargeter) {
            this.gameItemTargeter = gameItemTargeter;
        }

        void Awake()
        {
            if (thisCollider == null)
                thisCollider = GetComponent<Collider>();
        }

        void OnEnable()
        {
            thisCollider.enabled = true;
        }

        void OnDisable()
        {
            thisCollider.enabled = false;
        }

        protected void OnMouseDown()
        {
            // Send component information to Item Effect Manager
            gameItemTargeter.SetItemTarget(GetComponentData());
        }

        protected abstract object GetComponentData();
    }

}