using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllItem : MonoBehaviour
{
    public GameObject gameobject;

    public void DisableObject()
    {
        gameobject.SetActive(false);
    }

    public void ClearItem()
    {
        gameobject.SetActive(false);
        InventoryManager._instance.DeleteAll();
    }
}
