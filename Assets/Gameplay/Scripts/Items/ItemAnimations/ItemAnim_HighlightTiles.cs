using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_HighlightTiles : ItemAnimation {

        [SerializeField]
        private float flashDuration = 1f;
        [SerializeField]
        private Color emissionColor;
        [SerializeField]
        private string attachTo = "Head_end";
        [SerializeField]
        [Tooltip("Offset from transform to attach")]
        private Vector3 offset;
        [SerializeField]
        private AnimationCurve colorFadeCurve;

        private List<Material> boardTileMaterials;

        public override void InitAnimation(StateReference refData)
        {
            boardTileMaterials = new List<Material>();

            // Get user
            GamePlayer user = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            // Get user's player id (player 1 to 4)
            int playerColor = user.Player.Color;
            // Set position of item
            Transform attachToTransform = user.transform.FindDeepChild(attachTo);
            transform.SetParent(attachToTransform);
            transform.position = attachToTransform.position + offset;

            // Set tiles to highlight
            for (int x = 0; x < refData.GlobalVariables.XLength; x++)
            {
                for (int y = 0; y < refData.GlobalVariables.YLength; y++)
                {
                    if (refData.GameBoard.GetTileState(x, y) == (GameBoard.TileState)playerColor)
                    {
                        boardTileMaterials.Add(refData.GameBoard.GetGameTile(x, y).GetComponent<MeshRenderer>().material);
                    }
                }
            }
            Debug.Log("Checked Tiles");
        }

        protected override IEnumerator Animate()
        {
            /* Enable Emission
            foreach (Material mat in boardTileMaterials)
            {
                mat.EnableKeyword("_EMISSION");
            }*/

            Color startColor = Color.black;
			AudioManager.instance.PlaySFX("Lock");
            float timer = 0f;
            while (timer < flashDuration)
            {
                float timeProportion = timer / flashDuration;
                foreach (Material mat in boardTileMaterials)
                {
                    mat.SetColor("_EmissionColor", Color.Lerp(startColor, emissionColor, colorFadeCurve.Evaluate(timeProportion)));
                }
                timer += Time.deltaTime;
                yield return null;
            }

            // Set Emission Color to black (0)
            foreach (Material mat in boardTileMaterials)
            {
                mat.SetColor("_EmissionColor", startColor);
            }

            /* Disable Emission
            foreach (Material mat in boardTileMaterials)
            {
                mat.DisableKeyword("_EMISSION");
            }*/
            CompleteAnimation();
        }
    }

}