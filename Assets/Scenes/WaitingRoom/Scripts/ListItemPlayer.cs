using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperSad.Gameplay
{
    public class ListItemPlayer : BasePlayer {

        [SerializeField]
        private Image avatarIcon;

        public void SetPlayerDetails(string displayName, Color color, Sprite headIcon)
        {
            // Set username
            SetUsername(displayName, color);

            // Set avatar head icon
            avatarIcon.sprite = headIcon;
            avatarIcon.preserveAspect = true;
        }

        public override void KaomojiReact(Sprite reactSprite)
        {
            Debug.Log("Local Kaomoji");

            // attach avatar icon
            KaomojiDisplayer kaomojiDisplayer = (KaomojiDisplayer)playerKaomoji;
            kaomojiDisplayer.SetAvatarIcon(avatarIcon.sprite);

            base.KaomojiReact(reactSprite);
        }
    }
}