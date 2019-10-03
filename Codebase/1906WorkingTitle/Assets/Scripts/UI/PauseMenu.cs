using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    #region PauseMenuProperties
    private Button[] buttons;
    //private Button ResumeBTN, OptionsBTN, ExitBTN = null;
    private GameObject mainUI, optionsMenu = null;
    #endregion
    
    void Start()
    {
        if (GameObject.Find("Main UI"))
        {
            mainUI = GameObject.Find("Main UI");
            mainUI.SetActive(false);
        }

        optionsMenu = transform.Find("Options").gameObject;
        optionsMenu.SetActive(false);

        buttons = transform.Find("Pause").GetComponentsInChildren<Button>();

        //ResumeBTN = transform.Find("Pause").Find("Resume").gameObject.GetComponent<Button>();
        //OptionsBTN = transform.Find("Pause").Find("OptionsBTN").gameObject.GetComponent<Button>();
        //ExitBTN = transform.Find("Pause").Find("Exit Game").gameObject.GetComponent<Button>();

        //ResumeBTN.onClick.AddListener(Resume);
        //OptionsBTN.onClick.AddListener(Options);
        //ExitBTN.onClick.AddListener(ExitGame);

        buttons[0] = transform.Find("Pause").Find("Resume").gameObject.GetComponent<Button>();
        buttons[1] = transform.Find("Pause").Find("OptionsBTN").gameObject.GetComponent<Button>();
        buttons[2] = transform.Find("Pause").Find("Exit Game").gameObject.GetComponent<Button>();

        buttons[0].onClick.AddListener(Resume);
        buttons[1].onClick.AddListener(Options);
        buttons[2].onClick.AddListener(ExitGame);

        buttons[0].Select();

        Time.timeScale = 0;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            if (go.name != "Pause Menu")
                go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>())
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>().PauseStopWatch();
    }
    
    void Update()
    {
        //exit pause menu and reenable main ui
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !transform.Find("Options").gameObject.activeSelf)
            Resume();
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
                if (go.name != "Pause Menu")
                    go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
            if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>())
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>().PauseStopWatch();
            mainUI.SetActive(false);
        }

        optionsMenu = transform.Find("Options").gameObject;
        optionsMenu.SetActive(false);
        if (transform.Find("Pause").gameObject)
            transform.Find("Pause").gameObject.SetActive(true);
        if(buttons != null && buttons[0] != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            buttons[0].Select();
        }
    }

    #region PauseMenuFunctions
    private void Resume()
    {
        UnPause();
        if (mainUI != null)
        {
            mainUI.SetActive(true);
            mainUI.GetComponent<UpdateUI>().ResumeGame();
        }
    }

    private void Options()
    {
        optionsMenu.SetActive(true);
    }

    private void UnPause()
    {
        Time.timeScale = 1;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>())
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>().ResumeStopWatch();
    }

    private void ExitGame()
    {
        UnPause();
        SceneManager.LoadScene("Main Menu");
    }
    #endregion
}