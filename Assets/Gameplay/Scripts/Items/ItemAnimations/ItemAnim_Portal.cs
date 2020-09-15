using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_Portal : ItemAnimation {

        [SerializeField]
        [Tooltip("Initial duration for portal to appear")]
        private float appearDuration = 0.5f;
        [SerializeField]
        [Tooltip("Duration to walk to the portal")]
        private float walkToDuration = 1f;
        [SerializeField]
        [Tooltip("Duration to walk from the portal")]
        private float walkFromDuration = 1f;
        [SerializeField]
        [Tooltip("Animation Playback speed")]
        private float walkAnimSpeed = 1f;
        [SerializeField]
        [Tooltip("Item offset position from the player")]
        private Vector3 offset;

        private Vector3 destinationPos;
        private GamePlayer user;

        public override void InitAnimation(StateReference refData)
        {
            // Get user
            user = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            // Set item position to user
            transform.position = user.transform.position + offset;

            // Get destination position
            destinationPos = refData.GameBoard.GetTilePos(refData.VictimTile);
        }

        protected override IEnumerator Animate()
        {
            // scale item up
            Vector3 targetScale = transform.localScale;
            float timer = 0f;
			AudioManager.instance.PlaySFX("Door");
            while (timer < appearDuration)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, timer / appearDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;

            // Start moving
            user.PlayMoveAnimation(walkAnimSpeed);
            user.Rotate(Direction.Up);

            // Player move to item
            timer = 0f;
            Vector3 startPos = user.transform.position;
            while (timer < walkToDuration)
            {
                user.transform.position = Vector3.Lerp(startPos, transform.position, timer / walkToDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            // Set item to destination
            transform.position = destinationPos + offset;

            // player move to destination
            user.Rotate(Direction.Down);    // player movement direction
            timer = 0f;
            while (timer < walkFromDuration)
            {
                user.transform.position = Vector3.Lerp(transform.position, destinationPos, timer / walkFromDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            user.StopMoveAnimation();

            CompleteAnimation();
        }
    }

}