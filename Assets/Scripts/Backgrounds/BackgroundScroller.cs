using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    Material ScrollMat;
    float ppu;
    [SerializeField] float scrollScale;
    [SerializeField] Vector2 initialOffset;
    [SerializeField] Vector2 scrollOverTime;
    
    // Start is called before the first frame update
    void Start()
    {
        ScrollMat = GetComponent<MeshRenderer>().material;
        ppu = 1.0f / transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newOffset = 
            (Vector3)initialOffset + 
            transform.position * ppu * scrollScale + 
            (Vector3)scrollOverTime * ppu * Time.time;
        ScrollMat.SetVector("_Offset", newOffset);
        ScrollMat.mainTextureOffset = newOffset;
    }
}
