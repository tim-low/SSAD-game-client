using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_PillarFalling : ItemAnimation {

        [SerializeField]
        private Transform pillarObject;
        [SerializeField]
        private ParticleSystem dustSmokeParticles;

        public override void InitAnimation(StateReference refData)
        {
            // Set Pillar position
            GamePlayer player = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            transform.position = player.transform.position;

            // Rotate Pillar to face direction
            transform.eulerAngles = GamePlayer.GetEulerAngles(player.DirectionFaced);

            // Scale Pillar according to length of tiles to hit
            float heightScale = 1f;
            switch (player.DirectionFaced)
            {
                case Direction.Up:
                    heightScale = refData.GlobalVariables.YLength - player.Position.y;
                    break;
                case Direction.Down:
                    heightScale = player.Position.y + 1;
                    break;
                case Direction.Left:
                    heightScale = player.Position.x + 1;
                    break;
                case Direction.Right:
                    heightScale = refData.GlobalVariables.XLength - player.Position.x;
                    break;
            }

            pillarObject.localScale = new Vector3(pillarObject.localScale.x, heightScale, pillarObject.localScale.z);
        }

        protected override IEnumerator Animate()
        {
            yield return null;
        }
			
		public void PillarFall()
		{
			AudioManager.instance.PlaySFX("PillarDrop");
		}
        public void DustSmokeParticles()
        {
            if (dustSmokeParticles != null)
            {
                ParticleSystem ps = Instantiate(dustSmokeParticles, transform);
                ps.transform.localPosition = new Vector3(0f, 0f, 0.5f * pillarObject.localScale.y);
                var sh = ps.shape;
                sh.scale = pillarObject.localScale;
            }
        }
    }

}