using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public TestInventory inventory;
    public Transform content;
    public GameObject slotPrefab;
    void Start()
    {
        inventory = TestInventory.Instance;
        RefreshInventory();
    }
    public void RefreshInventory()
    {
        if (content == null)
        {
            Debug.LogWarning("InventoryUI.RefreshInventory: content가 할당되지 않았습니다.");
            return;
        }

        if (inventory == null || inventory.itemData == null)
        {
            Debug.LogWarning("InventoryUI.RefreshInventory: inventory 또는 inventory.itemData가 할당되지 않았습니다.");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogWarning("InventoryUI.RefreshInventory: slotPrefab이 할당되지 않았습니다.");
            return;
        }

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventory.itemData)
        {
            // itemID가 0이면 표시하지 않음 (빈 슬롯/미정의 아이템)
            if ((int)item.itemID == 0)
                continue;

            // 수량이 0 이하이면 표시하지 않음
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