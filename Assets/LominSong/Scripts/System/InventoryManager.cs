using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//ⓒ 2020. 'Mango Song' all rights reserved. 
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager _instance;

    [Header("▼ Regist all item GameObjects ▼")]
    public List<GameObject> registeredItem = new List<GameObject>();

    [Space]
    [Header("_______________________________")]
    public bool enable_UsingAnItemWithMouseClick = true;
    public bool checkIsLbuttonUncheckIsRbutton;
    public bool enable_ItemTooltipBox = true;

    private Dictionary<string, GameObject> registeredItemDict = new Dictionary<string, GameObject>();
    private Dictionary<string, IItem> itemOwned = new Dictionary<string, IItem>();
    private List<GameObject> foundSlot = new List<GameObject>();
    [HideInInspector]
    public ItemTooltip m_itemTooltip;

    [HideInInspector]
    public bool m_freeze = false;
    [HideInInspector]
    public bool m_tooltipSwitch = false;

    Ray m_mouseRay;
    RaycastHit2D m_hit;



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        RegistDict();
        m_itemTooltip = GetComponent<ItemTooltip>();
        DisableTooltip();
    }

    private void FixedUpdate()
    {
        CreateMouseRay();

        lock(itemOwned.Values)
        {
            foreach (IItem item in itemOwned.Values.ToList())
            {
                item.OnUpdate();
                mouseAccessCheck(item);
            }
        }

        
    }

    public void RegistDict()
    {
        registeredItemDict.Clear();
        foreach (GameObject item in registeredItem)
        {
            registeredItemDict.Add(item.GetComponent<IItem>().getName(), item);
        }
        UpdateInventoryUI();
    }

    public bool Add(string itemName, int acquiredItem = 1)
    {
        if (registeredItemDict.ContainsKey(itemName) == false)
        {
            Debug.Log(itemName + " : There is no item with a name.");
            return false;
        }

        else
        {
            if (itemOwned.ContainsKey(itemName))
            {
                itemOwned[itemName].AddCount(acquiredItem);
                Debug.Log("Added to existing item.");
            }

            else
            {
                GameObject item = Instantiate(registeredItemDict[itemName], this.transform);
                IItem itemSource = item.GetComponent<IItem>();
                

                itemSource.OnEnter();
                itemSource.setCount(acquiredItem);

                itemOwned.Add(itemSource.getName(), itemSource);
                Debug.Log("Added as new item.");
            }

            UpdateInventoryUI();
        }

        return true;
    }

    public bool Add(int itemNum, int acquiredItem = 1)
    {
        if (registeredItem[itemNum] == null)
        {
            Debug.Log(itemNum + " : There is no item with a number.");
            return false;
        }

        else
        {
            if (itemOwned.ContainsKey(registeredItem[itemNum].GetComponent<IItem>().getName()))
            {
                itemOwned[registeredItem[itemNum].GetComponent<IItem>().getName()].AddCount(acquiredItem);
                Debug.Log("Added to existing item.");
            }

            else
            {
                GameObject item = Instantiate(registeredItem[itemNum], this.transform);
                IItem itemSource = item.GetComponent<IItem>();


                itemSource.OnEnter();
                itemSource.setCount(acquiredItem);

                itemOwned.Add(itemSource.getName(), itemSource);
                Debug.Log("Added as new item.");
            }

            UpdateInventoryUI();
        }

        return true;
    }

    public bool Use(string itemName)
    {
        if (itemOwned.ContainsKey(itemName) == false)
        {
            Debug.Log(itemName + " : There is no item with a name.");
            return false;
        }

        else if(m_freeze)
        {
            Debug.Log(itemName + " is prohibited.");
            return false;
        }

        else
        {
            if (itemOwned[itemName].Use() == false)
                Debug.Log(itemName + " has been used up.");

            UpdateInventoryUI();
        }

        return true;
    }

    public bool Use(int num)
    {
        if (num > foundSlot.Count - 1)
        {
            Debug.Log(num + "slot does not exist.");
            return false;
        }

        if(foundSlot[num].transform.childCount == 0)
        {
            Debug.Log(num + "no items in the slot.");
            return false;
        }

        else if (m_freeze)
        {
            Debug.Log(num + " item is prohibited.");
            return false;
        }

        else
        {
            if (foundSlot[num].transform.GetChild(0).GetComponent<IItem>().Use() == false)
                Debug.Log(num + " item is all used.");

            UpdateInventoryUI();
        }

        return true;
    }


    public bool Delete(string itemName, int eraseCount = 1)
    {
        if (itemOwned.ContainsKey(itemName) == false)
        {
            Debug.Log(itemName + " is no item with that name.");
            return false;
        }

        else
        {
            if (itemOwned[itemName].AddCount(-eraseCount) == false)
                Debug.Log(itemName + " have been removed.");

            UpdateInventoryUI();
        }


        return true;
    }

    public bool Search(string itemName)
    {
        if (itemOwned.ContainsKey(itemName))
            return true;

        return false;
    }

    public bool DeleteAll()
    {
        foreach(IItem item in itemOwned.Values)
        {
            item.OnDestroy();
        }


        itemOwned.Clear();

        UpdateInventoryUI();

        return true;
    }

    public bool Freeze() //It make to don't use item.
    {
        return m_freeze = true;
    }

    public bool Defrost() //Make the item available.
    {
        return m_freeze = false;
    }

    public bool EnableTooltip()
    {

        m_itemTooltip.tooltipBox.SetActive(true);

        return true;

    }

    public bool DisableTooltip()
    {
        m_itemTooltip.tooltipBox.SetActive(false);

        return false;
    }

    public float AllItemCost()
    {
        float allCost = 0;

        foreach (IItem item in itemOwned.Values) //새로운 아이템 오브젝트 다시 추가
        {
            allCost += item.getPrice() * item.getCount();
        }

        return allCost;
    }

    public void ItemRecall()
    {
        foreach (IItem item in itemOwned.Values) //새로운 아이템 오브젝트 다시 추가
        {
            item.getTransform().parent = this.transform;
        }
    }


    public void UpdateInventoryUI() //Update
    {
        List<Transform> children = new List<Transform>();

        foundSlot.Clear();
        foundSlot.AddRange(GameObject.FindGameObjectsWithTag("InventorySlot"));
        
        

        if (foundSlot.Count == 0)
        {
            Debug.Log("slot does not exist.");
            return;
        }

        foreach (var slot in foundSlot) //찾은 슬롯들의 기존 아이템 오브젝트를 제거
        {
            children.Clear();
            children.AddRange(slot.GetComponentsInChildren<Transform>());

            foreach(Transform child in children)
            {
                if (child.gameObject.tag == "InventorySlot") ;

                else
                {
                    if (child.gameObject.GetComponent<IItem>().getCount() <= 0)
                    {
                        itemOwned.Remove(child.gameObject.GetComponent<IItem>().getName());
                        child.GetComponent<IItem>().OnDestroy();
                    }
                        
                    
                }
            }
        }

        int i = 0;

        foreach (IItem item in itemOwned.Values) //새로운 아이템 오브젝트 다시 추가
        {
            if (i > foundSlot.Count - 1) //슬롯의 칸 수만큼만 보여준다.
                break;

            item.getTransform().SetParent(foundSlot[i].transform);
            item.getTransform().localPosition = Vector3.zero;
            item.getImage().SetNativeSize();

            i++;
        }

    }

    void CreateMouseRay()
    {
        if (enable_ItemTooltipBox)
        {
            m_mouseRay.origin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100);

            m_mouseRay.direction = Vector3.forward;

            m_hit = Physics2D.GetRayIntersection(m_mouseRay, 500);
            Debug.Log(m_hit.collider);
        }

        if (m_hit.collider == null)
            DisableTooltip();
    }

    public void mouseAccessCheck(IItem item)
    {
        if (enable_ItemTooltipBox)
        {
            if (m_hit.collider != null && m_hit.collider.transform == item.getTransform())
            {
                m_itemTooltip.tooltipBox.SetActive(true);
                TooltipTableInit(item);

                if (Input.GetMouseButtonDown(1 - (checkIsLbuttonUncheckIsRbutton ? 1 : 0))
                    && enable_UsingAnItemWithMouseClick)
                {
                    Use(item.getName());
                }
            }
            else if (m_hit.collider == null)
            {
                DisableTooltip();
            }
        }

        else
            DisableTooltip();
    }

    void TooltipTableInit(IItem item)
    {
        m_itemTooltip.itemNameText.text = item.getName();
        m_itemTooltip.itemImage.sprite = item.getImage().sprite;
        m_itemTooltip.itemPriceText.text = item.getPrice().ToString();
        m_itemTooltip.itemCountText.text = item.getCount().ToString();
        m_itemTooltip.itemDescriptionText.text = item.getText();
    }


    //ⓒ 2020. 'Mango Song' all rights reserved.
}
