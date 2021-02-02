using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public GameObject tooltipBox;
    public Text itemNameText;
    public Image itemImage;
    public Text itemCountText;
    public Text itemPriceText;
    public Text itemDescriptionText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tooltipBox.transform.position = Input.mousePosition;
        tooltipBox.transform.position = new Vector3(tooltipBox.transform.position.x, tooltipBox.transform.position.y, 0);


    }
}
