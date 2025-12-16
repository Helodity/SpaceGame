using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjectScroller : MonoBehaviour
{
    [SerializeField] Transform RelativeObject;
    Vector3 initialObjectPos;
    [SerializeField] [Range(0,1)] float scrollScale;
    Vector3 initialOffset;
    // Start is called before the first frame update
    void Start()
    {
        initialOffset = transform.position;
        initialObjectPos = RelativeObject.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialOffset +
        (RelativeObject.position - initialObjectPos) * scrollScale;
    }
}
