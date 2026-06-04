using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestCraftSlot : MonoBehaviour
{
    // 참조할 인벤토리 (인스펙터에 드래그)
    public TestInventory inventory;

    // 이 슬롯이 참조할 인벤토리 리스트의 인덱스
    public int StartslotIndex = 0;

    // 현재 표시중인 인벤토리 리스트 인덱스 (CraftSlot의 currentItemID 역할)
    int currentItemIndex = 0;

    public Image itemImage;
    public TMP_Text itemNameText;
    public TMP_Text itemCountText;

    int previousItemID = -1;
    int previousItemCount = -1;

    void Start()
    {
        // 시작 시 slotIndex 를 현재 인덱스로 사용
        currentItemIndex = StartslotIndex;
        RefreshUI();
        // 초기값 저장
        if (inventory != null && inventory.itemData != null && currentItemIndex >= 0 && currentItemIndex < inventory.itemData.Count)
        {
            previousItemID = Mathf.FloorToInt(inventory.itemData[currentItemIndex].itemID);
            previousItemCount = Mathf.FloorToInt(inventory.itemData[currentItemIndex].itemCount);
        }
    }

    void Update()
    {
        // 인벤토리나 인덱스가 유효하면 itemID 변화 감지
        if (inventory == null || inventory.itemData == null) return;
        if (currentItemIndex < 0 || currentItemIndex >= inventory.itemData.Count) return;

        int currentID = Mathf.FloorToInt(inventory.itemData[currentItemIndex].itemID);
        int currentCount = Mathf.FloorToInt(inventory.itemData[currentItemIndex].itemCount);

        if (currentID != previousItemID || currentCount != previousItemCount)
        {
            previousItemID = currentID;
            previousItemCount = currentCount;
            RefreshUI();
        }
    }

    void RefreshUI()
    {
        if (inventory == null || inventory.itemData == null || currentItemIndex < 0 || currentItemIndex >= inventory.itemData.Count)
        {
            // 비어있을 때 처리
            if (itemImage != null) itemImage.sprite = null;
            if (itemNameText != null) itemNameText.text = "";
            if (itemCountText != null) itemCountText.text = "";
            return;
        }

        var data = inventory.itemData[currentItemIndex];
        int id = Mathf.FloorToInt(data.itemID);

        // id == 0 이면 아이템 없음
        if (id == 0)
        {
            if (itemImage != null) itemImage.sprite = null;
            if (itemNameText != null) itemNameText.text = "(빈슬롯)";
            if (itemCountText != null) itemCountText.text = "";
        }
        else
        {
            if (itemImage != null) itemImage.sprite = data.itemSprite;
            if (itemNameText != null) itemNameText.text = data.itemName ?? $"Item {id}";
            if (itemCountText != null) itemCountText.text = Mathf.FloorToInt(data.itemCount).ToString();
        }
    }

    // CraftSlot 호환용: 다음 아이템 (materials 순환과 동일 동작)
    public void NextItem()
    {
        if (inventory == null || inventory.itemData == null || inventory.itemData.Count == 0) return;

        currentItemIndex++;
        if (currentItemIndex >= inventory.itemData.Count)
        {
            currentItemIndex = 0;
        }

        RefreshUI();
    }

    // CraftSlot 호환용: 이전 아이템
    public void PrevItem()
    {
        if (inventory == null || inventory.itemData == null || inventory.itemData.Count == 0) return;

        currentItemIndex--;
        if (currentItemIndex < 0)
        {
            currentItemIndex = inventory.itemData.Count - 1;
        }

        RefreshUI();
    }
    public int GetCurrentItemID() //제작 시스템이 현재 재료를 읽음
    {
        if (inventory == null ||
            inventory.itemData == null ||
            currentItemIndex < 0 ||
            currentItemIndex >= inventory.itemData.Count)
        {
            return 0;
        }

        return Mathf.FloorToInt(
            inventory.itemData[currentItemIndex].itemID);
    }

    // Craft용: 현재 슬롯의 아이템 수량을 반환합니다.
    public int GetCurrentItemCount()
    {
        if (inventory == null ||
            inventory.itemData == null ||
            currentItemIndex < 0 ||
            currentItemIndex >= inventory.itemData.Count)
        {
            return 0;
        }

        return Mathf.FloorToInt(inventory.itemData[currentItemIndex].itemCount);
    }

    // 제작용: 현재 선택된 아이템을 소모합니다. 성공하면 true 반환
    public bool ConsumeCurrentItem(int amount)
    {
        if (amount <= 0) return false;
        if (inventory == null || inventory.itemData == null) return false;
        if (currentItemIndex < 0 || currentItemIndex >= inventory.itemData.Count) return false;

        var data = inventory.itemData[currentItemIndex];
        int id = Mathf.FloorToInt(data.itemID);
        if (id == 0) return false; // 비어있는 슬롯

        // 아이템 개수 차감
        data.itemCount = Mathf.Max(0, data.itemCount - amount);

        // 개수가 0이되면 아이템을 비웁니다 (id를 0으로)
        if (data.itemCount <= 0)
        {
            data.itemCount = 0;
        }

        // UI 갱신
        RefreshUI();
        return true;
    }
}
