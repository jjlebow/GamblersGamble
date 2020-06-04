using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChange();
    public OnItemChange OnItemChangeCallback;
    #region Singleton
    

    public static Inventory instance;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of inventory found");
            return;
        }
        instance = this;
    }

    #endregion
    public List<Item> items = new List<Item>(); //item objects that are stored programmatically

    public void Add(Item item)
    {
        if(!item.isDefaultItem)
        {
            items.Add(item);
            if(OnItemChangeCallback != null)
                OnItemChangeCallback.Invoke();
        }
    }

    public void Remove(Item item)
    {
        if(OnItemChangeCallback != null)
            OnItemChangeCallback.Invoke();
        items.Remove(item);
    }
}
