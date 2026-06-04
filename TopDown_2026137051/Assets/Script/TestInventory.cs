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
    [System.Serializable]
    public class WappleData
    {
        public int wappleID;
        public int wappleCount;
        public string wappleName;
        public Sprite wappleSprite;
    }

    public List<ItemData> itemData;
    public List<WappleData> wappleData;
}
