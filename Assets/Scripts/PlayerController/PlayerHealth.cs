using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] int maxHealth;
    [SerializeField] float healthRegen;
    float currentHealth;
    
    [Header("Effects")]
    [SerializeField] ShieldEffectPlayer shieldEffect;
    [SerializeField] ParticleSystem collisionParticlePrefab;

    Rigidbody rb;

    #region  Unity Events
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
    }
    void Update()
    {
        //Apply health regen
        modifyHealth(healthRegen * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        float amt = rb.velocity.magnitude;
        if(amt > 2)
        {
            playCollisionSFX(collision.GetContact(0).point);
            modifyHealth(-rb.velocity.magnitude);
        }
    }
    #endregion

    public void modifyHealth(float amt)
    {
        if(amt < 0)
        {
            playDamageSFX();
        }
        currentHealth += amt;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }


    #region Getters
    public float getHealthRatio()
    {
        return currentHealth / maxHealth;
    }
    #endregion

    #region VFX
    public void playDamageSFX()
    {
        shieldEffect.playAnim();
    }
    public void playCollisionSFX(Vector3 point)
    {
        ParticleSystem newSys = Instantiate(collisionParticlePrefab, point, Quaternion.identity);
        Destroy(newSys.gameObject, 0.1f);
    }
    #endregion

    #region IDamagable implementation
    public void takeDamage(int damage)
    {
        modifyHealth(-damage);
    }
    #endregion
}
