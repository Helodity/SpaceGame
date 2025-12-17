using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPlayerMovement : MovementController
{
    [SerializeField] Rigidbody target;

    Vector3 leadTarget()
    {
        return target.position + target.velocity;
    }

    protected override float getTargetAngle()
    {
        return Vector2.SignedAngle(Vector2.right, target.position - transform.position);
    }

    protected override Vector3 getTargetPos()
    {
        return target.position;
    }
}
