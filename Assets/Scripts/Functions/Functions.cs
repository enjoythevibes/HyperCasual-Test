using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Functions
{
    public static float ToAngle(float value)
    {
        if (value <= 0f) value += 360f;
        if (value > 360f) value -= 360f;
        return value;
    }

    public static float ToSignedAngle(float value)
    {
        if (value > 180f) value -= 360f;
        if (value < -180f) value += 360f;
        return value;
    }

    public static Vector3 VerifyEulerAnglesVector(Vector3 value)
    {
        value.x = ToAngle(value.x);
        value.y = ToAngle(value.y);
        value.z = ToAngle(value.z);
        return value;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        var start = (min + max) * 0.5f - 180;
        var floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        min += floor;
        max += floor;
        return Mathf.Clamp(angle, min, max);
    }

    public static Vector3 Flat(this Vector3 vector)
    {
        vector.y = 0f;
        vector.Normalize();        
        return vector;
    }
}