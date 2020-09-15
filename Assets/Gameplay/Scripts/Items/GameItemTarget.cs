using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public abstract class GameItemTarget : GameItem
    {
        public bool CanTargetSelf
        {
            get; private set;
        }

        public GameItemTarget(TypeOfTarget targetType, string description, bool canTargetSelf) : base(targetType, description)
        {
            this.CanTargetSelf = canTargetSelf;
        }
    }
}