using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float forwardMoveSpeed;
    [SerializeField] protected float sideMoveSpeed;
    [SerializeField] protected float reverseMoveSpeed;
    [SerializeField] protected float maxMagnitude;
    [SerializeField] protected bool useDampeners;

    [Header("Rotation")]
    [SerializeField] protected float maxTurnSpeed = 100;
    [SerializeField] protected float maxTiltAngle = 30;
    [SerializeField] protected float rollScale;
    [SerializeField] protected float maxRollAngle;
    [SerializeField] protected InterpolatedValue curTurnSpeed;
    [SerializeField] protected InterpolatedValue xAngle;
    [SerializeField] protected InterpolatedValue yAngle;

    [Header("Visuals")]
    [SerializeField] protected List<ParticleSystem> forwardThrusters = new List<ParticleSystem>();
    [SerializeField] protected List<ParticleSystem> reverseThrusters = new List<ParticleSystem>();
    [SerializeField] protected List<ParticleSystem> leftThrusters = new List<ParticleSystem>();
    [SerializeField] protected List<ParticleSystem> rightThrusters = new List<ParticleSystem>();


    protected Rigidbody rb;

    #region Unity Events
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void Update()
    {
        applyRotation();
        Vector2 moveVec = getAccelerationVector(useDampeners);
        updateThrusterParticles(moveVec);
        applyAcceleration(moveVec);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, getForwardVector());

        Gizmos.DrawWireSphere(getTargetPos(), 1);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(Vector3.forward * getTargetAngle()) * Vector3.right * 4);
        if(rb)
        {
            Gizmos.color = Color.cyan;
            Vector2 accelVector = getAccelerationVector(useDampeners);
            Gizmos.DrawRay(transform.position, (Vector2)transform.TransformVector(accelVector));
        }
    }
    #endregion

    void applyRotation()
    {
        float delta = Mathf.DeltaAngle(getCurrentAngle(), getTargetAngle());
        curTurnSpeed.SetValue(MathUtils.BetterSign(delta) * maxTurnSpeed);
        curTurnSpeed.Tick(Time.deltaTime);

        float accel = (getAccelerationVector(useDampeners) / Time.deltaTime).x;
        yAngle.SetValue(MathUtils.BetterSign(accel) * maxTiltAngle);
        yAngle.Tick(Time.deltaTime);

        float zAngle = Mathf.MoveTowardsAngle(getCurrentAngle(), getTargetAngle(), Mathf.Abs(curTurnSpeed.GetValue()) * Time.deltaTime);
        float angleDiff = Mathf.Clamp(Mathf.DeltaAngle(getCurrentAngle(), getTargetAngle()), -maxRollAngle, maxRollAngle);
        xAngle.SetValue(angleDiff * rollScale);
        xAngle.Tick(Time.deltaTime);
        transform.rotation = Quaternion.identity;
        transform.Rotate(Vector3.right * xAngle.GetValue(), Space.World);
        transform.Rotate(Vector3.up * yAngle.GetValue(), Space.World);
        transform.Rotate(Vector3.forward * zAngle, Space.World);
    }
    void applyAcceleration(Vector2 moveVec)
    {
        rb.velocity += transform.TransformVector(moveVec);

        //Limit magnitude from acceleration
        if(rb.velocity.magnitude > maxMagnitude)
        {
            rb.velocity = rb.velocity.normalized * maxMagnitude;
        }
    }
    void updateThrusterParticles(Vector2 moveDir)
    {
        float x = MathUtils.BetterSign(moveDir.x);
        switch(x)
        {
            case 1:
                forwardThrusters.ForEach(x => x.Play());
                reverseThrusters.ForEach(x => x.Stop());
                break;
            case -1:
                forwardThrusters.ForEach(x => x.Stop());
                reverseThrusters.ForEach(x => x.Play());
                break;
            default:
                forwardThrusters.ForEach(x => x.Stop());
                reverseThrusters.ForEach(x => x.Stop());
                break;
        }
        float y = moveDir.y == 0 ? 0 : Mathf.Sign(moveDir.y);
        switch(y)
        {
            case 1:
                leftThrusters.ForEach(x => x.Play());
                rightThrusters.ForEach(x => x.Stop());
                break;
            case -1:
                leftThrusters.ForEach(x => x.Stop());
                rightThrusters.ForEach(x => x.Play());
                break;
            default:
                leftThrusters.ForEach(x => x.Stop());
                rightThrusters.ForEach(x => x.Stop());
                break;
        }
    }

    #region Getters
    protected Vector3 getForwardVector()
    {
        Vector3 vec = transform.right;
        vec.z = 0;
        return vec.normalized;
    }

    protected float getCurrentAngle()
    {
        return Vector2.SignedAngle(Vector2.right, getForwardVector());
    }

    protected Vector2 getAccelerationVector(bool applyDampeners)
    {
        Quaternion angle = Quaternion.AngleAxis(-getCurrentAngle(), Vector3.forward);
        Vector2 moveVec = angle * (getTargetPos() - transform.position);
        
        if(applyDampeners)
        {
            Vector2 targetSpeed = moveVec == Vector2.zero ? Vector2.zero : moveVec.normalized * maxMagnitude;
            Vector2 curSpeed = angle * rb.velocity;
            Vector2 thrustDir = (targetSpeed - curSpeed).normalized;
            Vector2 maxAccel = getMaxMagnitude(thrustDir) * Time.deltaTime;

            Vector2 speedOffset = targetSpeed - curSpeed;
            if(targetSpeed == Vector2.zero && speedOffset.magnitude < maxAccel.magnitude)
            {
                return speedOffset;
            }

            return maxAccel;
        } else
        {
            moveVec = moveVec.normalized;
            moveVec.x *= moveVec.x < 0 ? reverseMoveSpeed : forwardMoveSpeed;
            moveVec.y *= sideMoveSpeed;
            return moveVec * Time.deltaTime;
        }
    }

    protected Vector2 getMaxMagnitude(Vector2 dir)
    {
        if(dir == Vector2.zero) return Vector2.zero;
        dir = dir.normalized;
        float angle = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.right, dir);
        float maxXspeed = Mathf.Cos(angle) < 0 ? reverseMoveSpeed : forwardMoveSpeed;
        return new Vector2(maxXspeed * Mathf.Cos(angle), sideMoveSpeed * Mathf.Sin(angle));
    }

    #endregion

    #region Abstract Functions
    protected abstract Vector3 getTargetPos();
    protected abstract float getTargetAngle();
    #endregion
}
