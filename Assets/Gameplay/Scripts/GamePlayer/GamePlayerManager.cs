using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Networking;
using SuperSad.Networking.Events;
using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class GamePlayerManager : PlayerManager {

        [SerializeField]
        private GamePlayer playerPrefab;
        [SerializeField]
        private Text numStepsText;

        /// <summary>
        /// Create a Player object
        /// </summary>
        /// <param name="playerId">Player Id from 1 to 4 for color assignment</param>
        public void SpawnPlayer(Player player, Color color, Vector3 pos, TilePosition spawnPosition, Attributes attributes)
        {
            if (!ContainsPlayer(player.Token))
            {
                // Create GamePlayer object
                GamePlayer gamePlayer = Instantiate(playerPrefab);

                // Set GamePlayer world position
                gamePlayer.transform.position = pos;

                // Rotate player if at top of the board
                if (spawnPosition.y > 0)
                    //gamePlayer.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                    gamePlayer.Rotate(Direction.Down);

                // Set PlayerId
                gamePlayer.SetPlayerDetails(player, color, attributes);

                // Set TilePosition
                gamePlayer.Position = new TilePosition(spawnPosition.x, spawnPosition.y);

                // Update Player List
                AddPlayer(player.Token, gamePlayer);
            }
            else
            {
                Log.Instance.AppendLine(player.Username + " already exists");
            }
        }

        public GamePlayer GetGamePlayer(string token)
        {
            return (GamePlayer)GetPlayer(token);
        }

        public void SetPlayerPosition(string playerToken, TilePosition newTilePos, Vector3 newPos)
        {
            // Get Player
            GamePlayer player = (GamePlayer)GetPlayer(playerToken);
            // Set position
            player.SetPosition(newTilePos, newPos);
        }

        public bool PlayerHasStepsLeft(string playerToken)
        {
            GamePlayer player = (GamePlayer)GetPlayer(playerToken);
            return (player.Player.StepsLeft > 0);
        }

        public void MovePlayer(string playerToken, Direction moveDir, TilePosition newTilePosition, Vector3 targetPos, bool isTurnPlayer)
        {
            // Move Player
            GamePlayer player = (GamePlayer)GetPlayer(playerToken);
            if (player != null)
            {
                // Start Move animation
				AudioManager.instance.PlaySFX("Step");
                player.Move(moveDir, newTilePosition, targetPos);

                // Reduce number of steps if is turn player
                if (isTurnPlayer)
                {
                    int stepsLeft = player.Player.DecrementSteps();
                    UpdateNumberStepsText(stepsLeft);
                }
            }
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            base.Subscribe(notifier);

            // Subscribe to receive item reward packet event
            notifier.Register(GetQuizReward, Packets.SelectAnswerAck);

            // Subscribe to use item packet event
            notifier.Register(UseItemAck, Packets.TurnPlayerUseItemAck);

            // Subscribe to start player turn ack packet event
            notifier.Register(StartPlayerTurnAck, Packets.StartPlayerTurnAck);
            // Subscribe to notify player turn ack packet event
            notifier.Register(NotifyPlayerTurnAck, Packets.NotifyPlayerTurnAck);

            // Subscribe to despawn player ack packet event
            notifier.Register(DespawnPlayerAck, Packets.GameDespawnPlayerAck);
        }

        private void DespawnPlayerAck(Packet packet)
        {
            GameDespawnPlayerAck ack = new GameDespawnPlayerAck(packet);

            // Destroy GamePlayer
            GamePlayer despawned = (GamePlayer)GetPlayer(ack.Token);
            Destroy(despawned.gameObject);

            // Remove GamePlayer from list
            RemovePlayer(ack.Token);
        }

        private void StartPlayerTurnAck(Packet packet)
        {
            StartPlayerTurnAck ack = new StartPlayerTurnAck(packet);

            // Set number of steps
            SetNumberStepsForTurn(ack.Token);
        }
        private void NotifyPlayerTurnAck(Packet packet)
        {
            NotifyPlayerTurnAck ack = new NotifyPlayerTurnAck(packet);

            // Set number of steps
            SetNumberStepsForTurn(ack.Token);
        }
        private void SetNumberStepsForTurn(string userToken)
        {
            GamePlayer player = (GamePlayer)GetPlayer(userToken);
            int numSteps = player.Player.StepsLeft;
            UpdateNumberStepsText(numSteps);
        }
        

        private void GetQuizReward(Packet packet)
        {
            SelectAnswerAck ack = new SelectAnswerAck(packet);

            foreach (QuizReward reward in ack.Rewards)
            {
                // Get GamePlayer
                GamePlayer player = (GamePlayer)GetPlayer(reward.Token);

                if (reward.IsCorrect)
                {
                    // Add Item reward to inventory
                    player.AddToInventory(reward.ItemAwarded);
                }
                // Add number steps to player
                player.Player.SetSteps(reward.StepsAwarded);
            }
        }

        private void UseItemAck(Packet packet)  // received event from packet
        {
            TurnPlayerUseItemAck ack = new TurnPlayerUseItemAck(packet);

            // Subtract item from inventory
            GamePlayer player = (GamePlayer)GetPlayer(ack.Caster);
            player.RemoveFromInventory(ack.ItemIdentifier);

            // Update number of steps
            int stepsLeft = player.Player.DecrementSteps();
            UpdateNumberStepsText(stepsLeft);
        }

        public int GetItemQuantity(string token, int itemType)
        {
            GamePlayer player = (GamePlayer)GetPlayer(token);
            return player.GetInventoryQuantity((ItemType)itemType);
        }

        public void UpdateNumberStepsText(int newSteps)
        {
            // Update text display
            numStepsText.text = newSteps.ToString();
        }

        public bool HasPlayerAtTile(TilePosition pos, out string playerAtTile)
        {
            playerAtTile = "";  // default

            List<BasePlayer> players = GetPlayers();
            foreach (BasePlayer p in players)
            {
                GamePlayer player = (GamePlayer)p;
                if (player.Position == pos) // there is a Player at the tile
                {
                    playerAtTile = player.Player.Token;
                    return true;
                }
            }
            return false;
        }
    }

}