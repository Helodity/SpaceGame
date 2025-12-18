using System.Collections.Generic;
using UnityEngine;

public abstract class Shooting : MonoBehaviour
{
    [SerializeField] List<ShipWeapon> weapons;
    [Header("Energy")]
    [SerializeField] float maxEnergy;
    [SerializeField] float energyRegen;
    float currentEnergy;

    [Header("Visuals")]
    [SerializeField] [ColorUsage(false,false)] Color color;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] int matIndex;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentEnergy = maxEnergy;
        meshRenderer.materials[matIndex].color = new Color(
            color.r/color.maxColorComponent,
            color.g/color.maxColorComponent,
            color.b/color.maxColorComponent
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (wantToShoot())
        {
            tryShootWeapons();
        }
        applyEnergyRegen();
    }
    void tryShootWeapons()
    {
        foreach(ShipWeapon w in weapons)
        {
            if (w.canShoot(currentEnergy))
            {
                w.shoot(rb.velocity, color);
                currentEnergy -= w.getEnergyCost();
            }
        }
    }
    void applyEnergyRegen()
    {
        currentEnergy += energyRegen * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
    }

    #region Getters
    public float GetEnergyRatio()
    {
        return currentEnergy / maxEnergy;
    }
    #endregion

    protected abstract bool wantToShoot();
}
