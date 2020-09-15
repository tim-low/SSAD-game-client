using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class StateReference {

        public string UserToken { get; private set; }
        public GamePlayerManager PlayerManager { get; private set; }
        public GameBoard GameBoard { get; private set; }
        public GameVariables GlobalVariables { get; private set; }
        public TurnManager TurnManager { get; private set; }
        public string VictimToken { get; private set; }
        public TilePosition VictimTile { get; private set; }

        public StateReference(string token, GamePlayerManager pm, GameBoard gb, GameVariables gv, TurnManager tm, string victimToken, TilePosition victimTile)
        {
            UserToken = token;
            PlayerManager = pm;
            GameBoard = gb;
            GlobalVariables = gv;
            TurnManager = tm;
            VictimToken = victimToken;
            VictimTile = victimTile;
        }
    }

}