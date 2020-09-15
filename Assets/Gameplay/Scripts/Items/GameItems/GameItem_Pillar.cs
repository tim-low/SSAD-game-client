using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_Pillar : GameItem {

        public GameItem_Pillar(string description) : base(TypeOfTarget.None, description)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Pillar");

            // Get faced Direction
            GamePlayer gamePlayer = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            Direction faceDir = gamePlayer.DirectionFaced;
            TilePosition playerPos = gamePlayer.Position;

            int incrementer = 0;
            switch (faceDir)
            {
                case Direction.Right:
                case Direction.Up:
                    incrementer = 1;
                    break;
                case Direction.Left:
                case Direction.Down:
                    incrementer = -1;
                    break;
            }

            // assign all tiles along direction faced
            Color playerColor = refData.GlobalVariables.GetTileColor(gamePlayer.Player.Color);
            GameBoard.TileState playerTileState = (GameBoard.TileState)gamePlayer.Player.Color;
            if (faceDir == Direction.Up || faceDir == Direction.Down)
            {
                for (int y = playerPos.y; y >= 0 && y < refData.GlobalVariables.YLength; y += incrementer)
                {
                    refData.GameBoard.SetTile(playerPos.x, y, playerTileState, playerColor);
                }
            }
            else
            {
                for (int x = playerPos.x; x >= 0 && x < refData.GlobalVariables.XLength; x += incrementer)
                {
                    refData.GameBoard.SetTile(x, playerPos.y, playerTileState, playerColor);
                }
            }
        }
    }

}