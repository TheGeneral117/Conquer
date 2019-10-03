using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatScreen : MonoBehaviour
{
    #region StatScreenProperties
    [SerializeField] private Player player = null;
    private float movementSpeed, currentHealth, maxHealth, currentExperience, nextLevelExp, attackSpeed = 0.0f;
    private int defense, damage, level, pointsAvailable = 0;
    private Button[] buttons;
    //private Button speedBTN, damageBTN, defenseBTN = null;
    private Text currentLevelText, nextLevelText, healthText, movementSpeedText, attackSpeedText, damageText, defenseText, pointsText = null;
    private RectTransform levelTransform = null;
    private GameObject mainUI = null, attackMax = null;
    private StopWatch stopWatch = null;
    #endregion
    
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>())
            stopWatch = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>();
        if (GameObject.Find("Main UI"))
        {
            mainUI = GameObject.Find("Main UI");
        }

        buttons = transform.GetComponentsInChildren<Button>();

        buttons[0] = transform.Find("Attack Speed").GetChild(0).GetComponent<Button>();
        buttons[1] = transform.Find("Attack Damage").GetChild(0).GetComponent<Button>();
        buttons[2] = transform.Find("Defense").GetChild(0).GetComponent<Button>();

        buttons[0].onClick.AddListener(AddSpeed);
        buttons[1].onClick.AddListener(AddDamage);
        buttons[2].onClick.AddListener(AddDefense);
        

        //assign player if found
        if (GameObject.FindGameObjectWithTag("Player"))
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //assign Texts
        currentLevelText = transform.Find("Level").GetChild(1).GetComponent<Text>();
        nextLevelText = transform.Find("Level").GetChild(2).GetComponent<Text>();
        healthText = transform.Find("Health").GetChild(0).GetComponent<Text>();
        movementSpeedText = transform.Find("Speed").GetChild(0).GetComponent<Text>();
        attackSpeedText = transform.Find("Attack Speed").GetChild(2).GetComponent<Text>();
        damageText = transform.Find("Attack Damage").GetChild(2).GetComponent<Text>();
        defenseText = transform.Find("Defense").GetChild(2).GetComponent<Text>();
        pointsText = transform.Find("Available Points").GetComponent<Text>();
        attackMax = transform.Find("Attack Speed").GetChild(1).gameObject;
        attackMax.SetActive(false);

        //grab level bar RectTransform
        levelTransform = transform.Find("Level").GetChild(0).GetComponent<RectTransform>();
        
        Time.timeScale = 0;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            if (go.name != "Stat Screen")
                go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);

        if(stopWatch != null)
            stopWatch.PauseStopWatch();
        buttons[0].Select();
    }

    private void Update()
    {
        if (player != null)
        {
            //update level
            level = player.GetLevel();
            currentLevelText.text = $"{level}";
            nextLevelText.text = $"{level + 1}";

            //update level bar
            currentExperience = player.GetExperience();
            nextLevelExp = player.GetNextLevelExperience();
            Vector3 levelScale = levelTransform.localScale;
            levelScale.x = currentExperience / nextLevelExp;
            levelTransform.localScale = levelScale;

            //update health
            currentHealth = player.GetHealth();
            maxHealth = player.GetMaxHealth();
            healthText.text = $"{(int)currentHealth} / {(int)maxHealth}";

            //update movement speed
            movementSpeed = player.GetMovementSpeed();
            movementSpeedText.text = $"{(int)movementSpeed}";

            //update attack speed
            attackSpeed = player.GetAttackSpeed();
            attackSpeedText.text = $"{attackSpeed}";

            //update damage
            damage = player.GetDamage();
            damageText.text = $"{damage}";

            //update defense
            defense = player.GetDefense();
            defenseText.text = $"{defense}";

            //update available points
            pointsAvailable = player.GetSpendingPoints();
            pointsText.text = $"{pointsAvailable}\t\tPoints Available";
            if(pointsAvailable == 0)
                mainUI.GetComponent<UpdateUI>().StopLevelFlashing();
        }
        //exit stat screen and reenable main ui
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Open Stats"))
            ResumeGame();
        if(player.GetTrueFireRate() <= 0.2f)
        {
            buttons[0].enabled = false;
            //speedBTN.enabled = false;
            attackMax.SetActive(true);
        }
        else
        {
            buttons[0].enabled = true;
            //speedBTN.enabled = true;
            attackMax.SetActive(false);
        }
        int selected = 0;
        foreach (Button button in buttons)
        {
            if (EventSystem.current.currentSelectedGameObject == button.gameObject)
            {
                selected++;
                break;
            }
        }
        if (selected == 0 && (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy))
            buttons[0].Select();
    }
    private void OnEnable()
    {
        if (mainUI != null)
        {
            Time.timeScale = 0;
            Object[] objects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
                if (go.name != "Stat Screen")
                    go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            if(stopWatch != null)
                stopWatch.PauseStopWatch();
        }
        if (buttons != null && buttons[0] != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            buttons[0].Select();
        }
    }

    #region StatScreenFunctions

    public void AddSpeed()
    {
        if (pointsAvailable > 0)
            player.AddAttackSpeed();
    }

    public void AddDamage()
    {
        if (pointsAvailable > 0)
            player.AddDamage();
    }

    public void AddDefense()
    {
        if (pointsAvailable > 0)
            player.AddDefense();
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
        if (stopWatch != null)
            stopWatch.ResumeStopWatch();
        if (mainUI != null)
        {
            mainUI.GetComponent<UpdateUI>().ResumeGame();
        }
    }
    #endregion
}