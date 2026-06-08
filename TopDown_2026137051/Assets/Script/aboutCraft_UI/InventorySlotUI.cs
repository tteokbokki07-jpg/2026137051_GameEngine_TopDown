using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemName;
    public TMP_Text itemCount;

    public void SetItem(TestInventory.ItemData item)
    {
        icon.sprite = item.itemSprite;
        itemName.text = item.itemName;
        itemCount.text = item.itemCount.ToString();
    }
}