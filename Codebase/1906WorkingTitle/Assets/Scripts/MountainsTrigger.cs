using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainsTrigger : MonoBehaviour
{
    #region MountainsTriggerProperties
    //Obstacle
    [SerializeField] private GameObject iceBarrier = null;
    //Particle effect for the fire
    [SerializeField] private GameObject fireOne = null;
    bool active;
    Player playerSave = null;
    #endregion

    private void Start()
    {
        playerSave = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // the coroutine wont start unless triggered by the fire bullet, ice bullet and normal bullet will not have an effect on the barrier
        if (other.tag == "Fire Bullet" && !active)
        {
            StartCoroutine(BurnPath());
            active = true;
            playerSave.iceWall = 1;
        }
    }

    //Coroutine to melt the ice
    IEnumerator BurnPath()
    {
        fireOne.SetActive(true);
        yield return new WaitForSeconds(2);
        iceBarrier.SetActive(false);
        fireOne.SetActive(false);
    }
}
