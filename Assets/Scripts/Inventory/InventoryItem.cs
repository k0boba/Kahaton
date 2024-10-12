
using UnityEngine;

public class InventoryItem
{
    public GameObject item;
    public string nameItem;
    public int countItem;
    
    public InventoryItem(GameObject gameObject, string strName)
    {
        item = gameObject;
        nameItem = strName;
        countItem = 1;
    }
}
