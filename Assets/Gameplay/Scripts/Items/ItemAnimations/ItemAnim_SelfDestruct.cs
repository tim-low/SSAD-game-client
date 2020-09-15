using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_SelfDestruct : ItemAnimation {

        [SerializeField]
        private float rotateSpeed = 10f;
        [SerializeField]
        private float endAngle = 180f;
        [SerializeField]
        private float pauseDuration = 0.6f;
        [SerializeField]
        private string attachTo = "Head_end";
        [SerializeField]
        [Tooltip("Offset from transform to attach")]
        private Vector3 offset;
        [SerializeField]
        private GameObject smokeParticles;

        private Vector3 userPos;

        public override void InitAnimation(StateReference refData)
        {
            // Get user GamePlayer
            GamePlayer user = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            userPos = user.transform.position;
            // Find Head
            Transform head = user.transform.FindDeepChild(attachTo);

            // Set position to head
            transform.position = head.position + offset;

        }

        protected override IEnumerator Animate()
        {
            float angle = 0f;
            while (angle < endAngle)
            {
                //Debug.Log(angle);
                angle += rotateSpeed * Time.deltaTime;
                transform.eulerAngles = new Vector3(0f, angle, 0f);
                yield return null;
            }

            transform.eulerAngles = new Vector3(0f, endAngle, 0f);

            GameObject vfx = Instantiate(smokeParticles);
            vfx.transform.position = userPos;
			AudioManager.instance.PlaySFX("SelfDestruct");
            yield return new WaitForSeconds(pauseDuration);

            CompleteAnimation();

        }
    }

}