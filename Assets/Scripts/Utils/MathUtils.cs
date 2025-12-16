using UnityEngine;

public static class MathUtils
{

    //Its like sign but it can return 0
    public static float BetterSign(float val)
    {
        if(val == 0)
            return 0;

        return Mathf.Sign(val);
    }

    #region Normalize
    //its like normalize but returns 0 when Vector2.zero
    public static Vector2 BetterNormalize(Vector2 vec)
    {
        if(vec == Vector2.zero) return Vector2.zero;
        return vec.normalized;
    }
    //its like normalize but returns 0 when Vector3.zero
    public static Vector3 BetterNormalize(Vector3 vec)
    {
        if(vec == Vector3.zero) return Vector3.zero;
        return vec.normalized;
    }
    #endregion
    
    #region Easings
    public static float EaseIn(float t)
    {
        return 1 - Mathf.Cos(t * Mathf.PI / 2);
    }

    public static float EaseOut(float t)
    {
        return Mathf.Sin(t * Mathf.PI / 2);
    }
    #endregion

}
