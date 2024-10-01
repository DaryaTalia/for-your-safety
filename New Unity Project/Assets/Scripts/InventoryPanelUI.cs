using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;
    List<GameObject> itemsInInventory;

    [SerializeField]
    Sprite keySprite;
    [SerializeField]
    Sprite wrenchSprite;
    [SerializeField]
    Sprite keycardSprite;

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
            case "Wrench":
                newItem.name = "Wrench";
                newItem.GetComponentInChildren<Image>().sprite = wrenchSprite;
                Debug.Log("Add New Item: Success");
                break;
            case "KeyCard":
                newItem.name = "KeyCard";
                newItem.GetComponentInChildren<Image>().sprite = keycardSprite;
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
                itemsInInventory.Remove(itemsInInventory.Find(go => go.name == "Key"));
                Debug.Log("Remove Item: Success");
                break;
            case "Wrench":
                itemsInInventory.Remove(itemsInInventory.Find(go => go.name == "Wrench"));
                Debug.Log("Remove Item: Success");
                break;
            case "KeyCard":
                itemsInInventory.Remove(itemsInInventory.Find(go => go.name == "KeyCard"));
                Debug.Log("Remove Item: Success");
                break;


            default:
                Debug.Log("Remove Item: Invalid Key");
                break;
        }
    }


}
