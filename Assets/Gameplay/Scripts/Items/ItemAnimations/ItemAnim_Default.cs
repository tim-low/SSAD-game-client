using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_Default : ItemAnimation
    {
        [SerializeField]
        private float animDuration = 2.5f;
        [SerializeField]
        private float speed = 0.25f;
        [SerializeField]
        private string attachTo = "Head_end";
        [SerializeField]
        [Tooltip("Offset from transform to attach")]
        private Vector3 offset;

        public override void InitAnimation(StateReference refData)
        {
            // Get user GamePlayer
            GamePlayer user = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            // Find Head
            Transform head = user.transform.FindDeepChild(attachTo);

            // Set position to head
            transform.position = head.position + offset;
        }

        protected override IEnumerator Animate()
        {
            float timer = 0f;
			AudioManager.instance.PlaySFX("Coin");
            while (timer < animDuration)
            {
                transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
                timer += Time.deltaTime;
                yield return null;
            }

            CompleteAnimation();
        }
    }

}