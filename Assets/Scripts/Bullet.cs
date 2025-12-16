using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletData data;
    [SerializeField] ParticleSystem collisionParticlePrefab;
    Rigidbody rb;
    Vector3 lastPos;
    void Awake()
    {
        lastPos = transform.position;
        data.lifetime = 5; //Something to just let it be set.
    }
    void Update()
    {
        data.lifetime -= Time.deltaTime;
        if(data.lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    void LateUpdate()
    {
        lastPos = transform.position;
    }

    public void Init(Vector2 parentVelocity, Vector2 dir, BulletData settings)
    {
        data = settings;
        rb = GetComponent<Rigidbody>();
        rb.velocity = (dir * settings.speed) + parentVelocity;
    }

    void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if(damagable != null)
        {
            damagable.takeDamage(data.damage);
        }

        ParticleSystem newSys = Instantiate(collisionParticlePrefab, other.ClosestPoint(lastPos), Quaternion.identity);
        Destroy(newSys.gameObject, 0.1f);
        Destroy(gameObject);
    }
}
