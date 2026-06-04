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

    public TestRecipeDataBase recipeDatabase;

    public TextMeshProUGUI TestResultText;

    public void Craft()
    {
        int[] selectedItems =
        {
            slot1.GetCurrentItemID(),
            slot2.GetCurrentItemID(),
            slot3.GetCurrentItemID()
        };

        Array.Sort(selectedItems);
        foreach (RecipeData recipe in recipeDatabase.recipes)
        {
            int[] recipeItems =
            {
                recipe.material1ID,
                recipe.material2ID,
                recipe.material3ID
            };//레시피 재료 정렬
            Array.Sort(recipeItems);

            bool match =
                selectedItems[0] == recipeItems[0] &&
                selectedItems[1] == recipeItems[1] &&
                selectedItems[2] == recipeItems[2]; //선택한 재료와 레시피 재료 비교

            if (match)
            {
                // 재료 개수 부족 검사
                if (!HasEnoughMaterials(recipe))
                {
                    string failMsg = "재료 부족";
                    Debug.Log(failMsg);
                    if (TestResultText != null)
                        TestResultText.text = failMsg;
                    return;
                }
                // 제작 성공
                string successMsg =
                    $"제작 성공!\n{recipe.recipeName}";
                Debug.Log(successMsg);
                if (TestResultText != null)
                    TestResultText.text = successMsg;
                // 재료 소모
                ConsumeMaterials(recipe);
                return;
            }
        }
        string noRecipeMsg = "제작 실패";
        Debug.Log(noRecipeMsg);
        if (TestResultText != null)
            TestResultText.text = noRecipeMsg;
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
        // 인벤토리 보유 수량 확인
        foreach (var pair in required)
        {
            int itemID = pair.Key;
            int requiredCount = pair.Value;

            var item =
                slot1.inventory.itemData.Find(x => x.itemID == itemID);

            if (item == null)
                return false;

            if (item.itemCount < requiredCount)
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

        foreach (int itemID in recipeItems)
        {
            var item =
                slot1.inventory.itemData.Find(x => x.itemID == itemID);

            if (item != null)
            {
                item.itemCount--;

                if (item.itemCount < 0)
                    item.itemCount = 0;
            }
        }
    }
}