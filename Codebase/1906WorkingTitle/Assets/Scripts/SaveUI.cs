using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour
{
    Button saveOne = null;
    Button saveTwo = null;
    Button saveThree = null;
    Button back = null;
    Button clearSaves = null;
    SaveScript saveScript = null;
    Text saveOneText = null;
    Text saveTwoText = null;
    Text saveThreeText = null;
    GameObject mainUI = null;

    // Start is called before the first frame update
    void Start()
    {
        saveOne = GameObject.Find("Save File 1").GetComponent<Button>();
        saveTwo = GameObject.Find("Save File 2").GetComponent<Button>();
        saveThree = GameObject.Find("Save File 3").GetComponent<Button>();
        back = GameObject.Find("Back Button").GetComponent<Button>();
        clearSaves = GameObject.Find("Clear Saves").GetComponent<Button>();
        saveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<SaveScript>();
        saveOne.onClick.AddListener(SelectOne);
        saveTwo.onClick.AddListener(SelectTwo);
        saveThree.onClick.AddListener(SelectThree);
        back.onClick.AddListener(TurnOff);
        clearSaves.onClick.AddListener(ClearSaves);
        saveOneText = saveOne.gameObject.GetComponentInChildren<Text>();
        saveTwoText = saveTwo.gameObject.GetComponentInChildren<Text>();
        saveThreeText = saveThree.gameObject.GetComponentInChildren<Text>();
        if (GameObject.Find("Main UI"))
        {
            mainUI = GameObject.Find("Main UI");
            mainUI.GetComponent<UpdateUI>().PauseUI();
        }
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

    private void Update()
    {
        if (PlayerPrefs.HasKey($"Level{1}"))
            saveOneText.text = $"Save 1\nLevel: {PlayerPrefs.GetInt($"Level{1}")}\nBox Pieces: {PlayerPrefs.GetInt($"Boxes{1}")}";
        else
            saveOneText.text = "Save 1";

        if (PlayerPrefs.HasKey($"Level{2}"))
            saveTwoText.text = $"Save 2\nLevel: {PlayerPrefs.GetInt($"Level{2}")}\nBox Pieces: {PlayerPrefs.GetInt($"Boxes{2}")}";
        else
            saveTwoText.text = "Save 2";

        if (PlayerPrefs.HasKey($"Level{3}"))
            saveThreeText.text = $"Save 3\nLevel: {PlayerPrefs.GetInt($"Level{3}")}\nBox Pieces: {PlayerPrefs.GetInt($"Boxes{3}")}";
        else
            saveThreeText.text = "Save 3";

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            TurnOff();
    }

    private void SelectOne()
    {
        saveScript.SetSaveSlot(1);
        saveScript.Save();
        if (PlayerPrefs.HasKey($"Level{saveScript.GetSaveSlot()}"))
            saveOneText.text = $"Save 1\nLevel: {PlayerPrefs.GetInt($"Level{saveScript.GetSaveSlot()}")}";
    }

    private void SelectTwo()
    {
        saveScript.SetSaveSlot(2);
        saveScript.Save();
        if (PlayerPrefs.HasKey($"Level{saveScript.GetSaveSlot()}"))
            saveTwoText.text = $"Save 2\nLevel: {PlayerPrefs.GetInt($"Level{saveScript.GetSaveSlot()}")}";
    }

    private void SelectThree()
    {
        saveScript.SetSaveSlot(3);
        saveScript.Save();
        if (PlayerPrefs.HasKey($"Level{saveScript.GetSaveSlot()}"))
            saveThreeText.text = $"Save 3\nLevel: {PlayerPrefs.GetInt($"Level{saveScript.GetSaveSlot()}")}";
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }

    private void ClearSaves()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume");
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }
}
