using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    #region ShopStats
    [SerializeField] private GameObject weaponsScreen = null, potionsScreen = null, scrollsScreen = null;
    [SerializeField] private Button Weapons = null, Potions = null, Scrolls = null, Exit = null;
    private Inventory inventory = null;
    private Text coinText, denyReason = null, purchaseText = null, descriptionText = null;
    private GameObject mainUI, denyScreen = null;
    private BaseItem currentItem = null;
    private int coins = 0;
    private StopWatch stopWatch = null;
    #endregion

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>())
            stopWatch = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>();
        if (GameObject.Find("Main UI"))
        {
            mainUI = GameObject.Find("Main UI");
            mainUI.SetActive(false);
        }

        denyScreen = transform.Find("Deny Screen").gameObject;
        denyReason = denyScreen.transform.GetChild(0).GetChild(1).GetComponent<Text>();
        denyScreen.SetActive(false);

        if (GameObject.FindGameObjectWithTag("Player"))
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        purchaseText = transform.Find("Display").GetChild(0).GetComponent<Text>();
        descriptionText = transform.Find("Display").GetChild(1).GetComponent<Text>();

        Time.timeScale = 0;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            if (go.name != "Shop UI")
                go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);

        Exit.onClick.AddListener(ExitMenu);
        Weapons.onClick.AddListener(BuyWeapons);
        Potions.onClick.AddListener(BuyPotions);
        Scrolls.onClick.AddListener(BuyScrolls);
        weaponsScreen.SetActive(true);
        potionsScreen.SetActive(false);
        scrollsScreen.SetActive(false);
    }
    
    void Update()
    {
        //update coin count
        if (inventory != null)
            coins = inventory.GetCoins();
        coinText = transform.Find("Coins Icon").GetChild(0).GetComponent<Text>();
        coinText.text = $"X{coins}";

        //exit stat screen and reenable main ui
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Open Shop"))
            ExitMenu();
    }

    private void OnEnable()
    {
        if (mainUI != null)
        {
            Time.timeScale = 0;
            Object[] objects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
                if (go.name != "Shop UI")
                    go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            if (stopWatch != null)
                stopWatch.PauseStopWatch();
            mainUI.SetActive(false);
        }
    }

    #region ShopFunctions
    private void ExitMenu()
    {
        Time.timeScale = 1;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
        if (stopWatch != null)
            stopWatch.ResumeStopWatch();
        transform.parent.gameObject.SetActive(false);
        if (mainUI != null)
        {
            mainUI.SetActive(true);
            mainUI.GetComponent<UpdateUI>().ResumeGame();
        }
    }

    public void Purchase()
    {
        if (currentItem != null && currentItem.GetValue() <= coins)
        {
            if (currentItem.GetItemType() == BaseItem.Type.Weapon)
            {
                if (!inventory.CheckDuplicateWeapon(currentItem))
                {
                    inventory.AddWeapon(currentItem);
                    inventory.AddCoins(-1 * currentItem.GetValue());
                }
                else
                {
                    DenyPuchase(true);
                }
            }
            else
            {
                inventory.AddConsumable(currentItem);
                inventory.AddCoins(-1 * currentItem.GetValue());
            }
        }
        else if (currentItem != null && currentItem.GetValue() > coins)
            DenyPuchase(false);
    }

    public void Checkout(BaseItem _item)
    {
        currentItem = _item;
        purchaseText.text = $"{currentItem.GetName()} - {currentItem.GetValue()} Coins";
        descriptionText.text = currentItem.GetDesc();
    }

    void DenyPuchase(bool duplicateWeapon)
    {
        denyScreen.SetActive(true);
        
        if (duplicateWeapon)
            denyReason.text = "You already have this weapon";
        else
            denyReason.text = "You don't have enough money to purchase this item";
    }

    public void Continue()
    {
        denyScreen.SetActive(false);
    }

    private void BuyWeapons()
    {
        weaponsScreen.SetActive(true);
        potionsScreen.SetActive(false);
        scrollsScreen.SetActive(false);
    }

    private void BuyPotions()
    {
        weaponsScreen.SetActive(false);
        potionsScreen.SetActive(true);
        scrollsScreen.SetActive(false);
    }

    private void BuyScrolls()
    {
        weaponsScreen.SetActive(false);
        potionsScreen.SetActive(false);
        scrollsScreen.SetActive(true);
    }
    #endregion
}
