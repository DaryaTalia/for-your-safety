using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBob : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    private Vector3 posOffset;
    private Vector3 tempPos;

    // Update is called once per frame
    void Update()
    {
        if(posOffset == null)
        {
            //posOffset = transform.position;
        }
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }
}
