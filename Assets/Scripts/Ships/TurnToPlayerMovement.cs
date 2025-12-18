using UnityEngine;

public class TurnToPlayerMovement : MovementController
{
    [SerializeField] Rigidbody target;
    [SerializeField] float bulletSpeed;

    Rigidbody self;

    void Awake()
    {
        self = GetComponent<Rigidbody>();
    }
    Vector3 leadTarget()
    {
        int iterations = 10;
        Vector3 location;
        float dist = (target.position - transform.position).magnitude;
        for(int i = 0; i < iterations; i++)
        {
            float dur = dist / bulletSpeed;
            location = getTargetAtTime(dur);
            dist = (location - transform.position).magnitude;
        }

        return getTargetAtTime(dist / bulletSpeed);
    }

    Vector3 getTargetAtTime(float duration)
    {
        self = GetComponent<Rigidbody>();
        return target.position + ((target.velocity - self.velocity) * duration);
    }

    protected override float getTargetAngle()
    {
        return Vector2.SignedAngle(Vector2.right, leadTarget() - transform.position);
    }

    protected override Vector3 getTargetPos()
    {
        return target.position;
    }
}
