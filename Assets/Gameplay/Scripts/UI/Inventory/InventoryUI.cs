using UnityEngine;
using UnityEngine.UI;
using SuperSad.Networking;
using SuperSad.Networking.Events;

namespace SuperSad.Gameplay {
    public class InventoryUI : EventListener {

        [SerializeField]
        private ItemSet[] itemSets;
        [SerializeField]
        private Button previousSetButton;
        [SerializeField]
        private Button nextSetButton;

        [SerializeField]
        private GameItemTargeter gameItemTargeter;
        [SerializeField]
        private ItemEffectManager itemEffectManager;
        [SerializeField]
        private GamePlayerManager playerManager;

        private int currItemSet = 0;
        public bool HasItemUseRemaining { get; private set; }

        [SerializeField]
        private InventorySlot[] inventorySlots;
        private int numSlots = 6;

        private string currUser = "";

        protected override void Init()
        {
            base.Init();
            numSlots = inventorySlots.Length;
        }

        void OnEnable()
        {
            SetCurrItemSet(currItemSet);
        }

        void OnDisable()
        {
            for (int i = 0; i < itemSets.Length; i++)
            {
                if (itemSets[i] != null)
                    itemSets[i].gameObject.SetActive(false);
            }
        }

        public void ChangeItemSet(bool increment)   // if !increment, then -1
        {
            if (increment)
                SetCurrItemSet(currItemSet + 1);
            else
                SetCurrItemSet(currItemSet - 1);
        }

        private void SetCurrItemSet(int newValue)
        {
            // Update current item set
            currItemSet = newValue;
            for (int i = 0; i < itemSets.Length; i++)
            {
                itemSets[i].gameObject.SetActive(currItemSet == i);
            }

            UpdateInventorySlots();
        }

        private void UpdateInventorySlots()
        {
            Debug.Log("currUser: " + currUser);

            // Disable inventory if all steps used up
            HasItemUseRemaining = HasStepsRemaining(currUser);   // check if user has steps remaining

            // Update Inventory Slots
            for (int slotId = 0; slotId < numSlots; slotId++)
            {
                // Get new quantity
                int newQuantity = playerManager.GetItemQuantity(currUser, currItemSet * numSlots + slotId);

                // Set quantity Text
                inventorySlots[slotId].SetQuantityText(newQuantity);

                // Set Button interactivity based on whether there is item quantity available
                bool isInteractable = HasItemUseRemaining && (newQuantity > 0);
                inventorySlots[slotId].GetComponent<Button>().interactable = isInteractable;
                if (isInteractable)
                    RotateItem(slotId);
                else
                    StopRotateItem(slotId);
            }

            // Update Inventory UI
            previousSetButton.interactable = (currItemSet != 0);
            nextSetButton.interactable = (currItemSet != itemSets.Length - 1);
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to start turn packet
            /// to enable item use for the turn player
            notifier.Register(StartingATurnAck, Packets.StartPlayerTurnAck);
            // Subscribe to notify player turn packet -- other player's turn, inventoryUI not activated now; ignore
            /// to enable item use for the turn
            notifier.Register(NotifyPlayerTurnAck, Packets.NotifyPlayerTurnAck);

            // Subscribe to use item packet
            /// update item quantity on the UI & disable item slots
            notifier.Register(UseItemAck, Packets.TurnPlayerUseItemAck);
        }

        private void NotifyPlayerTurnAck(Packet packet)
        {
            NotifyPlayerTurnAck ack = new NotifyPlayerTurnAck(packet);

            //HasItemUseRemaining = HasStepsRemaining(ack.Token); // check if user is allowed to use any items
            currUser = ack.Token;
            Debug.Log("currUser: " + currUser);
        }

        private void StartingATurnAck(Packet packet)
        {
            StartPlayerTurnAck ack = new StartPlayerTurnAck(packet);

            //HasItemUseRemaining = HasStepsRemaining(ack.Token); // check if user is allowed to use any items
            currUser = ack.Token;
            Debug.Log("currUser: " + currUser);

            // Update Inventory slots
            SetCurrItemSet(currItemSet);
        }

        private void RotateItem(int slotId)
        {
            GameObject item = itemSets[currItemSet].GetItem(slotId);
            Rotate rotate = item.GetComponent<Rotate>();
            if (rotate != null)
                rotate.enabled = true;
        }

        private void StopRotateItem(int slotId)
        {
            GameObject item = itemSets[currItemSet].GetItem(slotId);
            Rotate rotate = item.GetComponent<Rotate>();
            if (rotate != null)
                rotate.enabled = false;
        }

        /// <summary>
        /// Inventory Slot Button Click
        /// </summary>
        /// <param name="slotId"></param>
        public void SelectUseItem(int slotId)
        {
            // Get details on item
            ItemType itemType = (ItemType)(currItemSet * numSlots + slotId);
            GameItem.TypeOfTarget displayType = itemEffectManager.GetTargetType(itemType);

            // Enable Item Targeting or use Item immediately
            gameItemTargeter.UseItem(itemType, displayType);
        }

        /// <summary>
        /// Called when local player uses an item to update the inventory interface
        /// </summary>
        private void UseItemAck(Packet packet)
        {
            Debug.Log("Inventory UI UseItem to reduce quantity");

            TurnPlayerUseItemAck ack = new TurnPlayerUseItemAck(packet);

            // Subtract from inventory
            //int itemId = (int)ack.ItemIdentifier;
            //playerManager.UseItemAck(TempUserListener.Instance.UserToken, itemId); // temp local user token
            /// PlayerManager MUST go before InventoryUI
            
            UpdateInventorySlots();

            // Update inventory slots quantity and usage
            //SetCurrItemSet(currItemSet);
            //UpdateInventorySlots();
            /*for (int i = 0; i < inventorySlots.Length; i++)
            {
                UpdateSlot(i);
            }*/
        }

        private bool HasStepsRemaining(string userToken)
        {
            int stepsLeft = playerManager.GetGamePlayer(userToken).Player.StepsLeft;
            return (stepsLeft != 0);
        }
    }

}