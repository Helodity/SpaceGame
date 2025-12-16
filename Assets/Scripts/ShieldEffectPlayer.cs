using UnityEngine;

public class ShieldEffectPlayer : MonoBehaviour
{
    [SerializeField] float maxAlpha;
    [SerializeField] float duration;
    float durationRemaining;
    Material shieldMat;

    void Awake()
    {
        shieldMat = GetComponent<MeshRenderer>().material;
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
        shieldMat.SetFloat("_Alpha", maxAlpha * val);

    }
    public void playAnim()
    {
        durationRemaining = duration;
    }
}
