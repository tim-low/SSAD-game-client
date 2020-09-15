using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemSet : MonoBehaviour {

        [SerializeField]
        private GameObject[] itemList;

        void Start()
        {
            // disable inventory items first
            gameObject.SetActive(false);
        }

        public GameObject GetItem(int id)
        {
            return itemList[id];
        }
    }

}