using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperSad.Gameplay
{
    public class KaomojiDisplayer : PlayerKaomoji {

        [Header("Scale")]
        [SerializeField]
        private float minScale = 0.5f;
        [SerializeField]
        private float maxScale = 1.2f;

        [Header("Height")]
        [SerializeField]
        private float minHeight = -450f;
        [SerializeField]
        private float maxHeight = 450f;

        [Header("Speed")]
        [SerializeField]
        private float minSpeed = 15f;
        [SerializeField]
        private float maxSpeed = 40f;

        private Sprite avatarIconSprite;

        void Start()
        {
            // Init seed
            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        /// <summary>
        /// Sets Avatar Icon for the next Kaomoji.
        /// </summary>
        public void SetAvatarIcon(Sprite avatarIcon)
        {
            // Set Avatar icon
            avatarIconSprite = avatarIcon;
        }

        public override void ShowKaomoji(Sprite reactSprite)
        {
            // Create Kaomoji
            Kaomoji newKaomoji = Instantiate(kaomoji, transform.position, transform.rotation);
            newKaomoji.transform.SetParent(transform);
            newKaomoji.gameObject.SetActive(true);

            // Set Kaomoji Sprite
            newKaomoji.GetComponent<Image>().sprite = reactSprite;

            // Set Avatar Icon
            ScreenKaomoji screenKaomoji = (ScreenKaomoji)newKaomoji;
            screenKaomoji.SetAvatarIcon(avatarIconSprite);

            // Set random speed
            screenKaomoji.SetSpeed(Random.Range(minSpeed, maxSpeed));

            // Set random position for Kaomoji
            newKaomoji.transform.localPosition = new Vector3(newKaomoji.transform.localPosition.x, Random.Range(minHeight, maxHeight), 0f);

            // Set random size for Kaomoji
            float randomScale = Random.Range(minScale, maxScale);
            newKaomoji.transform.localScale = new Vector3(randomScale, randomScale, 1f);
        }
    }
}