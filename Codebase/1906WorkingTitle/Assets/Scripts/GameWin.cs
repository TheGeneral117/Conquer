using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameWin : MonoBehaviour
{
    #region GameWinProperties
    private Text displayText = null;
    private System.TimeSpan time;
    private float delay = 0.0f;
    private int minutes, seconds;
    private Text title, btnTxt1, btnTxt2 = null;
    private Image btnBack1, btnBack2 = null;
    private RawImage fadeIn;
    private Button[] buttons;
    private bool done;
    //private Button playAgain, mainMenu = null;
    private Color white, red, black = Color.clear;
    #endregion

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>())
            time = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>().Stop();
        delay = 0.02f;
        white = new Color(1f, 1f, 1f, 1f);
        red = new Color(1f, 0f, 0f, 1f);
        black = new Color(0f, 0f, 0f, 1f);

        #region Grabs
        displayText = transform.GetChild(1).GetComponent<Text>();
        fadeIn = transform.GetChild(0).GetComponent<RawImage>();
        title = transform.GetChild(2).GetComponent<Text>();
        btnTxt1 = transform.GetChild(3).GetChild(0).GetComponent<Text>();
        btnBack1 = transform.GetChild(3).GetComponent<Image>();
        btnTxt2 = transform.GetChild(4).GetChild(0).GetComponent<Text>();
        btnBack2 = transform.GetChild(4).GetComponent<Image>();

        //playAgain = transform.Find("Play Again").GetComponent<Button>();
        //mainMenu = transform.Find("Main Menu").GetComponent<Button>();

        //playAgain.onClick.AddListener(PlayAgain);
        //mainMenu.onClick.AddListener(MainMenu);

        buttons = GetComponentsInChildren<Button>();

        buttons[0].enabled = false;
        buttons[1].enabled = false;

        buttons[0].onClick.AddListener(PlayAgain);
        buttons[1].onClick.AddListener(MainMenu);
        
        #endregion

        Time.timeScale = 0;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
        if(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>())
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<StopWatch>().PauseStopWatch();
        done = false;
    }
    
    void Update()
    {
        if (fadeIn.color != black)
            fadeIn.color = Color.Lerp(fadeIn.color, black, delay);
        if (displayText.color != white)
            displayText.color = Color.Lerp(displayText.color, white, delay);
        if (btnBack1.color != white)
            btnBack1.color = Color.Lerp(btnBack1.color, white, delay);
        if (btnBack2.color != white)
            btnBack2.color = Color.Lerp(btnBack2.color, white, delay);
        if (btnTxt1.color != black)
            btnTxt1.color = Color.Lerp(btnTxt1.color, black, delay);
        if (btnTxt2.color != black)
            btnTxt2.color = Color.Lerp(btnTxt2.color, black, delay);
        if (title.color != red)
            title.color = Color.Lerp(title.color, red, delay);
        displayText.text = $"It took you {time.Minutes} minutes and {time.Seconds} seconds!";
        if(fadeIn.color.a >= 0.8f && !done)
        {

            buttons[0].enabled = true;
            buttons[1].enabled = true;
            done = true;
        }
        if (done)
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
    }

    void PlayAgain()
    {
        UnPause();
        GameObject clone = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneLoader"));
        StartCoroutine(clone.GetComponent<SceneLoader>().LoadNewScene(2));
        //playAgain.enabled = false;
        buttons[0].enabled = false;
    }

    void MainMenu()
    {
        UnPause();
        GameObject clone = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneLoader"));
        StartCoroutine(clone.GetComponent<SceneLoader>().LoadNewScene(0));
        //mainMenu.enabled = false;
        buttons[1].enabled = false;
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