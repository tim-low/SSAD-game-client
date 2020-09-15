using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_FallFromSky : ItemAnimation
    {
        [SerializeField]
        private GameItem.TypeOfTarget targetType;
        [SerializeField]
        private float startingHeight = 10f;
        [SerializeField]
        private float fallSpeed = 10f;
        [SerializeField]
        private float accelerate = 5f;
        [SerializeField]
        private float waitAfterLanding = 0.6f;

        private Vector3 targetPos;

        public override void InitAnimation(StateReference refData)
        {
            // Find position of Target
            if (targetType == GameItem.TypeOfTarget.Tile)
            {
                targetPos = refData.GameBoard.GetTilePos(refData.VictimTile);
            }
            else if (targetType == GameItem.TypeOfTarget.Player)
            {
                GamePlayer gamePlayer = refData.PlayerManager.GetGamePlayer(refData.VictimToken);
                targetPos = gamePlayer.transform.position;

                // attach as a child of the target player
                transform.SetParent(gamePlayer.transform);
            }

            // Set item position to be above target
            transform.position = targetPos + new Vector3(0f, startingHeight, 0f);
        }

        protected override IEnumerator Animate()
        {
			if (targetType == GameItem.TypeOfTarget.Player) {
				yield return new WaitForSeconds(0.3f);
				AudioManager.instance.PlaySFX("Cage");
			} 

            while (transform.position.y > targetPos.y)
            {
                transform.position -= new Vector3(0f, fallSpeed * Time.deltaTime, 0f);
                fallSpeed += accelerate * Time.deltaTime;
                yield return null;
            }


            transform.position = targetPos;

			if (targetType == GameItem.TypeOfTarget.Tile) {
				AudioManager.instance.PlaySFX("Flag");
			}

            yield return new WaitForSeconds(waitAfterLanding);

            CompleteAnimation();
        }
    }

}