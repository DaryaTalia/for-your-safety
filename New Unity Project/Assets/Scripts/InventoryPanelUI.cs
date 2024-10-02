using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;
    public List<GameObject> itemsInInventory;

    [SerializeField]
    Sprite keySprite;

    // Start is called before the first frame update
    void Start()
    {
        itemsInInventory = new List<GameObject>();
    }

    public void AddNewItem(string Key)
    {
        GameObject newItem;
        newItem = Instantiate(itemPrefab, this.transform);
        itemsInInventory.Add(newItem);

        switch (Key)
        {
            case "Key":
                newItem.name = "Key";
                newItem.GetComponentInChildren<Image>().sprite = keySprite;
                Debug.Log("Add New Item: Success");
                break;

            default:
                Debug.Log("Add New Item: Invalid Key");
                itemsInInventory.Remove(newItem);
                Destroy(newItem);
                break;
        }
    }

    public void RemoveItem(string Key)
    {
        switch (Key)
        {
            case "Key":
                Destroy(itemsInInventory.Find(go => go.name == "Key"));
                itemsInInventory.Clear();
                Debug.Log("Remove Item: Success");
                break;

            default:
                Debug.Log("Remove Item: Invalid Key");
                break;
        }
    }


}
