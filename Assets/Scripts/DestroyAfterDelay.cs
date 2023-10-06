using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float DelayTime;

    private void Start()
    {
        Destroy(gameObject, DelayTime);
    }
}
