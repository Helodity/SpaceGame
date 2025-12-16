using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] int maxHealth;
    [SerializeField] int maxShield;
    [SerializeField] float shieldRegen;
    float currentShield;
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
        currentShield = maxShield;
    }
    void Update()
    {
        //Apply health regen
        modifyShield(shieldRegen * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        float amt = collision.relativeVelocity.magnitude;
        if(amt > 2)
        {
            playCollisionSFX(collision.GetContact(0).point);
            takeDamage(amt);
        }
    }
    #endregion

    void modifyShield(float amt)
    {
        currentShield += amt;
        if(currentShield <= 0)
        {
            modifyHealth(currentShield);
        }
        currentShield = Mathf.Clamp(currentShield, 0, maxShield);
    }

    void modifyHealth(float amt)
    {
        currentHealth += amt;
        if(currentHealth <= 0)
        {
            die();
        }
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void die()
    {
        gameObject.SetActive(false);
    }


    #region Getters
    public float getHealthRatio()
    {
        return currentHealth / maxHealth;
    }
    public float getShieldRatio()
    {
        return currentShield / maxShield;
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
    public void takeDamage(float damage)
    {
        playDamageSFX();
        modifyShield(-damage);
    }
    #endregion
}
