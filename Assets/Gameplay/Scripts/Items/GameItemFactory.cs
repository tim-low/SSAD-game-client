using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public enum ItemType
    {
        NIL = -1,

        Crown = 0,  // assign all tiles surrounding player
        Flag,   // select 1 tile to assign to player; won't select a tile that is the player's
        Lock,   // lock all of your tiles from being overwritten in the next turn
        Pillar, // assign all tiles along the direction the player is currently facing
        GroundSpike,    // make a tile inaccessible for 1 turn cycle
        SelfDestruct,   // set tiles surrounding player to unassigned

        Coin,   // gain 1 extra step
        Cage,   // skip a player's next turn
        BoulderSpike,   // return a player to their spawn point
        Bed,    // return back to own spawn point
        Door,   // teleport to a random spawn point
        WizardHat,  // swap places with another player

        Total
    }

    public class GameItemFactory : MonoBehaviour {

        [SerializeField]
        private string descriptionsResourcePath;

        private List<GameItem> items;

        void Awake()
        {
            // Read item descriptions
            string[] descriptions = ReadDescriptions();

            // Create GameItems
            items = new List<GameItem>();

            items.Add(new GameItem_Crown(descriptions[0]));
            items.Add(new GameItem_Flag(descriptions[1]));
            items.Add(new GameItem_Lock(descriptions[2]));
            items.Add(new GameItem_Pillar(descriptions[3]));
            items.Add(new GameItem_GroundSpike(descriptions[4]));
            items.Add(new GameItem_SelfDestruct(descriptions[5]));

            items.Add(new GameItem_Coin(descriptions[6]));
            items.Add(new GameItem_Cage(descriptions[7]));
            items.Add(new GameItem_BoulderSpike(descriptions[8]));
            items.Add(new GameItem_Bed(descriptions[9]));
            items.Add(new GameItem_Door(descriptions[10]));
            items.Add(new GameItem_WizardHat(descriptions[11]));
        }

        private string[] ReadDescriptions()
        {
            TextAsset descData = Resources.Load(descriptionsResourcePath) as TextAsset;

            string[] descriptions = descData.text.Split('\n');
            return descriptions;
        }

        public GameItem GetGameItem(ItemType itemType)
        {
            Debug.Log(itemType);
            if (itemType < 0 || itemType > ItemType.Total)
                return null;

            // Get the GameItem type
            return items[(int)itemType];
        }

        public ItemAnimation CreateItemAnimation(ItemType itemType)
        {
            string path = "Items/" + itemType.ToString();
            Debug.Log(path);
            ItemAnimation item = Instantiate(Resources.Load<ItemAnimation>(path));
            return item;
        }
    }

}