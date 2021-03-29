using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateGameobject : MonoBehaviour
{
    public GameObject objectToDeactivate;

    public void DeactivateDesiredGameObject()
    {
        objectToDeactivate.SetActive(false);
    }
}
