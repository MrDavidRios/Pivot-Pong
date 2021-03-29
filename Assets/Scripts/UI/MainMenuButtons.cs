using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    //Booleans
    public bool playSoundOnHover;

    //Animator
    private Animator anim;

    private void Awake()
    {
        // existing components on the GameObject
        AnimationClip clip;

        // new event created
        AnimationEvent evt;
        evt = new AnimationEvent();

        evt.intParameter = 1;

        evt.time = 1.0f;

        evt.functionName = "UpdateAnimationCompletionBool";

        // get the animation clip and add the AnimationEvent
        anim = GetComponent<Animator>();

        clip = anim.runtimeAnimatorController.animationClips[0];
        clip.AddEvent(evt);
    }

    public void UpdateAnimationCompletionBool(int introAnimationComplete)
    {
        if (introAnimationComplete == 1)
            anim.SetBool("isIntroAnimationComplete", true);
        else
            anim.SetBool("isIntroAnimationComplete", false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}