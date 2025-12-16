using System;
using UnityEngine;

[Serializable]
public class InterpolatedValue
{
    public float interpolationSpeed;
    protected float curValue;
    protected float targetValue;

    public void Tick(float deltaTime)
    {
        float difference = Mathf.Ceil(targetValue - curValue);
        float toChange = difference * interpolationSpeed * deltaTime;
        toChange = Mathf.Clamp(toChange, Mathf.Sign(difference) * -difference, Mathf.Sign(difference) * difference);
        curValue += toChange;
    }

    public float GetValue()
    {
        return curValue;
    }

    public void SetValue(float val)
    {
        targetValue = val;
    }

    public static InterpolatedValue operator +(InterpolatedValue left, float right) {
        left.targetValue += right;
        return left;
    }
    public static InterpolatedValue operator -(InterpolatedValue left, float right) {
        left.targetValue -= right;
        return left;
    }
    public static InterpolatedValue operator *(InterpolatedValue left, float right) {
        left.targetValue *= right;
        return left;
    }
    public static InterpolatedValue operator /(InterpolatedValue left, float right) {
        left.targetValue /= right;
        return left;
    }
}
