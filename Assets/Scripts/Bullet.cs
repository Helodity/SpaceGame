using UnityEngine;

public class Bullet : MonoBehaviour
{
    BulletData data;
    [SerializeField] ParticleSystem collisionParticlePrefab;
    Rigidbody rb;
    Vector3 lastPos;

    [Header("Visuals")]
    [SerializeField] Light l;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] float emissionIntensity;



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

    public void Init(Vector2 parentVelocity, Vector2 dir, BulletData settings, Color shipCol)
    {
        data = settings;
        rb = GetComponent<Rigidbody>();
        rb.velocity = (dir * settings.speed) + parentVelocity;
        InitVisuals(shipCol);
    }

    void InitVisuals(Color c)
    {
        l.color = c;
        foreach(Material m in mesh.materials)
        {
            m.SetColor("_EmissionColor", c * emissionIntensity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Bullet>() != null) return;
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
