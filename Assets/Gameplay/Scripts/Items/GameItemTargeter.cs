using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Networking;
using SuperSad.Networking.Events;
using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class GameItemTargeter : EventListener {

        [SerializeField]
        private GamePlayerManager playerManager;
        [SerializeField]
        private ItemEffectManager itemEffectManager;
        [SerializeField]
        private GameBoard gameBoard;
        [Header("UI Elements")]
        [SerializeField]
        private GameObject targetUI;
        [SerializeField]
        private Text targetText;
        [SerializeField]
        private string targetString = "Select a ";

        private Dictionary<GameItem.TypeOfTarget, List<ItemTarget>> itemTargets;

        /*
         * Selected Use Item
         */
        private ItemType selectedItem = ItemType.Total; // unassigned default
        private GameItem.TypeOfTarget targetType = GameItem.TypeOfTarget.None;  // unassigned default
        /*
         * Item Target Data
         */
        private bool canTargetSelf;
        private TilePosition targetTilePos;
        private string targetPlayer;

        protected override void Init()
        {
            base.Init();

            targetTilePos = new TilePosition(0, 0);
            targetPlayer = "12345678901234567890123456789012345678901234";
            ResetItemUse(); // set to default

            itemTargets = new Dictionary<GameItem.TypeOfTarget, List<ItemTarget>>();

            // Disable Target Text UI first
            targetUI.SetActive(false);
        }

        // Use this for initialization
        void Start() {

            // Add List of ItemTargets for TileSelect
            itemTargets.Add(GameItem.TypeOfTarget.Tile, new List<ItemTarget>());
            // Get GameTiles
            GameObject[] tempBoardTiles = GameObject.FindGameObjectsWithTag("GameTile");
            foreach (GameObject temp in tempBoardTiles)
            {
                ItemTarget itemTarget = temp.GetComponent<ItemTarget>();
                itemTarget.SetGameItemTargeter(this);
                itemTargets[GameItem.TypeOfTarget.Tile].Add(itemTarget);

                // disable it first as it is not needed yet
                itemTarget.enabled = false;
            }
        }

        public void SetPlayerTargets()
        {
            // Add List of ItemTargets for PlayerSelect
            itemTargets.Add(GameItem.TypeOfTarget.Player, new List<ItemTarget>());
            // Get Players
            GameObject[] tempPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject temp in tempPlayers)
            {
                ItemTarget itemTarget = temp.GetComponent<ItemTarget>();
                itemTarget.SetGameItemTargeter(this);
                itemTargets[GameItem.TypeOfTarget.Player].Add(itemTarget);

                // disable it first as it is not needed yet
                itemTarget.enabled = false;
            }
        }

        /// <summary>
        /// Called when an Inventory Slot is clicked; will only be called by the local client during their turn
        /// </summary>
        /// <param name="itemType">Item Type</param>
        /// <param name="targetType">Type of Item Target</param>
        public void UseItem(ItemType itemType, GameItem.TypeOfTarget targetType)
        {
            // Disable turn actions
            TurnAction.Instance.EnableActionableUI(false);

            if (targetType == GameItem.TypeOfTarget.None)
            {
                ConstructUseItemPacket(itemType);
            }
            else    // Target is required
            {
                // Save selected item
                selectedItem = itemType;
                canTargetSelf = itemEffectManager.CanTargetSelf(itemType);

                // Display Item Effect UI
                SetEnableTargets(targetType, true);
            }

        }

        /// <summary>
        /// Called when a tile or player target is selected
        /// </summary>
        /// <param name="updateData">Data on the targeted tile or player</param>
        public void SetItemTarget(object updateData)
        {
            // Retrieve item target data
            if (targetType == GameItem.TypeOfTarget.Tile)
            {
                targetTilePos = (TilePosition)updateData;
            }
            else if (targetType == GameItem.TypeOfTarget.Player)
            {
                targetPlayer = (string)updateData;
            }

            if (!canTargetSelf) // check if self was targeted if it's not allowed
            {
                if (targetType == GameItem.TypeOfTarget.Tile)
                {
                    int playerId = playerManager.GetGamePlayer(UserState.Instance().Token).Player.Color;
                    if (gameBoard.GetTileState(targetTilePos) == (GameBoard.TileState)playerId) // TEMPORARY
                        return;
                }
                else if (targetType == GameItem.TypeOfTarget.Player)
                {
                    if (targetPlayer == UserState.Instance().Token)
                        return;
                }
            }

            // Send use item packet
            ConstructUseItemPacket(selectedItem);
        }

        private void SetEnableTargets(GameItem.TypeOfTarget targetType, bool isEnabled)
        {
            this.targetType = targetType;

            List<ItemTarget> targets = itemTargets[targetType];
            if (targets == null)
                return;

            // enable/disable targets
            foreach (ItemTarget target in targets)
            {
                //Debug.Log("Enable " + target.gameObject.name + " to " + isEnabled);
                target.enabled = isEnabled;
            }

            // enable/disable target text UI
            targetUI.SetActive(isEnabled);
            if (isEnabled)
                targetText.text = targetString + targetType.ToString();
        }

        private void ConstructUseItemPacket(ItemType itemType)
        {
            // Construct packet
            Packet cmd = new CmdTurnPlayerUseItem()
            {
                ItemIdentifier = itemType,
                Victim = targetPlayer,
                Tile = new BoardTile(targetTilePos)
        }.CreatePacket();

            // Send packet
            NetworkStreamManager.Instance.SendPacket(cmd);

            // Let user player use item
            /*
             * inventoryUI.UseItem(itemType);
            itemEffectManager.UseItemEffect(TempUserListener.Instance.UserToken, itemType, this);
            */
            // Disable ItemTarget components
            DisableItemTargeting();
        }

        private void DisableItemTargeting()
        {
            if (targetType != GameItem.TypeOfTarget.None)
                SetEnableTargets(targetType, false);
            ResetItemUse();
        }

        private void ResetItemUse()
        {
            selectedItem = ItemType.Total;  // unassigned default
            targetType = GameItem.TypeOfTarget.None;  // unassigned default
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to EndPlayerTurnAck packet event
            notifier.Register(DisableItemTargeting, Packets.EndPlayerTurnAck);
        }
        private void DisableItemTargeting(Packet packet)
        {
            // Disable targeting components
            DisableItemTargeting();
        }

        public void CancelTargeting()
        {
            // Disable targeting components
            DisableItemTargeting();

            // Re-enable turn action UI
            TurnAction.Instance.EnableActionableUI(true);
        }
    }

}