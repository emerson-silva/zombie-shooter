using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    void TakeDamage(float damage);
    void Death();
    void ShowBlood(Quaternion rotation);
}
