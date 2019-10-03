using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{

    private Animator chestAnim = null; //Animator for the chest;
    private GameObject player = null; //Player Object
    private Player playerScript = null;//Player Script
    private Inventory playerInventory = null;
    private CapsuleCollider capsuleCollider = null; // Capsule Collider
    private GameObject chestParticles = null;
    [SerializeField] int listIndex = 0;
    int openChest = 0;

    // Use this for initialization
    void Awake()
    {
        //get the Animator component from the chest;
        chestAnim = GetComponent<Animator>();
        //get the Player Component 
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponentInParent<Player>();
        playerInventory = player.GetComponent<Inventory>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        if(GetComponentInChildren<ParticleSystem>().gameObject != null)
        chestParticles = GetComponentInChildren<ParticleSystem>().gameObject;
        chestParticles.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //play open animation;  
            chestAnim.SetTrigger("open");
            openChest = 1;
            //Gets Random Coin ammount
            System.Random rand = new System.Random();
            int seed = rand.Next(1, 50);
            switch (seed)
            {
                case 1:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Health Potion"));
                    break;
                case 2:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Cone of Cold"));
                    break;
                case 3:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Damage Buff Potion"));
                    break;
                case 4:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Death Aura"));
                    break;
                case 5:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Defense Potion"));
                    break;
                case 6:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Fire Potion"));
                    break;
                case 7:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Fireball"));
                    break;
                case 8:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Hex Shot"));
                    break;
                case 9:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Ice Potion"));
                    break;
                case 10:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Love Potion"));
                    break;
                case 11:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Speed Potion"));
                    break;
                case 12:
                    playerInventory.AddConsumable(Resources.Load<Consumable>("Prefabs/PickUps/Lightning Bolt"));
                    break;
                default:
                    break;
            }
            //Player gets treasure
            playerScript.AddCoins(seed);
            //Not allow the player to cash out the chest again
            capsuleCollider.enabled = false;
            StartCoroutine(ShowParticle());
        }
    }

    IEnumerator ShowParticle()
    {
        chestParticles.SetActive(true);
        yield return new WaitForSeconds(1);
        chestParticles.SetActive(false);
    }

    public void ChestOpen()
    {
        chestAnim.SetTrigger("open");
        capsuleCollider.enabled = false;
    }

    public int GetListIndex()
    {
        return listIndex;
    }

    public int GetOpenChest()
    {
        return openChest;
    }

    public void SetOpenChest(int _open)
    {
        openChest = _open;
    }
}