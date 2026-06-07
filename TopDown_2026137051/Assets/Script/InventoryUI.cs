using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public TestInventory inventory;
    public Transform content;
    public GameObject slotPrefab;

    void Start()
    {
        RefreshInventory();
    }
    public void RefreshInventory()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory.itemData)
        {
            if (item.itemCount <= 0)
                continue;

            GameObject slotObj =
                Instantiate(slotPrefab, content);

            InventorySlotUI slot =
                slotObj.GetComponent<InventorySlotUI>();

            slot.SetItem(item);
        }
    }
}