using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class Tilemap3D : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("For visualisation only")]
        protected int xLength;
        [SerializeField]
        [Tooltip("For visualisation only")]
        protected int yLength;

        [SerializeField]
        protected GameTile tilePrefab;

        protected GameTile[,] tilemap;  // reference to each tile by index

        // Start is called before the first frame update
        void Start()
        {
            //tilemap = new GameObject[rows, columns];
            //SpawnTiles();
        }

        public void SpawnTiles(int xLength, int yLength)
        {
            tilemap = new GameTile[xLength, yLength];

            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    // Create Tile
                    GameTile tile = Instantiate(tilePrefab, transform);
                    // Set world position of Tile
                    tile.transform.position = CalcTilePosition(x, y);
                    tile.transform.position -= new Vector3(0f, 0.5f * tile.transform.localScale.y, 0f);
                    // Set TilePosition of Tile
                    tile.SetTilePosition(x, y);

                    //Debug.Log("X: " + x + ", Y: " + y);

                    // Assign Tile to tilemap
                    tilemap[x, y] = tile;
                }
            }
        }

        public void SetTileColor(int i, int j, Color color)
        {
            GameTile tile = tilemap[i, j];
            tile.GetComponent<MeshRenderer>().material.color = color;
        }

        public GameTile GetTileObject(int i, int j)
        {
            return tilemap[i, j];
        }

        public virtual Vector3 CalcTilePosition(int i, int j)
        {
            return transform.position + new Vector3(i * (tilePrefab.transform.localScale.x + 0.05f), 0f, j * (tilePrefab.transform.localScale.z + 0.05f));
        }

        protected void OnDrawGizmosSelected()
        {
            // Draw a semitransparent blue cube at the transforms position
            if (tilePrefab != null)
            {
                for (int i = 0; i < xLength; i++)
                {
                    for (int j = 0; j < yLength; j++)
                    {
                        Gizmos.color = new Color(0, 0, 0.8f, 0.5f);
                        Gizmos.DrawWireMesh(tilePrefab.GetComponent<MeshFilter>().sharedMesh, -1,
                                            CalcTilePosition(i, j),
                                            tilePrefab.transform.rotation,
                                            tilePrefab.transform.localScale
                        );
                    }
                }
            }
        }
    }

}