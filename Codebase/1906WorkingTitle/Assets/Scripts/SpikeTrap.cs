using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{



    public Animator spikeTrapAnim; //Animator for the SpikeTrap;
    bool onTrap;
    bool trapOn;
    bool playerHurt;
    GameObject player; //Player Object
    Player playerScript;//Player Script

    // Use this for initialization
    void Awake()
    {
        //get the Player Component 
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponentInParent<Player>();
        //get the Animator component from the trap;
        spikeTrapAnim = GetComponent<Animator>();
        //start opening and closing the trap for demo purposes;
        StartCoroutine(OpenCloseTrap());

    }

    private void FixedUpdate()
    {
        if ((onTrap && trapOn) && !playerHurt)
        {
            playerScript.TakeDamage(Color.red, 1);
            playerHurt = true;
            StartCoroutine(PlayerHurt());
        }
    }
    IEnumerator PlayerHurt()
    {
        yield return new WaitForSeconds(.5f);
        playerHurt = false;
    }
    IEnumerator OpenCloseTrap()
    {
        
        //play open animation;
        spikeTrapAnim.SetTrigger("open");
        trapOn = true;
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //play close animation;
        spikeTrapAnim.SetTrigger("close");
        trapOn = false;
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //Do it again;
        StartCoroutine(OpenCloseTrap());

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            onTrap = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTrap = false;
        }
    }


}