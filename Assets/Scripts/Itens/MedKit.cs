using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    public float HealAmount;
    public float DestroyTimer;

    private void Start()
    {
        if (DestroyTimer > 0)
        {
            Destroy(gameObject, DestroyTimer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.PLAYER))
        {
            other.gameObject.GetComponent<Player>().Heal(HealAmount);
            Destroy(gameObject);
        }
    }
}
