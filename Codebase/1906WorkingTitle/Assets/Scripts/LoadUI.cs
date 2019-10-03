using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadUI : MonoBehaviour
{
    private Button loadOne = null;
    private Button loadTwo = null;
    private Button loadThree = null;
    private SaveScript saveScript = null;
    private Text loadOneText = null;
    private Text loadTwoText = null;
    private Text loadThreeText = null;
    private GameObject mainUI = null;

    void Start()
    {
        loadOne = GameObject.Find("Load File 1").GetComponent<Button>();
        loadTwo = GameObject.Find("Load File 2").GetComponent<Button>();
        loadThree = GameObject.Find("Load File 3").GetComponent<Button>();
        saveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SaveScript>();
        loadOne.onClick.AddListener(SelectOne);
        loadTwo.onClick.AddListener(SelectTwo);
        loadThree.onClick.AddListener(SelectThree);
        loadOneText = loadOne.gameObject.GetComponentInChildren<Text>();
        loadTwoText = loadTwo.gameObject.GetComponentInChildren<Text>();
        loadThreeText = loadThree.gameObject.GetComponentInChildren<Text>();
        if (GameObject.Find("Main UI"))
        {
            mainUI = GameObject.Find("Main UI");
            mainUI.GetComponent<UpdateUI>().PauseUI();
        }
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey($"Level{1}"))
            loadOneText.text = $"Load 1\nLevel: {PlayerPrefs.GetInt($"Level{1}")}\nBox Pieces: {PlayerPrefs.GetInt($"Boxes{1}")}";
        else
            loadOneText.text = "Load 1";

        if (PlayerPrefs.HasKey($"Level{2}"))
            loadTwoText.text = $"Load 2\nLevel: {PlayerPrefs.GetInt($"Level{2}")}\nBox Pieces: {PlayerPrefs.GetInt($"Boxes{2}")}";
        else
            loadTwoText.text = "Load 2";

        if (PlayerPrefs.HasKey($"Level{3}"))
            loadThreeText.text = $"Load 3\nLevel: {PlayerPrefs.GetInt($"Level{3}")}\nBox Pieces: {PlayerPrefs.GetInt($"Boxes{3}")}";
        else
            loadThreeText.text = "Load 3";
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
        if (mainUI != null)
            mainUI.GetComponent<UpdateUI>().PauseUI();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        Object[] objects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject go in objects)
            go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
        if (mainUI != null)
            mainUI.GetComponent<UpdateUI>().ResumeGame();
    }

    private void SelectOne()
    {
        saveScript.SetSaveSlot(1);
        saveScript.Load();
        gameObject.SetActive(false);
    }

    private void SelectTwo()
    {
        saveScript.SetSaveSlot(2);
        saveScript.Load();
        gameObject.SetActive(false);
    }

    private void SelectThree()
    {
        saveScript.SetSaveSlot(3);
        saveScript.Load();
        gameObject.SetActive(false);
    }
}
