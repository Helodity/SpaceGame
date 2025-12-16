using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject playerGameObject;
    PlayerMovement pMovement;
    PlayerShooting pShooting;
    PlayerHealth pHealth;
    [SerializeField] Slider shieldSlider;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider energySlider;

    // Start is called before the first frame update
    void Start()
    {
        pMovement = playerGameObject.GetComponent<PlayerMovement>();
        pShooting = playerGameObject.GetComponent<PlayerShooting>();
        pHealth = playerGameObject.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        energySlider.value = pShooting.GetEnergyRatio();
        healthSlider.value = pHealth.getHealthRatio();
        shieldSlider.value = pHealth.getShieldRatio();
    }
}
