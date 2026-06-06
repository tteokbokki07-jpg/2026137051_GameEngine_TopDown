using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftSlot : MonoBehaviour
{
    [System.Serializable]
    public class MaterialData
    {
        public string itemName;
        public Sprite itemSprite;
    }

    public List<MaterialData> materials;

    public Image itemImage;
    public TMP_Text itemText;

    int currentItemID = 0;


    void Start()
    {
        RefreshUI();
    }

    public void NextItem()
    {
        currentItemID++;

        if (currentItemID >= materials.Count)
        {
            currentItemID = 0;
        }

        RefreshUI();
    }

    public void PrevItem()
    {
        currentItemID--;

        if (currentItemID < 0)
        {
            currentItemID = materials.Count - 1;
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        itemImage.sprite = materials[currentItemID].itemSprite;
        itemText.text = materials[currentItemID].itemName;
    }

    public int GetCurrentItemID()
    {
        return currentItemID;
    }
}