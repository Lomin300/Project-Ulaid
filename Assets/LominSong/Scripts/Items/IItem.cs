using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IItem
{
    void OnEnter();

    void OnUpdate();

    bool Use();

    void OnDestroy();

    int getCount();
    void setCount(int newCount);
    bool AddCount(int addCount);

    string getName();
    void setName(string newName);

    string getText();
    void setText(string newText);

    int getPrice();
    void setPrice(int newPrice);

    Transform getTransform();
    Image getImage();

}
