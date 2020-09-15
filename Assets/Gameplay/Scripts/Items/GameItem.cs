using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public abstract class GameItem
    {
        public enum TypeOfTarget {
            None,
            Tile,
            Player
        }

        public TypeOfTarget TargetType { get; private set; }
        public string Description { get; private set; }

        public GameItem(TypeOfTarget targetType, string description)
        {
            TargetType = targetType;
            Description = description;
        }

        public abstract void UseEffect(StateReference refData);
    }

}