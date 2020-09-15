using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class BasePlayer3D : BasePlayer {

        [SerializeField]
        protected Animator animator;
        [SerializeField]
        protected Customizer avatarCustomizer;

        // Use this for initialization
        void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();

            //yPos = transform.position.y;
        }

        protected void SetAvatar(Attributes attributes)
        {
            // Set clothes
            avatarCustomizer.ChangeSkin((int)Attributes.Parts.Head, attributes.Head);
            avatarCustomizer.ChangeSkin((int)Attributes.Parts.Top, attributes.Top);
            avatarCustomizer.ChangeSkin((int)Attributes.Parts.Bottom, attributes.Bottom);
            avatarCustomizer.ChangeSkin((int)Attributes.Parts.Shoes, attributes.Shoes);
        }

        public void PlayMoveAnimation(float playSpeed = 1f)
        {
            animator.SetTrigger("Move");
            animator.speed = playSpeed;
        }
        public void StopMoveAnimation()
        {
            animator.speed = 1f;    // reset speed
            animator.SetTrigger("EndMove");
        }

    }

}