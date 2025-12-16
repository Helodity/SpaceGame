using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float forwardMoveSpeed;
    [SerializeField] float sideMoveSpeed;
    [SerializeField] float reverseMoveSpeed;
    [SerializeField] float maxMagnitude;
    [SerializeField] bool useDampeners;
    [SerializeField] bool mouseMovement;

    [Header("Rotation")]
    [SerializeField] float maxTurnSpeed = 100;
    [SerializeField] float maxTiltAngle = 30;
    [SerializeField] float rollScale;
    [SerializeField] float maxRollAngle;
    [SerializeField] InterpolatedValue curTurnSpeed;
    [SerializeField] InterpolatedValue xAngle;
    [SerializeField] InterpolatedValue yAngle;

    [Header("Visuals")]
    [SerializeField] List<ParticleSystem> forwardThrusters = new List<ParticleSystem>();
    [SerializeField] List<ParticleSystem> reverseThrusters = new List<ParticleSystem>();
    [SerializeField] List<ParticleSystem> leftThrusters = new List<ParticleSystem>();
    [SerializeField] List<ParticleSystem> rightThrusters = new List<ParticleSystem>();


    Rigidbody rb;

    #region Unity Events
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rotatePlayer();
        applyAcceleration();
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            useDampeners = !useDampeners;
        }
        
        Camera.main.gameObject.transform.position = 
            new Vector3(
                transform.position.x, 
                transform.position.y, 
                Camera.main.transform.position.z
            );
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, getForwardVector());

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.DrawWireSphere(mouseWorldPos, 1);
        if(rb)
        {
            Gizmos.color = Color.cyan;
            Vector2 accelVector = getAccelerationVector(useDampeners);
            Gizmos.DrawRay(transform.position, (Vector2)transform.TransformVector(accelVector));
        }
    }
    #endregion

    void rotatePlayer()
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


    Vector3 getForwardVector()
    {
        Vector3 vec = transform.right;
        vec.z = 0;
        return vec.normalized;
    }

    void applyAcceleration()
    {
        Vector2 moveVec = getAccelerationVector(useDampeners);

        updateThrusterParticles(moveVec);

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

    float getTargetAngle()
    {
        return Vector2.SignedAngle(Vector2.right, getMouseWorldPos() - transform.position);
    }

    float getCurrentAngle()
    {
        return Vector2.SignedAngle(Vector2.right, getForwardVector());
    }

    Vector3 getMouseWorldPos()
    {
        return Camera.main.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                transform.position.z - Camera.main.transform.position.z
            )
        );
    }


    Vector2 getAccelerationVector(bool applyDampeners)
    {
        Vector2 moveVec = Vector2.zero;
        Quaternion angle = Quaternion.AngleAxis(-getCurrentAngle(), Vector3.forward);
        if(mouseMovement)
        {
            if(Input.GetMouseButton(1))
            {
                Vector3 mouseWorldPos = getMouseWorldPos();
                moveVec = angle * (mouseWorldPos - transform.position);
                if(!applyDampeners)
                {
                    moveVec = moveVec.normalized;
                }
            }
        } else
        {
            moveVec = new Vector2(Input.GetAxisRaw("Vertical"), -Input.GetAxisRaw("Horizontal"));
        }
        
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
            moveVec.x *= moveVec.x < 0 ? reverseMoveSpeed : forwardMoveSpeed;
            moveVec.y *= sideMoveSpeed;
            return moveVec * Time.deltaTime;
        }
    }


    Vector2 getMaxMagnitude(Vector2 dir)
    {
        if(dir == Vector2.zero) return Vector2.zero;
        dir = dir.normalized;
        float angle = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.right, dir);
        float maxXspeed = Mathf.Cos(angle) < 0 ? reverseMoveSpeed : forwardMoveSpeed;
        return new Vector2(maxXspeed * Mathf.Cos(angle), sideMoveSpeed * Mathf.Sin(angle));
    }
}
