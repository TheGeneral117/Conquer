using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    #region MainMenuProperties
    //private Button startBTN, loadBTN, tutorialBTN, creditsBTN, optionsBTN, exitBTN = null;
    private Button[] buttons;
    private GameObject credits, options = null;
    GameObject music = null;
    #endregion
    
    void Start()
    {
        music = GameObject.Find("Menu Music");
        buttons = transform.GetComponentsInChildren<Button>();
        //assign buttons
        ////startBTN = transform.Find("Start Game").GetComponent<Button>();
        ////loadBTN = transform.Find("Load Game").GetComponent<Button>();
        ////tutorialBTN = transform.Find("Tutorial").GetComponent<Button>();
        ////creditsBTN = transform.Find("CreditsBTN").GetComponent<Button>();
        ////optionsBTN = transform.Find("OptionsBTN").GetComponent<Button>();
        ////exitBTN = transform.Find("Exit Game").GetComponent<Button>();

        ////add function listeners
        //startBTN.onClick.AddListener(StartGame);
        //loadBTN.onClick.AddListener(LoadGame);
        //tutorialBTN.onClick.AddListener(Tutorial);
        //creditsBTN.onClick.AddListener(Credits);
        //optionsBTN.onClick.AddListener(OptionsMenu);
        //exitBTN.onClick.AddListener(ExitGame);

        buttons[0].onClick.AddListener(StartGame);
        buttons[1].onClick.AddListener(LoadGame);
        buttons[2].onClick.AddListener(Tutorial);
        buttons[3].onClick.AddListener(Credits);
        buttons[4].onClick.AddListener(OptionsMenu);

#if UNITY_STANDALONE
            buttons[5].onClick.AddListener(ExitGame);
#elif UNITY_WEBGL
            buttons[5].gameObject.SetActive(false);
     Debug.Log("Unity Webplayer");
#endif

        credits = GameObject.Find("Credits");
        credits.SetActive(false);

        options = GameObject.Find("Options");
        options.SetActive(false);
    }

    private void OnEnable()
    {
        if (credits != null)
            credits.SetActive(false);
        if (options != null)
            options.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseCredits();

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

#region MainMenuFunctions

    private void StartGame()
    {
        GameObject clone = Instantiate(Resources.Load<GameObject>("Prefabs/UI/IntroLetter"));
        StartCoroutine(clone.GetComponent<IntroLetterLoader>().LoadNewScene(2));
        buttons[0].enabled = false;
        music.SetActive(false);
    }

    private void LoadGame()
    {
        GameObject clone = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneLoader"));
        StartCoroutine(clone.GetComponent<SceneLoader>().LoadSceneandSettings(2));
        buttons[1].enabled = false;
        music.SetActive(false);
    }

    private void Tutorial()
    {
        GameObject clone = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneLoader"));
        StartCoroutine(clone.GetComponent<SceneLoader>().LoadNewScene(1));
        buttons[2].enabled = false;
        music.SetActive(false);
    }

    private void Credits()
    {
        credits.SetActive(true);
    }

    public void CloseCredits()
    {
        if (credits.activeSelf)
            credits.SetActive(false);
    }

    private void OptionsMenu()
    {
        options.SetActive(true);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
#endregion
}
