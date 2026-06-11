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
        inventory = TestInventory.Instance;
        // 시작 시 slotIndex 를 현재 인덱스로 사용하되 허용 범위로 보정
        int max = GetAllowedMax();
        if (max >= 0)
            currentItemIndex = Mathf.Clamp(StartslotIndex, 0, max);
        else
            currentItemIndex = 0;

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

        int max = GetAllowedMax();
        if (max < 0) return;
        if (currentItemIndex < 0 || currentItemIndex > max) return;

        int currentID = Mathf.FloorToInt(inventory.itemData[currentItemIndex].itemID);
        int currentCount = Mathf.FloorToInt(inventory.itemData[currentItemIndex].itemCount);

        if (currentID != previousItemID || currentCount != previousItemCount)
        {
            previousItemID = currentID;
            previousItemCount = currentCount;
            RefreshUI();
        }
    }

    public void RefreshUI()
    {
        if (inventory == null || inventory.itemData == null)
        {
            // 비어있을 때 처리
            if (itemImage != null) itemImage.sprite = null;
            if (itemNameText != null) itemNameText.text = "";
            if (itemCountText != null) itemCountText.text = "";
            return;
        }

        int max = GetAllowedMax();
        if (max < 0 || currentItemIndex < 0 || currentItemIndex > max)
        {
            if (itemImage != null) itemImage.sprite = null;
            if (itemNameText != null) itemNameText.text = "(빈슬롯)";
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

    // CraftSlot 호환용: 다음 아이템 (0..3 범위 내에서 순환)
    public void NextItem()
    {
        int max = GetAllowedMax();
        if (inventory == null || inventory.itemData == null || max < 0) return;

        currentItemIndex++;
        if (currentItemIndex > max)
        {
            currentItemIndex = 0;
        }

        RefreshUI();
    }

    // CraftSlot 호환용: 이전 아이템 (0..3 범위 내에서 순환)
    public void PrevItem()
    {
        int max = GetAllowedMax();
        if (inventory == null || inventory.itemData == null || max < 0) return;

        currentItemIndex--;
        if (currentItemIndex < 0)
        {
            currentItemIndex = max;
        }

        RefreshUI();
    }
    public int GetCurrentItemID() //제작 시스템이 현재 재료를 읽음
    {
        if (inventory == null ||
            inventory.itemData == null)
        {
            return 0;
        }

        int max = GetAllowedMax();
        if (max < 0 || currentItemIndex < 0 || currentItemIndex > max)
            return 0;

        return Mathf.FloorToInt(
            inventory.itemData[currentItemIndex].itemID);
    }

    // Craft용: 현재 슬롯의 아이템 수량을 반환합니다.
    public int GetCurrentItemCount()
    {
        if (inventory == null ||
            inventory.itemData == null)
        {
            return 0;
        }

        int max = GetAllowedMax();
        if (max < 0 || currentItemIndex < 0 || currentItemIndex > max)
            return 0;

        return Mathf.FloorToInt(inventory.itemData[currentItemIndex].itemCount);
    }

    // 제작용: 현재 선택된 아이템을 소모합니다. 성공하면 true 반환
    public bool ConsumeCurrentItem(int amount)
    {
        if (amount <= 0) return false;
        if (inventory == null || inventory.itemData == null) return false;

        int max = GetAllowedMax();
        if (max < 0 || currentItemIndex < 0 || currentItemIndex > max) return false;

        var data = inventory.itemData[currentItemIndex];
        int id = Mathf.FloorToInt(data.itemID);
        if (id == 0) return false; // 비어있는 슬롯

        // 아이템 개수 차감
        data.itemCount = Mathf.Max(0, data.itemCount - amount);

        // UI 갱신
        RefreshUI();
        return true;
    }

    // 허용되는 최대 인덱스 반환: inventory가 비어있으면 -1, 아니면 0..min(3, count-1)
    private int GetAllowedMax()
    {
        if (inventory == null || inventory.itemData == null) return -1;
        if (inventory.itemData.Count == 0) return -1;
        return Mathf.Min(3, inventory.itemData.Count - 1);
    }
}
