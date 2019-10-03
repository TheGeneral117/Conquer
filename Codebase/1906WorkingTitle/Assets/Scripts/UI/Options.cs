using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Options : MonoBehaviour
{
    #region OptionsProperties
    private GameObject headUI = null;
    private Button backBTN = null;
    #endregion
    
    void Start()
    {
        if (GameObject.Find("Pause"))
            headUI = GameObject.Find("Pause");
        else
            headUI = GameObject.Find("Main Menu");
        if(headUI != null)
            headUI.SetActive(false);
        backBTN = transform.Find("Sound").Find("OptionsBack").GetComponent<Button>();
        backBTN.Select();
    }

    private void OnEnable()
    {
        if (headUI != null)
            headUI.SetActive(false);
        if (backBTN != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            backBTN.Select();
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseCurrentScreen();

        if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
            backBTN.Select();
    }

    #region OptionsFunctions

    public void CloseCurrentScreen()
    {
        if (headUI != null)
        {
            headUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    #endregion
}
