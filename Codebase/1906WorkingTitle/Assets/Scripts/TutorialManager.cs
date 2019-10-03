using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject player = null;
    private Player playerScript = null;
    [SerializeField] private GameObject spawner = null;
    private SpawnScript spawnScript = null;
    [SerializeField] private GameObject[] popUps = null;
    private GameObject door;
    [SerializeField] GameObject pickup = null;
    private Inventory inventoryScript = null;
    [SerializeField] GameObject shop = null;
    int popUpIndex = 0;
    bool pressF;

    public void Start()
    {
        spawnScript = spawner.GetComponent<SpawnScript>();
        playerScript = player.GetComponent<Player>();
        inventoryScript = player.GetComponent<Inventory>();
        door = GameObject.FindGameObjectWithTag("Door");
        spawner.SetActive(false);
        spawnScript.SetEnabled(false);
    }

    private void Update()
    {
        //Keeps track of what popup is on and which ones are off
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
                popUps[i].SetActive(true);
            else
                popUps[i].SetActive(false);
        }
        PopUpConditions();
    }

    void PopUpConditions()
    {
        //Movement 
        if (popUpIndex == 0)
        {
            if (Input.GetAxis("Vertical") > 0f || Input.GetAxis("Horizontal") < 0f ||
                    Input.GetAxis("Vertical") < 0f || Input.GetAxis("Horizontal") > 0f)
                popUpIndex++;
        }
        //Dash
        else if (popUpIndex == 1)
        {
            if (playerScript.isDashing)
                popUpIndex++;
        }
        //Attack
        else if (popUpIndex == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
                spawner.SetActive(true);
                spawnScript.SetEnabled(true);
            }
        }
        //Enemies
        else if (popUpIndex == 3)
        {
            if (spawnScript.GetPointsRemaining() < 1 && spawnScript.GetNumEnemies() == 0)
            {
                popUpIndex++;
                playerScript.SetHealth(1);
                pickup.SetActive(true);
            }
            else
                playerScript.SetHealth(playerScript.GetMaxHealth());
        }
        //Potion usage
        else if (popUpIndex == 4)
        {
            if (Input.GetKeyDown(KeyCode.F) || pressF)
            {
                pressF = true;
                if (playerScript.GetHealth() > 1)
                {
                    popUpIndex++;
                    playerScript.SetExperience(playerScript.GetNextLevelExperience());
                    playerScript.LevelUp();
                }
            }
        }
        //Activating the stats screen
        else if (popUpIndex == 5)
        {
            if (Input.GetButtonDown("Open Stats"))
                popUpIndex++;
        }
        //Leveling up
        else if (popUpIndex == 6)
        {
            if ((player.GetComponent<Player>().GetDamage() > 1 || player.GetComponent<Player>().GetDefense() > 1
                || player.GetComponent<Player>().GetAttackSpeed() > 1) || Input.GetButtonDown("Open Stats") || Input.GetKeyDown(KeyCode.Escape))
            {
                popUpIndex++;
                player.GetComponent<Inventory>().AddCoins(998);
            }
        }
        // Shop
        else if(popUpIndex == 7)
        {
            if(shop.transform.Find("Shop Camera").gameObject.activeSelf)
                popUpIndex++;
        }
        // Weapons and Consumable
        else if(popUpIndex == 8)
        {
            if(Input.GetButtonDown("Submit"))
                popUpIndex++;
        }
        // Scrolling
        else if(popUpIndex == 9)
        {
            if (Input.GetButtonDown("Submit"))
                popUpIndex++;
        }
        //Move towards game
        else if (popUpIndex == 10)
        {
            if (door.activeSelf)
                door.SetActive(false);
        }
    }
}
