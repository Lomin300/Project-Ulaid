using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotActive : MonoBehaviour
{
    public GameObject inventory;
    public Text scoreText;

    public void FixedUpdate()
    {
        scoreText.text = InventoryManager._instance.AllItemCost().ToString();
    }

    public void ActiveSlot()
    {
        inventory.SetActive(inventory.activeSelf ? false : true);
        InventoryManager._instance.UpdateInventoryUI();
    }
}
