using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipUI : MonoBehaviour
{
    [SerializeField] Transform trackingObject;
    Vector3 offset;
    PlayerHealth pHealth;
    [SerializeField] Slider shieldSlider;
    [SerializeField] Slider healthSlider;


    void Start()
    {
        pHealth = trackingObject.GetComponent<PlayerHealth>();
        offset = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.position = trackingObject.position + offset;
        healthSlider.value = pHealth.getHealthRatio();
        shieldSlider.value = pHealth.getShieldRatio();
    }
}
