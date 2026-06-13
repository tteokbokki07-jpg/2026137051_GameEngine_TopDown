using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public TMP_Text MoneyText;
    public Image TodayImage;
    public TMP_Text TodayText;

    public TMP_Text TodayHaveText;
    public TMP_Text TodaySellValueText;
    public int TodaySellValue;
    private int currentSelectedID = -1;
    private TestInventory.ItemData currentSelectedItem = null;

    public int EndingCost = 3200;
    public TMP_Text EndingValueText;

    public void Start()
    {
        TodayRandomWapple();
        EndingValueText.text = "구매가 : " + EndingCost;
    }
    public void Update()
    {
        //인벤토리의 Money함수를 할당한 텍스트에 표시
        TestInventory inventory = TestInventory.Instance;
        MoneyText.text = "돈 : " + inventory.Money;
    }
    public void TodayRandomWapple()
    {
        TestInventory inventory = TestInventory.Instance;

        if (inventory == null)
        {
            Debug.LogWarning("TodayRandomWapple: TestInventory 인스턴스가 없습니다.");
            return;
        }

        if (inventory.itemData == null || inventory.itemData.Count == 0)
        {
            Debug.LogWarning("TodayRandomWapple: 인벤토리에 아이템 데이터가 없습니다.");
            return;
        }

        List<TestInventory.ItemData> candidates = new List<TestInventory.ItemData>();

        foreach (var item in inventory.itemData)
        {
            int id = (int)item.itemID;
            if (id >= 100 && id <= 119)
            {
                candidates.Add(item);
            }
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("TodayRandomWapple: 범위(100~119)에 해당하는 아이템이 없습니다.");
            return;
        }

        int randIndex = Random.Range(0, candidates.Count);
        TestInventory.ItemData selected = candidates[randIndex];
        int selectedID = (int)selected.itemID;

        int count = inventory.GetItemCount(selectedID);

        // 오늘 판매가 랜덤 설정 (100 ~ 250)
        TodaySellValue = Random.Range(100, 251);
        if (TodaySellValueText != null)
        {
            TodaySellValueText.text = "판매가 : " + TodaySellValue;
        }
        else
        {
            Debug.LogWarning("TodayRandomWapple: TodaySellValueText가 할당되지 않았습니다.");
        }

        // 보유량 표시
        if (TodayHaveText != null)
        {
            TodayHaveText.text = "(보유량 : " + count + ")";
        }
        else
        {
            Debug.LogWarning("TodayRandomWapple: TodayHaveText가 할당되지 않았습니다.");
        }

        // 선택 정보 저장
        currentSelectedID = selectedID;
        currentSelectedItem = selected;

        // Today UI에 적용
        if (TodayText != null)
        {
            TodayText.text = string.IsNullOrEmpty(selected.itemName) ? $"Item {selectedID}" : selected.itemName;
        }
        else
        {
            Debug.LogWarning("TodayRandomWapple: TodayText가 할당되지 않았습니다.");
        }

        if (TodayImage != null)
        {
            if (selected.itemSprite != null)
            {
                TodayImage.sprite = selected.itemSprite;
                TodayImage.enabled = true;
            }
            else
            {
                Debug.LogWarning($"TodayRandomWapple: 선택된 아이템({selectedID})에 스프라이트가 없습니다.");
                TodayImage.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("TodayRandomWapple: TodayImage가 할당되지 않았습니다.");
        }

        Debug.Log($"선택된 ItemID: {selectedID}, itemCount: {count}, itemName: {selected.itemName}");
    } //인벤토리에서 itemID가 100~119인 항목들을 랜덤으로 하나 골라 해당 itemCount를 디버그로 출력합니다.

    public void TodayWappleSell()
    {

        TestInventory inventory = TestInventory.Instance;
        // 아이템 보유량 차감 (ConsumeItem이 실패하면 보유량 부족)
        bool consumed = inventory.ConsumeItem(currentSelectedID, 1);
        if (!consumed)
        {
            Debug.LogWarning($"TodayWappleSell: 아이템 차감 실패 - ItemID {currentSelectedID}의 보유량이 부족합니다.");
            return;
        }

        // 돈 지급
        inventory.Money += TodaySellValue;

        // UI 갱신
        int newHave = inventory.GetItemCount(currentSelectedID);
        if (TodayHaveText != null)
            TodayHaveText.text = "(보유량 : " + newHave + ")";

        Debug.Log($"TodayWappleSell: ItemID {currentSelectedID} 1개 차감, 금액 {TodaySellValue} 차감. 남은 보유: {newHave}, 남은 돈: {inventory.Money}");
    }

    public void BuyEnding()
    {
        TestInventory inventory = TestInventory.Instance;
        if (inventory.Money >= EndingCost)
        {
            inventory.Money -= EndingCost;
            SceneManager.LoadScene("End");
        }
    }
}
