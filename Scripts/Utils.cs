using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public float GetAngleFromVector(float x, float y)
    {
        float angle = Mathf.Atan(x / y) * (180f / 3.1415f);

        //Going from 90' to 360'
        if (y < 0)
        {
            if (x < 0)
                angle += 180;
            else
                angle += 180;
        }
        else if (x < 0)
        {
            angle = 360 + angle;
        }
        return angle;

    }

    public float GetAngleDiference(float a1, float a2)
    {
        float angleDif = a1 - a2;

        if (angleDif > 180)
            angleDif -= 360;
        if (angleDif < -180)
            angleDif += 360;

        return Mathf.Abs(angleDif);
    }
}
