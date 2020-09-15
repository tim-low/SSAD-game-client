using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperSad.Networking;
using SuperSad.Networking.Events;

namespace SuperSad.Gameplay
{
    public class ItemEffectManager : EventListener {

        [SerializeField]
        private GameItemFactory gameItemFactory;
        [SerializeField]
        private GamePlayerManager playerManager;
        [SerializeField]
        private GameBoard gameBoard;
        [SerializeField]
        private GameVariables globalVariables;
        [SerializeField]
        private TurnManager turnManager;

        private Dictionary<ItemAnimation, ItemDuration> itemsNotDestroyed;

        protected override void Init()
        {
            base.Init();
            itemsNotDestroyed = new Dictionary<ItemAnimation, ItemDuration>();
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to use item packets
            notifier.Register(UseItemAck, Packets.TurnPlayerUseItemAck);

            // Subscribe to StartPlayerTurnAck & NotifyPlayerTurnAck for counting down turns
            notifier.Register(StartPlayerTurnEvent, Packets.StartPlayerTurnAck);
            notifier.Register(NotifyPlayerTurnEvent, Packets.NotifyPlayerTurnAck);

            // Subscribe to InitializeCycleAck for counting down cycles
            notifier.Register(InitializeCycleEvent, Packets.InitializeCycleAck);
        }

        private void StartPlayerTurnEvent(Packet packet)
        {
            StartPlayerTurnAck ack = new StartPlayerTurnAck(packet);
            TurnsCountdown(ack.Token);
        }
        private void NotifyPlayerTurnEvent(Packet packet)
        {
            NotifyPlayerTurnAck ack = new NotifyPlayerTurnAck(packet);

            // Count down turns for item destruction
            TurnsCountdown(ack.Token);
        }
        private void InitializeCycleEvent(Packet packet)
        {
            // Count down cycles for item destruction
            CyclesCountdown();
        }

        private void CyclesCountdown()
        {
            // Keep a temporary List of Keys to prevent list iteration failure if elements deleted
            List<ItemAnimation> tempItems = new List<ItemAnimation>();
            foreach (var item in itemsNotDestroyed.Keys) // iterate each Item not destroyed yet
                tempItems.Add(item);

            foreach (var item in tempItems)
            {
                switch (item.WhenToDestroy)
                {
                    case ItemAnimation.ToDestroy.EndOfCycle:
                        {
                            UpdateItemCountdown(item);
                        }
                        break;

                    default:    // other duration effects are to be ignored
                        break;
                }
            }
        }

        private void TurnsCountdown(string userToken)
        {
            // Keep a temporary List of Keys to prevent list iteration failure if elements deleted
            List<ItemAnimation> tempItems = new List<ItemAnimation>();
            foreach (var item in itemsNotDestroyed.Keys) // iterate each Item not destroyed yet
                tempItems.Add(item);

            foreach (var item in tempItems)
            {
                switch (item.WhenToDestroy)
                {
                    case ItemAnimation.ToDestroy.EndOfNextTurn: // always reduce regardless of turn type
                        {
                            UpdateItemCountdown(item);
                        }
                        break;

                    case ItemAnimation.ToDestroy.EndOfTargetPlayerTurn: // only reduce if turn type is the same
                        {
                            if (userToken == itemsNotDestroyed[item].VictimPlayer)
                            {
                                UpdateItemCountdown(item);
                            }
                        }
                        break;

                    default:    // other duration effects are to be ignored
                        break;
                }
            }
        }

        private void UpdateItemCountdown(ItemAnimation item)
        {
            itemsNotDestroyed[item].ReduceCountdown();
            if (itemsNotDestroyed[item].IsCountdownUp())    // countdown up; destroy
            {
                itemsNotDestroyed.Remove(item);
                Destroy(item.gameObject);
            }
        }

        public GameItem.TypeOfTarget GetTargetType(ItemType itemType)
        {
            GameItem item = gameItemFactory.GetGameItem(itemType);
            return item.TargetType;
        }

        public bool CanTargetSelf(ItemType itemType)
        {
            GameItem item = gameItemFactory.GetGameItem(itemType);
            GameItemTarget itemTarget = (GameItemTarget)item;

            return itemTarget.CanTargetSelf;
        }

        public string GetItemDescription(ItemType itemType)
        {
            GameItem item = gameItemFactory.GetGameItem(itemType);
            return item.Description;
        }

        private void UseItemAck(Packet packet)
        {
            TurnPlayerUseItemAck ack = new TurnPlayerUseItemAck(packet);

            // Use Item
            UseItemEffect(ack.Caster, ack.ItemIdentifier, ack.Victim, ack.Tile);
        }

        private void UseItemEffect(string userToken, ItemType itemType, string victimToken, TilePosition targetTile)
        {
            // Create Item
            GameItem item = gameItemFactory.GetGameItem(itemType);
            ItemAnimation itemAnim = gameItemFactory.CreateItemAnimation(itemType);

            StartCoroutine(UseItem(item, itemAnim, userToken, victimToken, targetTile));
        }

        private IEnumerator UseItem(GameItem item, ItemAnimation itemAnim, string userToken, string victimToken, TilePosition targetTile)
        {
            // Item Animation
            StateReference refData = new StateReference(userToken, playerManager, gameBoard, globalVariables, turnManager, victimToken, targetTile);
            itemAnim.InitAnimation(refData);
            itemAnim.StartAnimation();
            while (!itemAnim.AnimationCompleted)
                yield return null;

            switch (itemAnim.WhenToDestroy)
            {
                case ItemAnimation.ToDestroy.Immediate: // Destroy now
                    Destroy(itemAnim.gameObject);
                    break;
                case ItemAnimation.ToDestroy.EndOfNextTurn: // Destroy 2 start turns later
                    itemsNotDestroyed.Add(itemAnim, new ItemDuration(2));
                    break;
                case ItemAnimation.ToDestroy.EndOfTargetPlayerTurn:
                    itemsNotDestroyed.Add(itemAnim, new ItemDuration(1, victimToken));
                    break;
                case ItemAnimation.ToDestroy.EndOfCycle:    // Destroy 1 cycle later
                    itemsNotDestroyed.Add(itemAnim, new ItemDuration(1));
                    break;
                default:    // should not be here
                    break;
            }

            // Apply Effect
            item.UseEffect(refData);

            // Enable turn actions
            TurnAction.Instance.EnableActionableUI(true);
        }
    }

}