using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class PlayerKaomoji : MonoBehaviour {

        [SerializeField]
        protected Kaomoji kaomoji;

        private IEnumerator reactCoroutine = null;

        void Start()
        {
            kaomoji.gameObject.SetActive(false);
        }

        public virtual void ShowKaomoji(Sprite reactSprite)
        {
            // Stop existing reaction if there is
            if (reactCoroutine != null)
                StopCoroutine(reactCoroutine);

            reactCoroutine = ShowReact(reactSprite);
            StartCoroutine(reactCoroutine);
        }

        private IEnumerator ShowReact(Sprite reactSprite, float duration = 3f)
        {
            // Show reaction
            kaomoji.GetComponent<SpriteRenderer>().sprite = reactSprite;
            kaomoji.gameObject.SetActive(true);

            // Reset Kaomoji's position
            kaomoji.ResetPosition();

            // Wait for duration
            yield return new WaitForSeconds(duration);

            // End reaction
            kaomoji.gameObject.SetActive(false);
            reactCoroutine = null;
        }

    }

}