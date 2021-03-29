using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWithFade : MonoBehaviour
{
    public void FadeOut() 
    {
        GetComponent<Animator>().SetBool("FadeOut", true);
    }
}
