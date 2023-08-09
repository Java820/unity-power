using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    [SerializeField] SellingItem[] sellingItems;
    Transform content;

    void Awake()
    {
        content = gameObject.transform.Find("Content");
    }

    private void Start()
    {
        System.Array.Sort(sellingItems, (item1, item2) => item1.price.CompareTo(item2.price));
        foreach (var item in sellingItems)
        {
            Instantiate(item, content);
        }
    }

    void Update()
    {
        
    }
}
