using SuperSad.Networking;
using SuperSad.Networking.Events;
using System;
using UnityEngine;

using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class GameVariables : EventListener {

        [Header("Component Controllers")]
        [SerializeField]
        private GameBoard gameBoard;
        [SerializeField]
        private GamePlayerManager playerManager;
        [SerializeField]
        private GameItemTargeter gameItemTargeter;

        [Header("Graphical User Interface")]
        [SerializeField]
        private UserListDisplay userListDisplay;

        [Header("Game Global Variables")]
        [SerializeField]
        protected int xLength;
        public int XLength
        {
            get { return xLength; }
        }
        [SerializeField]
        protected int yLength;
        public int YLength
        {
            get { return yLength; }
        }

        [Header("Game Preset Values")]
        [SerializeField]
        [Tooltip("Colours for players 1 to 4, 0 for unassigned")]
        private Color[] tileColorAssignment;
        [SerializeField]
        [Tooltip("Spawn positions for players 1 to 4")]
        private TilePosition[] spawnPositions;

        /*
         * Variables holding the Game State
         */
        //private Dictionary<string, int> playerList; // map client token to player Id
        //private Dictionary<string, GamePlayer> playersList; // mapping from token to GamePlayer (which also contains the Player data model)

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to initialize game packet event
            notifier.Register(InitializeGame, Packets.InitializeGameAck);

            // Subscribe to player move packet event
            notifier.Register(PlayerMoveAck, Packets.TurnPlayerMoveAck);
        }

        // Use this for initialization
        protected override void Init()
        {
            base.Init();
            gameBoard.InitGameBoard(xLength, yLength);

            //playersList = new Dictionary<string, GamePlayer>();
        }

        public Color GetTileColor(int tileState)
        {
            return tileColorAssignment[tileState];
        }

        public TilePosition GetSpawnPosition(int playerId)
        {
            return spawnPositions[playerId - 1];
        }

        private void InitializeGame(Packet packet)
        {
            InitializeGameAck ack = new InitializeGameAck(packet);

            Debug.Log("Number of Players: " + ack.PlayerSequence.Length);

            // if it's first time initializing,
            for (int i = 0; i < ack.PlayerSequence.Length; i++)
            {
                Player player = ack.PlayerSequence[i];

                Debug.Log("Spawn Player in GameVariables: " + player.Username + ", " + player.Token);

                /// call PlayerManager to spawn Player
                TilePosition spawnPos = GetSpawnPosition(player.Color);
                Color playerColor = GetTileColor(player.Color);
                playerManager.SpawnPlayer(player, playerColor, gameBoard.GetTilePos(spawnPos.x, spawnPos.y), spawnPos, ack.PlayerAttributes[i]);

                /// update game board spawn position colours
                gameBoard.SetTile(spawnPos.x, spawnPos.y, (GameBoard.TileState)player.Color, playerColor);

                /// update user list display UI
                userListDisplay.AddUserItem(player.Color, player.Username, playerColor);
            }

            /// store players as item targets
            gameItemTargeter.SetPlayerTargets();

        }

        public bool MoveCommand(Direction dir)
        {
            // Retrieve user token
            string userToken = UserState.Instance().Token;

            // Get tile position
            TilePosition playerTilePos = playerManager.GetGamePlayer(userToken).Position;

            Debug.Log("Posx: " + playerTilePos.x + " Posy: " + playerTilePos.y);
            Debug.Log("Direction: " + dir);

            // Check legality of move
            if (CheckMoveLegality(playerTilePos, dir) && playerManager.PlayerHasStepsLeft(userToken))
            {
                Log.Instance.AppendLine("Player Move");

                // Move player -- Don't move, wait for server to legalise movement
                //MovePlayer(userToken, dir, playerTilePos + dir);

                // Construct packet
                Packet cmd = new CmdTurnPlayerMove()
                {
                    Token = UserState.Instance().Token,
                    Direction = dir
                }.CreatePacket();

                // Send packet
                NetworkStreamManager.Instance.SendPacket(cmd);

                return true;
            }

            Log.Instance.AppendLine("Illegal Move");

            return false;
        }

        private void PlayerMoveAck(Packet packet)
        {
            TurnPlayerMoveAck ack = new TurnPlayerMoveAck(packet);

            // Check if move is legal
            if (ack.Legal)
            {
                // Move player
                MovePlayer(ack.Token, ack.Direction, ack.NewPos);
            }
            else
            {
                // Reenable Turn Action UI for turn player
                if (TurnAction.Instance.IsLocalClientTurn)
                    TurnAction.Instance.EnableActionableUI(true);
            }
        }

        private void MovePlayer(string token, Direction dir, TilePosition newPos)
        {
            Log.Instance.AppendLine("New Pos: x=" + newPos.x + ", y=" + newPos.y);

            // Get Turn Player
            GamePlayer turnPlayer = playerManager.GetGamePlayer(token);

            // Check if there is a Player at the target Tile
            string otherPlayer;
            if (playerManager.HasPlayerAtTile(newPos, out otherPlayer)) // Swap player
            {
                // Get position to swap to
                Vector3 swapPos = gameBoard.GetTilePos(turnPlayer.Position);
                Direction oppDir = GetOppDirection(dir);
                playerManager.MovePlayer(otherPlayer, oppDir, turnPlayer.Position, swapPos, false);

                Log.Instance.AppendLine("Turn Player Direction: " + dir);
                Log.Instance.AppendLine("Swapped Player Direction: " + oppDir);
            }

            // Move player & Update number of steps remaining
            Vector3 targetPos = gameBoard.GetTilePos(newPos);
            playerManager.MovePlayer(token, dir, newPos, targetPos, true);

            // Get move player's color
            int playerColor = turnPlayer.Player.Color;
            // Update tile state - check for Lock inside GameBoard
            gameBoard.SetTile(newPos, (GameBoard.TileState)playerColor, GetTileColor(playerColor));
        }

        private bool CheckMoveLegality(TilePosition tilePos, Direction moveDir)
        {
            switch (moveDir)
            {
                case Direction.Up:
                    if (tilePos.y >= yLength - 1)
                        return false;
                    break;

                case Direction.Down:
                    if (tilePos.y <= 0)
                        return false;
                    break;

                case Direction.Left:
                    if (tilePos.x <= 0)
                        return false;
                    break;

                case Direction.Right:
                    if (tilePos.x >= xLength - 1)
                        return false;
                    break;
            }

            return true;
        }

        private static Direction GetOppDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;

                default:    // Should not reach here
                    throw new Exception("Direction value " + dir + " is invalid");
            }
        }
    }

}