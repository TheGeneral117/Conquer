using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour
{
    GameObject player = null;
    private Button[] buttons;
    //private Button playAgain, mainMenu = null, continueFromLastSave;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(PlayAgain);
        buttons[1].onClick.AddListener(MainMenu);
        buttons[2].onClick.AddListener(Continue);

        buttons[0].Select();

        //playAgain = transform.Find("Play Again").GetComponent<Button>();
        //mainMenu = transform.Find("Main Menu").GetComponent<Button>();
        //continueFromLastSave = transform.Find("Continue from last save").GetComponent<Button>();

        //playAgain.onClick.AddListener(PlayAgain);
        //mainMenu.onClick.AddListener(MainMenu);
        //continueFromLastSave.onClick.AddListener(Continue);

        Time.timeScale = 0;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {

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

    private void PlayAgain()
    {
        UnPause();
        SceneManager.LoadScene("Build Scene");
        //playAgain.enabled = false;
        buttons[0].enabled = false;
    }

    private void MainMenu()
    {
        UnPause();
        SceneManager.LoadScene("Main Menu");
        //mainMenu.enabled = false;
        buttons[1].enabled = false;
    }

    private void Continue()
    {
        UnPause();
        Object[] objects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        foreach (GameObject go in objects)
            if (go.CompareTag("Player") && !go.activeSelf)
            {
                go.SetActive(true);
                //go.GetComponent<Animator>().SetBool("Death", false);
            }
        ConditionManager conManager = player.GetComponent<ConditionManager>();
        conManager.TimerSet("fire", 0);
        conManager.TimerSet("thaw", 0);
        conManager.TimerSet("stun", 0);
        player.GetComponent<Player>().SetLives(5);
        StartCoroutine(player.GetComponent<Player>().Invincible());
        //continueFromLastSave.enabled = false;
        //buttons[2].enabled = false;
        gameObject.SetActive(false);
    }

    void UnPause()
    {
        Time.timeScale = 1;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>())
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>().ResumeStopWatch();
    }
}
