using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Credits : MonoBehaviour
{
    GameObject text = null;
    Button backBTN = null;
    
    void Start()
    {
        text = transform.Find("Credit").gameObject;
        backBTN = transform.GetChild(2).GetComponent<Button>();
        backBTN.Select();
    }

    

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
            backBTN.Select();
    }

    private void OnEnable()
    {
        if(backBTN != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            backBTN.Select();
        }
        StartCoroutine(Continue());
    }

    IEnumerator Continue()
    {
        yield return new WaitForSeconds(22f);
        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenu>().CloseCredits();
    }
}
