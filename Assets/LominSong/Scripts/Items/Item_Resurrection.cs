using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Resurrection : MonoBehaviour, IItem
{
    public string itemName = "Nuadas_Touch";
    [Multiline]
    public string itemText = "누아자의 손길입니다.";
    public int price;
    public int count = 0;

    

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }

    public bool Use()
    {
        if (count > 0)
            ActiveItem();

        if (count <= 0)
            return false;

        return true;
    }

    void ActiveItem()
    {
        playerResurrection._Instance.Resurrection();
        count--;
    }

    #region Data get, set
    public int getCount()
    {
        return count;
    }

    public void setCount(int newCount)
    {
        count = newCount;

    }

    public bool AddCount(int addCount)
    {
        count += addCount;

        if (count <= 0)
            return false;

        return true;

    }

    public string getName()
    {
        return itemName;
    }

    public void setName(string newName)
    {
        itemName = newName;
    }

    public string getText()
    {
        return itemText;
    }

    public void setText(string newText)
    {
        itemText = newText;
    }

    public int getPrice()
    {
        return price;
    }

    public void setPrice(int newPrice)
    {
        price = newPrice;
    }

    public Transform getTransform()
    {
        return this.transform;
    }

    public Image getImage()
    {
        Image image = this.GetComponent<Image>();

        return image ? image : null;
    }

    #endregion

    
}
