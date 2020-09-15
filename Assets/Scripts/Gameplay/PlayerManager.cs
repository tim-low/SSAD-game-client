using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Networking;
using SuperSad.Networking.Events;

namespace SuperSad.Gameplay
{
    public class PlayerManager : EventListener {

        [SerializeField]
        private ChatWheel chatWheel;

        private Dictionary<string, BasePlayer> playersList; // key = player's token

        protected override void Init()
        {
            base.Init();
            playersList = new Dictionary<string, BasePlayer>();
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to SendKaomoji packet event
            notifier.Register(SendKaomojiAck, Packets.SendKaomojiAck);
        }

        private void SendKaomojiAck(Packet packet)
        {
            SendKaomojiAck ack = new SendKaomojiAck(packet);
            // Kaomoji react
            KaomojiReact(ack.Token, ack.KaomojiId);
        }
        public void KaomojiReact(string token, int reactId) // received event from packet
        {
            BasePlayer player = playersList[token];
            player.KaomojiReact(chatWheel.GetReact(reactId));
        }

        protected BasePlayer GetPlayer(string playerToken)
        {
            return playersList[playerToken];
        }

        protected void AddPlayer(string playerToken, BasePlayer player)
        {
            playersList.Add(playerToken, player);
        }
        protected void RemovePlayer(string playerToken)
        {
            playersList.Remove(playerToken);
        }

        protected bool ContainsPlayer(string playerToken)
        {
            return playersList.ContainsKey(playerToken);
        }

        /*
         * Player List Iteration
         */
        protected List<BasePlayer> GetPlayers()
        {
            List<BasePlayer> list = new List<BasePlayer>();
            foreach (string token in playersList.Keys)
                list.Add(playersList[token]);

            return list;
        }
    }

}