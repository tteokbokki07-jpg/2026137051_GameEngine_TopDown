using NUnit;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TestRecipeDataBase;

public class CraftManager : MonoBehaviour
{
    public TestCraftSlot slot1;
    public TestCraftSlot slot2;
    public TestCraftSlot slot3;
    public TMP_Text Wapple;

    public TestRecipeDataBase recipeDatabase;

    public TextMeshProUGUI TestResultText;
    public TextMeshProUGUI RecipeText;
    public TestInventory inventory;
    public InventoryUI inventoryUI;

    private void Awake()
    {
        inventory = TestInventory.Instance;
        inventoryUI = FindFirstObjectByType<InventoryUI>();
    }
    [Serializable]
    public class ResultToItemMapping
    {
        public int resultID;
        public int itemID;
    }

    [Tooltip("레시피의 resultID를 실제 인벤토리의 itemID로 매핑 (없으면 resultID 그대로 사용)")]
    public List<ResultToItemMapping> resultMappings = new();

    void Update() //레시피 예측 업데이트
    {
        if (RecipeText == null || recipeDatabase == null) return;

        int[] selectedItems =
        {
            slot1 != null ? slot1.GetCurrentItemID() : 0,
            slot2 != null ? slot2.GetCurrentItemID() : 0,
            slot3 != null ? slot3.GetCurrentItemID() : 0
        };

        Array.Sort(selectedItems);

        string predicted = "";
        foreach (RecipeData recipe in recipeDatabase.recipes)
        {
            int[] recipeItems =
            {
                recipe.material1ID,
                recipe.material2ID,
                recipe.material3ID
            };
            Array.Sort(recipeItems);

            bool match =
                selectedItems[0] == recipeItems[0] &&
                selectedItems[1] == recipeItems[1] &&
                selectedItems[2] == recipeItems[2];

            if (match)
            {
                predicted = recipe.recipeName;
                break;
            }
        }

        RecipeText.text = "이 레시피대로라면... " + predicted + "...?";
        // itemID 4 (와플) 보유 수량을 Wapple 텍스트에 표시
        if (Wapple != null && inventory != null)
        {
            Wapple.text = "제작 가능 횟수 : " + inventory.GetItemCount(4).ToString();
        }
    }

    public void Craft()
    {
        Debug.Log("===== Craft 시작 =====");

        int[] selectedItems =
        {
        slot1.GetCurrentItemID(),
        slot2.GetCurrentItemID(),
        slot3.GetCurrentItemID()
    };

        Debug.Log($"선택 재료 : {selectedItems[0]}, {selectedItems[1]}, {selectedItems[2]}");

        Array.Sort(selectedItems);

        foreach (RecipeData recipe in recipeDatabase.recipes)
        {
            int[] recipeItems =
            {
            recipe.material1ID,
            recipe.material2ID,
            recipe.material3ID
        };

            Array.Sort(recipeItems);

            bool match =
                selectedItems[0] == recipeItems[0] &&
                selectedItems[1] == recipeItems[1] &&
                selectedItems[2] == recipeItems[2];

            if (match)
            {
                Debug.Log($"레시피 매칭 성공 : {recipe.recipeName}");

                if (!HasEnoughMaterials(recipe))
                {
                    Debug.Log("재료 부족");

                    if (TestResultText != null)
                        TestResultText.text = "재료 부족";

                    return;
                }

                Debug.Log("재료 검사 통과");

                ConsumeMaterials(recipe);

                var inv = inventory != null
                    ? inventory
                    : (slot1 != null ? slot1.inventory : null);

                if (inv == null)
                {
                    Debug.LogError("인벤토리 참조가 NULL");
                    return;
                }

                Debug.Log($"사용중인 인벤토리 : {inv.gameObject.name}");

                Debug.Log($"결과물 ID : {recipe.resultID}");
                Debug.Log($"결과물 개수 : {recipe.resultCount}");

                inv.AddItem(recipe.resultID, recipe.resultCount);
                inventoryUI.RefreshInventory();
                Debug.Log("AddItem 호출 완료");

                string successMsg =
                    $"제작 성공!\n{recipe.recipeName}";

                if (TestResultText != null)
                    TestResultText.text = successMsg;

                Debug.Log("===== Craft 종료 =====");

                return;
            }
        }

        Debug.Log("레시피 없음");

        if (TestResultText != null)
            TestResultText.text = "제작 실패";
    }

    // resultID를 실제 itemID로 변환 (매핑이 없으면 그대로 반환)
    private int MapResultID(int resultID)
    {
        var m = resultMappings.Find(x => x.resultID == resultID);
        return m != null ? m.itemID : resultID;
    }

    private bool HasEnoughMaterials(RecipeData recipe) //재료량 검사
    {
        Dictionary<int, int> required = new();
        int[] recipeItems =
        {
            recipe.material1ID,
            recipe.material2ID,
            recipe.material3ID
        };
        // 레시피에 필요한 수량 계산
        foreach (int id in recipeItems)
        {
            if (required.ContainsKey(id))
                required[id]++;
            else
                required[id] = 1;
        }

        // 인벤토리 선택: 인스펙터 할당된 inventory 우선, 없으면 slot1.inventory 사용
        var inv = inventory != null ? inventory : (slot1 != null ? slot1.inventory : null);
        if (inv == null) return false;

        // 인벤토리 보유 수량 확인
        foreach (var pair in required)
        {
            int itemID = pair.Key;
            int requiredCount = pair.Value;

            int haveCount = inv.GetItemCount(itemID);

            if (haveCount < requiredCount)
                return false;
        }
        // 추가 검사: itemID 4가 레시피에 포함되어 있지 않더라도, 인벤토리에 1개 이상 있어야 함
        if (!required.ContainsKey(4))
        {
            if (inv.GetItemCount(4) < 1)
                return false;
        }
        return true;
    }

    private void ConsumeMaterials(RecipeData recipe) // 재료 소모
    {
        int[] recipeItems =
        {
            recipe.material1ID,
            recipe.material2ID,
            recipe.material3ID
        };

        var inv = inventory != null ? inventory : (slot1 != null ? slot1.inventory : null);
        if (inv == null) return;

        // 각 재료별로 1개씩 소모 (itemID가 0인 경우 소모하지 않음)
        foreach (int itemID in recipeItems)
        {
            if (itemID == 0)
            {
                Debug.Log($"ItemID 0은 소모하지 않음 (아이템 인덱스 무시)");
                continue;
            }

            inv.ConsumeItem(itemID, 1);
        }

        // 제작 성공시 추가로 itemID 4를 1개 소모한다 (레시피에 이미 4가 포함되어 있다면 중복 소모를 피함)
        bool recipeContains4 = Array.Exists(recipeItems, id => id == 4);
        if (!recipeContains4)
        {
            inv.ConsumeItem(4, 1);
        }
    }
}