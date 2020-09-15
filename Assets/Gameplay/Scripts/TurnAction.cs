using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Networking.Events;
using SuperSad.Networking;
using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class TurnAction : EventListener {

        [SerializeField]
        private GameObject actionable;
        [SerializeField]
        private GameObject noMovesText;
        [SerializeField]
        private Text otherPlayerTurnText;
        [SerializeField]
        private GameVariables globalVariables;
        [SerializeField]
        private GamePlayerManager playerManager;
        [SerializeField]
        private InventoryUI inventoryUI;

        private static TurnAction instance = null;
        public static TurnAction Instance
        {
            get { return instance; }
        }

        private string userToken;
        public bool IsLocalClientTurn { get; private set; }

        protected override void Init()
        {
            base.Init();

            if (instance == null)
                instance = this;
            else
                Destroy(this);

            gameObject.SetActive(false);
            IsLocalClientTurn = false;

            // Disable player's turn text
            otherPlayerTurnText.gameObject.SetActive(false);
        }

        void OnEnable()
        {
            // Check if user has remaining moves left
            //noMovesText.SetActive(NoMoreMovesLeft());
            if (IsLocalClientTurn)
                CheckNoMoreMovesLeft();
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to Start Turn
            notifier.Register(ShowTurnUI, Packets.StartPlayerTurnAck);
            // Subscribe to Start Turn
            notifier.Register(ShowPartialTurnUI, Packets.NotifyPlayerTurnAck);

            // Subscribe to End Turn
            notifier.Register(EndPlayerTurn, Packets.EndPlayerTurnAck);
            // Subscribe to Start Quiz Ack (end of another player's turn)
            notifier.Register(EndPlayerTurn, Packets.StartQuizAck);
        }

        public void EnableActionableUI(bool enable)
        {
            actionable.SetActive(enable && IsLocalClientTurn);
            if (enable && IsLocalClientTurn)
            {
                // Check if user has remaining moves left
                CheckNoMoreMovesLeft();
                //    EndTurn();
            }
        }

        private void ShowTurnUI(Packet packet)
        {
            StartPlayerTurnAck ack = new StartPlayerTurnAck(packet);

            // Save Token
            userToken = ack.Token;
            Debug.Log("userToken:" + userToken);

            IsLocalClientTurn = true;

            // Enable UI
            gameObject.SetActive(true);
            actionable.SetActive(true);
			AudioManager.instance.PlaySFX ("UIPop");

            // Disable player's turn text
            otherPlayerTurnText.gameObject.SetActive(false);

            // Update number of moves player has
            CheckNoMoreMovesLeft();
        }

        private void ShowPartialTurnUI(Packet packet)
        {
            NotifyPlayerTurnAck ack = new NotifyPlayerTurnAck(packet);

            IsLocalClientTurn = false;

            // Save Token
            userToken = ack.Token;
            Debug.Log("userToken:" + userToken);

            // Enable UI
            gameObject.SetActive(true);
            // Disable Actionable
            actionable.SetActive(false);

            // Enable player's turn text
            otherPlayerTurnText.gameObject.SetActive(true);
            // Set Player turn's Text
            SetPlayerTurnText(ack.Token);

            // Update number of moves player has
            CheckNoMoreMovesLeft();
        }
        private void SetPlayerTurnText(string token)
        {
            Player player = playerManager.GetGamePlayer(token).Player;
            // Set Username
            otherPlayerTurnText.text = player.Username + "'s Turn";
            // Set Color
            otherPlayerTurnText.color = globalVariables.GetTileColor(player.Color);
        }

        private void EndPlayerTurn(Packet packet)
        {
            IsLocalClientTurn = false;

            // Disable UI (in case it is not already disabled)
            gameObject.SetActive(false);
        }

        private void CheckNoMoreMovesLeft()
        {
            Log.Instance.AppendLine("Checking if no more moves left");
            // Check if steps used up & item was used
            Debug.Log("userToken:" + userToken);
            bool noMoreMovesLeft = !playerManager.PlayerHasStepsLeft(userToken) && !inventoryUI.HasItemUseRemaining;
            noMovesText.SetActive(noMoreMovesLeft);
        }
    }

}