using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_TranslateFromTile : ItemAnimation {

        [SerializeField]
        private float translateDuration;
        [SerializeField]
        private float waitAfterTranslating;
        [SerializeField]
        private Vector3 translateBy;

        private Vector3 targetPos;

        public override void InitAnimation(StateReference refData)
        {
            // Find position of Target
            targetPos = refData.GameBoard.GetTilePos(refData.VictimTile);

            // Set item to the offset
            transform.position = targetPos - translateBy;
        }

        protected override IEnumerator Animate()
        {
            // translate
            float timer = 0;
            Vector3 startPos = transform.position;
			AudioManager.instance.PlaySFX("GroundSpike");
            while (timer < translateDuration)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, timer / translateDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            // pause for a while
            yield return new WaitForSeconds(waitAfterTranslating);

            CompleteAnimation();
        }
    }

}