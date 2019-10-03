using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    #region FireBallProperties
    [SerializeField] private GameObject childType = null;
    [SerializeField] private float explosionDamage = 2;
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (childType != null)
        {
            GameObject child = Instantiate(childType, transform.position, childType.transform.rotation);
            child.GetComponent<CreepManager>().SetParticleDamage(explosionDamage / 60f);
        }
    }
}