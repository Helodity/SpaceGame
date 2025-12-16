using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float shotsPerSecond;
    [SerializeField] BulletData bulletData;

    float reloadTime;

    [Header("Energy")]
    [SerializeField] float energyCost;


    void Update()
    {
        reloadTime -= Time.deltaTime;
    }

    public void shoot(Vector2 shipVelocity)
    {
        Bullet b = Instantiate(bulletPrefab, transform.position, transform.rotation);
        b.Init(shipVelocity, transform.right, bulletData);
        reloadTime = 1 / shotsPerSecond;
    }

    public float getEnergyCost()
    {
        return energyCost;
    }

    public bool canShoot(float currentEnergy)
    {
        return reloadTime <= 0 && energyCost <= currentEnergy;
    }

}
