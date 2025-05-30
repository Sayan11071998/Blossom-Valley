using System.Collections;
using UnityEngine;
using BlossomValley.PlayerSystem;

namespace BlossomValley.CharacterSystem
{
    public class CharacterRotationHandler
    {
        private MonoBehaviour coroutineRunner;
        private Transform characterTransform;
        private Quaternion defaultRotation;

        private bool isTurning = false;

        public CharacterRotationHandler(Transform transform, MonoBehaviour runner)
        {
            characterTransform = transform;
            defaultRotation = transform.rotation;
            coroutineRunner = runner;
        }

        public void LookAtPlayer()
        {
            Transform player = Object.FindAnyObjectByType<PlayerView>().transform;
            Vector3 direction = player.position - characterTransform.position;
            direction.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(direction);
            coroutineRunner.StartCoroutine(LookAt(lookRot));
        }

        public void ResetRotation() => coroutineRunner.StartCoroutine(LookAt(defaultRotation));

        private IEnumerator LookAt(Quaternion lookRot)
        {
            if (isTurning)
                isTurning = false;
            else
                isTurning = true;

            while (characterTransform.rotation != lookRot)
            {
                if (!isTurning) yield break;
                characterTransform.rotation = Quaternion.RotateTowards(characterTransform.rotation, lookRot, 720 * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }

            isTurning = false;
        }
    }
}