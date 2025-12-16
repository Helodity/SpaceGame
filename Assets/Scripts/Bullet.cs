using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] ParticleSystem collisionParticlePrefab;
    Rigidbody rb;
    Vector3 lastPos;
    float lifetime = 5;
    void Awake()
    {
        lastPos = transform.position;
    }
    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    void LateUpdate()
    {
        lastPos = transform.position;
    }

    public void Init(Vector2 parentVelocity)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = (Vector2)(transform.right * bulletSpeed) + parentVelocity;
        lifetime = 1;
    }

    void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if(damagable != null)
        {
            damagable.takeDamage(damage);
        }

        ParticleSystem newSys = Instantiate(collisionParticlePrefab, other.ClosestPoint(lastPos), Quaternion.identity);
        Destroy(newSys.gameObject, 0.1f);
        Destroy(gameObject);
    }
}
