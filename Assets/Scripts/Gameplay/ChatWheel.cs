using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SuperSad.Gameplay
{
    [RequireComponent(typeof(RectTransform))]
    public class ChatWheel : MonoBehaviour {

        [SerializeField]
        private WheelItem itemPrefab;
        [SerializeField]
        private int radiusReduction;
        [SerializeField]
        private Sprite[] itemList;
        [SerializeField]
        private GameObject chatWheelPanel;

        [SerializeField]
        private PlayerManager playerManager;

        public Sprite GetReact(int id)
        {
            if (id < 0 || id > itemList.Length)
                return null;

            return itemList[id];
        }

        // Use this for initialization
        void Start()
        {
            InitWheel();
        }

        public void InitWheel()
        {
            RectTransform rt = GetComponent<RectTransform>();
            int radius = (int)rt.sizeDelta.y / 2 - radiusReduction; // half of size of this RectTransform object

            float angle = 360f / itemList.Length;   // angle between each item

            int idxCount = 0;
            Vector3 dir = radius * Vector3.up;
            // Spawn items in circle
            foreach (Sprite sprite in itemList)
            {
                WheelItem newItem = Instantiate(itemPrefab);
                newItem.transform.SetParent(transform);
                newItem.SetSprite(sprite);
                newItem.gameObject.SetActive(true);
                newItem.SetId(idxCount);

                newItem.transform.localPosition = Quaternion.Euler(0, 0, idxCount * angle) * dir;
                ++idxCount;

                Button button = newItem.Button;
                //button.onClick.AddListener(() => SendCommand(idxCount));
                button.onClick.AddListener(() => newItem.SendCommand(playerManager));   // send selected chat command
                button.onClick.AddListener(() => chatWheelPanel.SetActive(false));  // disable panel
            }
        }
    }

}