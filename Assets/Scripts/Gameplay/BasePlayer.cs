using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperSad.Gameplay
{
    public class BasePlayer : MonoBehaviour {

        [SerializeField]
        protected Text usernameText;
        [SerializeField]
        protected PlayerKaomoji playerKaomoji;

        protected void SetUsername(string username, Color color)
        {
            // Set username
            usernameText.text = username;
            usernameText.color = color;
        }

        public virtual void KaomojiReact(Sprite reactSprite)
        {
            playerKaomoji.ShowKaomoji(reactSprite);
            AudioManager.instance.PlaySFX(Random.Range(1, 4).ToString());
        }
    }
}