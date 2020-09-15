using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class GamePlayer : BasePlayer3D {

        [SerializeField]
        private MeshRenderer playerIdIndicator;
        [SerializeField]
        private float runAnimDuration = 1f;

        public Player Player { get; private set; }
        public Direction DirectionFaced { get; private set; }

        private TilePosition position;  // position on GameBoard
        public TilePosition Position
        {
            get { return position; }
            set { position = value; }
        }

        private Dictionary<ItemType, int> playerInventory;
        public Dictionary<ItemType, int> PlayerInventory
        {
            get { return playerInventory; }
        }

        public void SetPlayerDetails(Player player, Color color, Attributes attributes)
        {
            Player = player;
            //PlayerId = id;
            //Token = token;

            // Set Username
            SetUsername(player.Username, color);
            //usernameText.text = player.Username;
            //usernameText.color = color;

            // Set Indicator
            playerIdIndicator.material.color = color;

            // Set Avatar
            SetAvatar(attributes);
        }

        // Use this for initialization
        void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            // Initialize Inventory
            playerInventory = new Dictionary<ItemType, int>();
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                if (type != ItemType.NIL)
                    playerInventory.Add(type, 0);
            }
        }

        public void Move(Direction moveDir, TilePosition newTilePosition, Vector3 targetPos)
        {
            // Rotate player to face moveDir
            Rotate(moveDir);

            // Start move Animation Coroutine
            StartCoroutine(MoveToTile(newTilePosition, targetPos));
        }

        public void Rotate(Direction dir)
        {
            // save Direction faced
            DirectionFaced = dir;

            // Rotate object
            transform.eulerAngles = GetEulerAngles(dir);
        }

        public static Vector3 GetEulerAngles(Direction dir)
        {
            Vector3 eulerAngles = Vector3.zero;
            switch (dir)
            {
                case Direction.Up:
                    eulerAngles = Vector3.zero;
                    break;

                case Direction.Down:
                    eulerAngles = new Vector3(0f, 180f, 0f);
                    break;

                case Direction.Left:
                    eulerAngles = new Vector3(0f, 270f, 0f);
                    break;

                case Direction.Right:
                    eulerAngles = new Vector3(0f, 90f, 0f);
                    break;
            }

            return eulerAngles;
        }

        public void SetPosition(TilePosition newTilePosition, Vector3 newPos)
        {
            Position = newTilePosition;
            transform.position = newPos;
        }

        private IEnumerator MoveToTile(TilePosition newTilePosition, Vector3 targetPos)
        {
            // Start movement
            /// Disable Action UI
            TurnAction.Instance.EnableActionableUI(false);

            /// Play move Animation
            PlayMoveAnimation();

            // Lerp transform.position from starting pos to target pos
            float timer = 0f;
            Vector3 startPos = transform.position;
            while (timer < runAnimDuration)
            {
                timer += Time.deltaTime;
                if (timer >= runAnimDuration)
                    break;

                transform.position = Vector3.Lerp(startPos, targetPos, timer / 0.8f);
                yield return null;
            }
            transform.position = targetPos;

            // After animation & Lerp ends,
            /// end move Animation
            StopMoveAnimation();

            // Set new positions
            SetPosition(newTilePosition, targetPos);

            /// reenable Action UI
            TurnAction.Instance.EnableActionableUI(true);
        }

        public int GetInventoryQuantity(ItemType type)
        {
            return playerInventory[type];
        }

        public void AddToInventory(ItemType type)
        {
            if (type != ItemType.NIL)
                playerInventory[type] += 1;
        }
        public void RemoveFromInventory(ItemType type)
        {
            if (playerInventory[type] > 0)
                playerInventory[type] -= 1;
        }
    }

}