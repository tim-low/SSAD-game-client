using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_Projectile : ItemAnimation {

        [SerializeField]
        private float animDuration = 2f;
        [SerializeField]
        private float targetHeight = 2f;
        [SerializeField]
        [Tooltip("Offset from GamePlayer's origin")]
        private Vector3 positionOffset;
        [SerializeField]
        private AnimationCurve animCurve;

        [SerializeField]
        private GameObject hitParticleEffect;

        private GamePlayer user;
        private Vector3 targetPos;

        public override void InitAnimation(StateReference refData)
        {
            // Get user GamePlayer position
            user = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            transform.position = user.transform.position + positionOffset;

            // Get target GamePlayer position
            GamePlayer target = refData.PlayerManager.GetGamePlayer(refData.VictimToken);
            targetPos = target.transform.position + positionOffset;
        }

        private float RotateBy(Vector3 start, Vector3 target)
        {
            Vector3 dir = (target - start).normalized;
            dir.y = 0f; // don't need to look up/down

            //float dot = Vector3.Dot(Vector3.forward, dir);
            return Mathf.Rad2Deg * Mathf.Atan2(dir.x, dir.z);
        }

        protected override IEnumerator Animate()
        {
            // Rotate user to face target
            Vector3 originalEulerAngles = user.transform.eulerAngles;
            user.transform.eulerAngles = new Vector3(0f, RotateBy(transform.position, targetPos), 0f);

            float timer = 0f;
            float initialHeight = transform.position.y;
            Vector3 startPos = transform.position;
            while (timer < animDuration)
            {
                // get percentage of animation duration
                float timeProportion = timer / animDuration;

                // Lerp projectile position according to AnimationCurve
                Vector3 nextPos = Vector3.Lerp(startPos, targetPos, timeProportion);

                // get height
                nextPos.y = Mathf.Lerp(initialHeight, initialHeight + targetHeight, animCurve.Evaluate(timeProportion));
                // set height
                transform.position = nextPos;

                timer += Time.deltaTime;
                yield return null;
            }

            GameObject vfx = Instantiate(hitParticleEffect);
			AudioManager.instance.PlaySFX("BoulderSpike"); //Sfx
            vfx.transform.position = targetPos;

            // Rotate user back
            user.transform.eulerAngles = originalEulerAngles;

            CompleteAnimation();
        }
    }

}