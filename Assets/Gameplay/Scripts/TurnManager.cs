using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SuperSad.Networking;
using SuperSad.Networking.Events;

using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class TurnManager : EventListener {

        [Header("Game Managers")]
        [SerializeField]
        private GameVariables globalVariables;
        [SerializeField]
        private GamePlayerManager playerManager;

        [Header("Graphical User Interface")]
        [SerializeField]
        private TurnSequenceDisplay turnSequenceDisplay;
        [SerializeField]
        private GameObject turnActionUI;
        [SerializeField]
        private CountdownTimer turnTimer;
        [SerializeField]
        private Text currentTurnText;

        private Dictionary<string, bool> playerIsLocked;    // key = player token

        public int TotalTurns { get; private set; }
        private int currTurn = 0;

        public override void Subscribe(INotifier<Packet> notifier)
        {
            /*
             * Initialization
             */
            // Subscribe to initialize game ack packet event
            notifier.Register(InitializeGame, Packets.InitializeGameAck);
            // Subscribe to Initialize Cycle Ack packet event
            notifier.Register(InitializeCycle, Packets.InitializeCycleAck);
            // Subscribe to Start Quiz Ack packet event
            notifier.Register(IncrementTurnCount, Packets.StartQuizAck);

            /*
             * Start/End Turn
             */
            // Subscribe to EndPlayerTurnAck packet event
            notifier.Register(EndTurnAck, Packets.EndPlayerTurnAck);
            // Subscribe to StartPlayerTurnAck packet event
            notifier.Register(StartPlayerTurnAck, Packets.StartPlayerTurnAck);
            // Subscribe to NotifyPlayerTurnAck packet event
            notifier.Register(NotifyPlayerTurnAck, Packets.NotifyPlayerTurnAck);

            // Subscribe to StartQuizAck packet event (end of a cycle)
            notifier.Register(EndCycleAck, Packets.StartQuizAck);

            // Subscribe to Game Despawn Player Ack packet event
            notifier.Register(DespawnPlayerAck, Packets.GameDespawnPlayerAck);
        }

        private void IncrementTurnCount(Packet packet)
        {
            Debug.Log("INCREMENT, Current turn = " + currTurn);

            // Increment
            ++currTurn;

            // Update display
            currentTurnText.text = currTurn + "/" + TotalTurns;
        }

        protected override void Init()
        {
            base.Init();
            playerIsLocked = new Dictionary<string, bool>();
        }

        public void LockPlayer(string userToken)
        {
            Debug.Log("Lock!!");
            if (!playerIsLocked.ContainsKey(userToken))
            {
                Debug.Log("Create Player Key: " + userToken);
                playerIsLocked.Add(userToken, true);
            }
            else
            {
                Debug.Log("Player Key already exists: " + userToken);
                playerIsLocked[userToken] = true;
            }
        }

        private void UnlockPlayer(string userToken)
        {
            playerIsLocked[userToken] = false;
        }

        private bool IsPlayerLocked(string userToken)
        {
            if (playerIsLocked.ContainsKey(userToken))
            {
                Debug.Log("Player Key exists");
                return playerIsLocked[userToken];
            }
            return false;
        }

        private void EndCycleAck(Packet packet)
        {
            // Empty turn sequence display
            turnSequenceDisplay.EmptyQueue();
        }

        public void InitializeCycle(Packet packet)
        {
            // At the start of a turn cycle (after answering a question)
            InitializeCycleAck ack = new InitializeCycleAck(packet);

            /// update turn sequence
            UpdateTurnSequence(ack.PlayerSequence);
        }

        private void InitializeGame(Packet packet)
        {
            // Only when game first initializes
            InitializeGameAck ack = new InitializeGameAck(packet);

            /// update turn sequence
            UpdateTurnSequence(ack.PlayerSequence);

            /// set no. turns for the game, to display currTurn / (total no. turns)
            //turnSequenceDisplay.SetNumberTurns(ack.NumberTurns);
            TotalTurns = ack.NumberTurns;
            currTurn = 0;
        }

        private void UpdateTurnSequence(Player[] playerSequence)
        {
            TurnData[] turnSequence = new TurnData[playerSequence.Length];

            for (int i = 0; i < playerSequence.Length; i++)
            {
                Player player = playerSequence[i];
                // Read from PlayerManager as Player data model is the color of the player position's tile
                int playerColor = playerManager.GetGamePlayer(player.Token).Player.Color;

                /// update turn sequence & UI for turn display
                turnSequence[i] = new TurnData(player.Token, player.Username, globalVariables.GetTileColor(playerColor));
            }
            /// update turn sequence & UI for turn display
            turnSequenceDisplay.SetTurnSequence(turnSequence);
        }

        private void DespawnPlayerAck(Packet packet)
        {
            GameDespawnPlayerAck ack = new GameDespawnPlayerAck(packet);

            // Update Turn Sequence Queue
            turnSequenceDisplay.RemovePlayerFromQueue(ack.Token);
        }

        private void EndTurnAck(Packet packet)
        {
            Log.Instance.AppendLine("End turn");
            //EndPlayerTurnAck ack = new EndPlayerTurnAck(packet);

            // End current turn (if it's server-initiated)
            DisableTurnUI();
        }

        private void NotifyPlayerTurnAck(Packet packet)
        {
            NotifyPlayerTurnAck ack = new NotifyPlayerTurnAck(packet);
            Log.Instance.AppendLine("Next player's turn: " + ack.Token);

            // Show next player's turn
            turnSequenceDisplay.GoToNextTurn();

            // Update player locked
            UpdatePlayerLocked(ack.Token);

            // Set timer
            turnTimer.StartTimer(ack.Duration, false);
        }

        private void StartPlayerTurnAck(Packet packet)
        {
            StartPlayerTurnAck ack = new StartPlayerTurnAck(packet);

            // Update turn sequence & UI for turn display
            turnSequenceDisplay.GoToNextTurn();

            // Check if start turn player is locked here
            if (UpdatePlayerLocked(ack.Token))
            {
                // End turn immediately
                EndTurn();
            }
            else
            {
                // Set timer
                turnTimer.StartTimer(ack.Duration, true);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userToken">Return is locked</param>
        /// <returns></returns>
        private bool UpdatePlayerLocked(string userToken)
        {
            bool isLocked = IsPlayerLocked(userToken);
            Log.Instance.AppendLine("Is Player locked: " + isLocked);
            // Check if start turn player is locked here
            if (isLocked)
            {
                // Unlock player
                UnlockPlayer(userToken);
            }

            return isLocked;
        }

        public void EndTurn()
        {
            // Disable UI
            DisableTurnUI();

            // Construct packet
            Packet cmd = new CmdEndTurn()
            {
                Token = UserState.Instance().Token
            }.CreatePacket();

            // Send packet
            NetworkStreamManager.Instance.SendPacket(cmd);
        }

        private void DisableTurnUI()
        {
            // Disable UI
            turnActionUI.SetActive(false);
        }
    }

}