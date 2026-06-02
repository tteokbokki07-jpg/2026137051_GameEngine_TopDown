using System.Collections.Generic;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public float itemID;
        public float itemCount;
        public string itemName;
        public Sprite itemSprite;
    }

    public List<ItemData> itemData;
}
