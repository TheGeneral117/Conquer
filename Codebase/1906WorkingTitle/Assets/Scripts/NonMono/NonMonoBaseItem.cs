using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMonoBaseItem
{
    #region BaseProperties
    public enum Type { Weapon, Consumable, EOF};
    [SerializeField] private Type itemType = Type.EOF;
    [SerializeField] private int value = 0;
    [SerializeField] private string name = "";
    [SerializeField] private string description = "";
    [SerializeField] private Sprite sprite;
    #endregion

    #region BaseFunctions
    #region Gets
    public Type GetItemType()
    {
        return itemType;
    }

    public int GetValue()
    {
        return value;
    }

    public Sprite GetSprite()
    {
        if (sprite != null)
            return Sprite.Instantiate<Sprite>(sprite);
        else
            return Resources.Load<Sprite>("Sprites/background");
    }

    public string GetName()
    {
        return name;
    }

    public string GetDesc()
    {
        return description;
    }
    #endregion

    #region Sets
    public void SetType(Type _type)
    {
        itemType = _type;
    }

    public void SetValue(int _value)
    {
        value = _value;
    }

    public void SetSprite(Sprite _sprite)
    {
        sprite = _sprite;
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public void SetDesc(string _desc)
    {
        description = _desc;
    }
    #endregion
    #endregion
}
