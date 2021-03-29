using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    /*
     * 0 = bottom half
     * 1 = top half
    */

    public int FindHitHalf(float yHitPos)
    {
        float halfwayPoint = GetComponent<SpriteRenderer>().sprite.rect.y / 2;

        yHitPos -= transform.position.y;

        //Debug.Log("Collision Y Position: " + yHitPos + " Paddle Y Position: " + transform.position.y);

        if (yHitPos >= halfwayPoint)
            return 1;

        return 0;
    }
}