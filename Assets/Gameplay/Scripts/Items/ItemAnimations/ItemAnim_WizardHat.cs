using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemAnim_WizardHat : ItemAnimation {

        [SerializeField]
        private float pauseDuration = 0.6f;
        [SerializeField]
        private float disappearDuration = 0.8f;
        [SerializeField]
        private string attachTo = "Head_end";
        [SerializeField]
        [Tooltip("Offset from transform to attach")]
        private Vector3 offset;

        [Header("Visual Effects")]
        [SerializeField]
        private GameObject wizardHatPoof;
        [SerializeField]
        private GameObject magicParticles;

        private Transform targetAttach;
        private GamePlayer user;
        private GamePlayer target;
        private bool userDisappeared = false;

        public override void InitAnimation(StateReference refData)
        {
            // Get user GamePlayer
            user = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            // Find Head
            targetAttach = user.transform.FindDeepChild(attachTo);

            // Set position to targetAttach
            //transform.SetParent(targetAttach);
            transform.position = targetAttach.position + offset;

            // Rotate hat to follow player's direction
            transform.eulerAngles = GamePlayer.GetEulerAngles(user.DirectionFaced);

            // Get target GamePlayer
            target = refData.PlayerManager.GetGamePlayer(refData.VictimToken);
        }

        void LateUpdate()
        {
            // Update position to player's head
            if (!userDisappeared)
                transform.position = targetAttach.position + offset;
        }

        protected override IEnumerator Animate()
        {
            userDisappeared = false;
            // Instantiate WizardHat 'poof' particles
            GameObject vfx = Instantiate(wizardHatPoof);
            vfx.transform.position = transform.position;


            // Allow Wizard Hat to appear for a while
            yield return new WaitForSeconds(pauseDuration);

            // Make players disappear
			AudioManager.instance.PlaySFX("WizardHat"); //sfx
            user.gameObject.SetActive(false);
            target.gameObject.SetActive(false);
            // Set WizardHat to disappear
            userDisappeared = true;
            transform.position = new Vector3(1000f, 1000f, 1000f);  // faraway position
            // Instantiate "magic" particles
            GameObject vfx1 = Instantiate(magicParticles);
            Vector3 userHeadPos = user.transform.FindDeepChild(attachTo).position;
            vfx1.transform.position = userHeadPos;
            GameObject vfx2 = Instantiate(magicParticles);
            Vector3 targetHeadPos = target.transform.FindDeepChild(attachTo).position;
            vfx2.transform.position = targetHeadPos;
            // Set to wait
            yield return new WaitForSeconds(0.8f * disappearDuration);

            // Instantiate "magic" particles
            vfx1 = Instantiate(magicParticles);
            vfx1.transform.position = userHeadPos;
            vfx2 = Instantiate(magicParticles);
            vfx2.transform.position = targetHeadPos;
            // finish the delay
            yield return new WaitForSeconds(0.2f * disappearDuration);

            // Make players reappear
            user.gameObject.SetActive(true);
            target.gameObject.SetActive(true);

            CompleteAnimation();
        }
    }

}