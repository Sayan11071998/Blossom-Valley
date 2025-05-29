using System.Collections;
using UnityEngine;

public class WorldBubble : MonoBehaviour
{

    [SerializeField] private Animator speechAnimator;

    private Transform cameraPos;

    public enum Emote
    {
        Happy, BadMood, Heart, Thinking, Sad
    }


    private void Start() => cameraPos = FindAnyObjectByType<CameraController>().transform;

    public void Display(Emote mood)
    {
        ResetAnimator();
        speechAnimator.SetBool(mood.ToString(), true);
    }

    public void Display(Emote mood, float time)
    {
        Display(mood);
        StartCoroutine(Delay(time));
    }

    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        ResetAnimator();
        gameObject.SetActive(false);
    }

    private void ResetAnimator()
    {
        foreach (AnimatorControllerParameter param in speechAnimator.parameters)
            speechAnimator.SetBool(param.name, false);
    }

    private void OnDisable() => ResetAnimator();

    private void Update() => transform.rotation = cameraPos.rotation;
}