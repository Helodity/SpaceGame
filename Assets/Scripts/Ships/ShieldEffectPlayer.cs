using UnityEngine;

public class ShieldEffectPlayer : MonoBehaviour
{
    [SerializeField] float maxAlpha;
    [SerializeField] float duration;
    float durationRemaining;
    Material[] materials;

    void Awake()
    {
        materials = GetComponent<MeshRenderer>().materials;
        durationRemaining = 0;
    }

    void Update()
    {
        durationRemaining -= Time.deltaTime;

        if(durationRemaining < 0)
        {
            durationRemaining = 0;
        }

        float val = MathUtils.EaseIn(durationRemaining / duration);
        foreach(Material m in materials)
        {
            m.SetFloat("_Alpha", maxAlpha * val);
        }

    }
    public void playAnim()
    {
        durationRemaining = duration;
    }
}
