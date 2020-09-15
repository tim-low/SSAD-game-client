using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_FallAndScale : ItemAnimation {

        [SerializeField]
        private float startingHeight = 10f;
        [SerializeField]
        private float fallSpeed = 10f;
        [SerializeField]
        private float accelerate = 5f;
        [SerializeField]
        private float scaleSpeed = 1f;
        [SerializeField]
        private float targetUniformScale = 1f;
        [SerializeField]
        private string attachTo = "Head_end";
        [SerializeField]
        [Tooltip("Offset from target transform")]
        private Vector3 offsetTargetPos;

        private Vector3 targetPos;

        public override void InitAnimation(StateReference refData)
        {
            // Get user GamePlayer
            GamePlayer user = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            // Find Head
            Transform head = user.transform.FindDeepChild(attachTo);

            // Set position to above head
            targetPos = head.position + offsetTargetPos;

            // Set item position to be above target
            transform.position = targetPos + new Vector3(0f, startingHeight, 0f);
        }

        protected override IEnumerator Animate()
        {
            // Fall from sky
            while (transform.position.y > targetPos.y)
            {
                transform.position -= new Vector3(0f, fallSpeed * Time.deltaTime, 0f);
                yield return null;
            }
		
            transform.position = targetPos;
            yield return new WaitForSeconds(0.4f);

            // Scale
			AudioManager.instance.PlaySFX("Crown");
            float currScale = transform.localScale.x;
            while (currScale < targetUniformScale)
            {
                currScale += scaleSpeed * Time.deltaTime;
                transform.localScale = new Vector3(currScale, currScale, currScale);
                fallSpeed += accelerate * Time.deltaTime;
                yield return null;
            }

            CompleteAnimation();
        }
    }

}