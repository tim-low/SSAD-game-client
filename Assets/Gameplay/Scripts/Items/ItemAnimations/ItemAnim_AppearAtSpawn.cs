using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_AppearAtSpawn : ItemAnimation {

        [SerializeField]
        private float waitDuration = 1.5f;
        [SerializeField]
        private GameObject vfxAtSpawnTile;

        public override void InitAnimation(StateReference refData)
        {
            // Get user's player id (1 to 4)
            int playerId = refData.PlayerManager.GetGamePlayer(refData.UserToken).Player.Color;
            // Get spawn position of user
            Vector3 spawnPos = refData.GameBoard.GetTilePos(refData.GlobalVariables.GetSpawnPosition(playerId));

            // Set position to spawn position
            transform.position = spawnPos;
            // Instantiate VFX
            GameObject vfx = Instantiate(vfxAtSpawnTile);
            vfx.transform.position = transform.position;
        }

        protected override IEnumerator Animate()
        {
			AudioManager.instance.PlaySFX("Bed"); //sfx
            yield return new WaitForSeconds(waitDuration);

            CompleteAnimation();
        }
    }

}