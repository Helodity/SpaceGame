using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] Transform bulletSpawnPos;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float bulletsPerSecond;
    [SerializeField] float shootKnockback;
    float shootDelay;

    [Header("Energy")]
    [SerializeField] float energyCost;
    [SerializeField] float energyRegen;

    [SerializeField] float maxEnergy;
    float currentEnergy;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(shootDelay <= 0 && currentEnergy > energyCost)
            {
                Bullet b = Instantiate(bulletPrefab, bulletSpawnPos.position, transform.rotation);
                b.Init(rb.velocity);
                rb.velocity += transform.right * shootKnockback;
                shootDelay = 1 / bulletsPerSecond;
                currentEnergy -= energyCost;
            }
        }
        shootDelay -= Time.deltaTime;

        currentEnergy += energyRegen * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }


    public float GetEnergyRatio()
    {
        return currentEnergy / maxEnergy;
    }
}
