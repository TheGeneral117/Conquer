using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : BaseItem
{
    #region ConsumableStats
    public enum ConsumableType { Consumable, Thrown, EOF};
    [SerializeField] private ConsumableType consumableType = ConsumableType.Consumable;
    [SerializeField] private float floatModifier = 5;
    [SerializeField] private int intModifier = 5;
    [SerializeField] private GameObject consumableEffect = null;
    #endregion

    #region ConsumableFunctions

    #region Gets
    public ConsumableType GetConsumableType()
    {
        return consumableType;
    }

    public float GetFloatModifier()
    {
        return floatModifier;
    }

    public int GetIntModifier()
    {
        return intModifier;
    }

    public GameObject GetConsumableEffect()
    {
        return consumableEffect;
    }
    #endregion

    #region Sets
    public void SetConsumableType(ConsumableType _consumableType)
    {
        consumableType = _consumableType;
    }
    public void SetFloatModifier(float _floatModifier)
    {
        floatModifier = _floatModifier;
    }
    public void SetIntModifier(int _intModifier)
    {
        intModifier = _intModifier;
    }
    public void SetConsumableEffect(GameObject _consumableEffect)
    {
        consumableEffect =_consumableEffect;
    }
    #endregion

    #endregion
}
