using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
    
    private void Start()
    {
        if (instance == null)
        { instance = this; }
    }

    public void AddItem(GameObject gameObject, string nameObg)
    {        
        if (FindItem(nameObg))
        {
            InventoryItem itemFind = items.Find(item => item.nameItem == nameObg.Trim());
            itemFind.countItem += 1;
        }
        else
        {
            InventoryItem newItem = new InventoryItem(gameObject, nameObg.Trim());
            items.Add(newItem);
        }        
        Destroy(gameObject);
    }

    public void DeleteItem (string nameObg, int count)
    {        
        InventoryItem itemFind = items.Find(item => item.nameItem == nameObg.Trim());
        if (itemFind != null)
        {
            if (itemFind.countItem > count) 
                itemFind.countItem = itemFind.countItem - count;
            else if (itemFind.countItem == count)
                items.Remove(itemFind);
        }
        else
        {
            Debug.Log("Объект не найден");
        }
    }

    public void DeleteItems(Dictionary<string, int> itemsQuest)
    {
        if (!FindItems(itemsQuest))
            return;        
        
        foreach (string item in itemsQuest.Keys)
        {
            InventoryItem itemFind = items.Find(itemfind => itemfind.nameItem == item.Trim());

            // Проверяем, существует ли ключ и получаем значение
            if (itemFind != null && itemsQuest.TryGetValue(item, out int questValue))
            {
                // Сравниваем количество найденного предмета с требуемым количеством
                if (itemFind.countItem >= questValue)
                {
                    DeleteItem(item, questValue);
                }
            }
        }
    }

    public bool FindItem(string nameObg)
    {
        InventoryItem itemFind = items.Find(item => item.nameItem == nameObg.Trim());
        if (itemFind != null)
        {
            return true;
        }
        else
        {
            Debug.Log("Объект не найден");
            return false;
        }
    }

    public bool FindItems (Dictionary<string, int> itemsQuest)
    {
        int cout = 0;
        foreach (string item in itemsQuest.Keys)
        {
            if (InventoryManager.instance.FindItem(item))
            {
                InventoryItem itemFind = items.Find(itemfind => itemfind.nameItem == item.Trim());

                // Проверяем, существует ли ключ и получаем значение
                if (itemsQuest.TryGetValue(item, out int questValue))
                {
                    // Сравниваем количество найденного предмета с требуемым количеством
                    if (itemFind != null && itemFind.countItem >= questValue)
                    {
                        cout++;
                    }
                }

            }
            if (cout == itemsQuest.Count)
            {
                return true;
            }
        }
        return false;
    }
}
