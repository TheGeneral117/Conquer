using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeep : MonoBehaviour
{
    [SerializeField] GameObject Shop;
    
    private void Start()
    {
        Shop = transform.Find("Shop Camera").gameObject;
    }

    public void OpenShop()
    {
        Shop.SetActive(true);
    }
}
