using UnityEngine;

public class TurnToPlayerMovement : MovementController
{
    [SerializeField] Rigidbody target;
    [SerializeField] float bulletSpeed;

    Rigidbody self;

    Vector3 leadTarget()
    {
        // Math from 
        // https://medium.com/andys-coding-blog/ai-projectile-intercept-formula-for-gaming-without-trigonometry-37b70ef5718b
        if(!self) self = GetComponent<Rigidbody>();

        Vector3 pA = target.transform.position - transform.position;
        Vector3 vA = target.velocity - self.velocity;

        float coeffA = Vector3.Dot(vA, vA) - bulletSpeed * bulletSpeed;
        float coeffB = 2 * Vector3.Dot(pA, vA);
        float coeffC = Vector3.Dot(pA, pA);

        float t1 = (-coeffB + Mathf.Sqrt((coeffB * coeffB) - 4 * coeffA * coeffC)) / (2 * coeffA);
        float t2 = (-coeffB - Mathf.Sqrt((coeffB * coeffB) - 4 * coeffA * coeffC)) / (2 * coeffA);

        if(!float.IsNaN(t1) && t1 >= 0) return getTargetAtTime(t1);
        if(!float.IsNaN(t2) && t2 >= 0) return getTargetAtTime(t2);

        return getTargetAtTime(0);
    }

    Vector3 getTargetAtTime(float duration)
    {
        return target.position + ((target.velocity - self.velocity) * duration);
    }

    protected override float getTargetAngle()
    {
        return Vector2.SignedAngle(Vector2.right, leadTarget() - transform.position);
    }

    protected override Vector3 getTargetPos()
    {
        return target.position + target.transform.up * 5;
    }
}
