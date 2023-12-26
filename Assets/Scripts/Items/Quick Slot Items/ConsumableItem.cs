using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class ConsumableItem : Item
    {
        [Header("ITEM QUANTITY")]
        public int maxItemAmount;
        public int currentItemAmount;

        [Header("ITEM MODEL")]
        public GameObject itemModel;

        [Header("ANIMATIONS")]
        public string consumeAnimation;
        public bool isInteracting;

        public virtual void AttemptToConsumeItem(PlayerManager player)
        {
            if (currentItemAmount > 0)
            {
                player.playerAnimatorManager.PlayTargetAnimation(consumeAnimation, isInteracting, true);
            }
            else
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerAnimatorManager.animation_cannot_cast, true);
            }
        }

        public virtual void SuccessfullyConsumeItem(PlayerManager player)
        {
            currentItemAmount -= 1;
        }

        public virtual bool CanIUseThisItem(PlayerManager player)
        {
            return true;
        }
    }
}