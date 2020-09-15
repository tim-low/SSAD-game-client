using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Networking;

namespace SuperSad.Gameplay
{
    public class WheelItem : MonoBehaviour
    {

        [SerializeField]
        private Image backgroundImage;
        [SerializeField]
        private Image spriteImage;

        [SerializeField]
        private Button button;
        public Button Button
        {
            get { return button; }
        }

        private int id;

        public void SetId(int id)
        {
            this.id = id;
        }

        public void SendCommand(PlayerManager playerManager)
        {
            // Create react packet
            Packet cmd = new CmdSendKaomoji()
            {
                Token = UserState.Instance().Token,
                KaomojiId = id
            }.CreatePacket();

            // Send packet
            NetworkStreamManager.Instance.SendPacket(cmd);

            // Display Kaomoji immediately for self
            Debug.Log("Kaomoji: " + id);
            playerManager.KaomojiReact(UserState.Instance().Token, id);
        }

        public void SetSprite(Sprite sprite)
        {
            // Set Sprite
            spriteImage.sprite = sprite;
            spriteImage.preserveAspect = true;

            // Resize background image
            //Debug.Log("Bounds: " + spriteImage.sprite.bounds.extents);
            //Debug.Log("Border: " + spriteImage.sprite.border);
            //Debug.Log("Rect: " + spriteImage.sprite.rect);
            //Debug.Log("Texture Width: " + spriteImage.sprite.texture.width);
            Vector3 extents = spriteImage.sprite.bounds.extents;
            backgroundImage.GetComponent<RectTransform>().sizeDelta = new Vector2(extents.x * 80 + 80, extents.y * 130);
        }
    }

}