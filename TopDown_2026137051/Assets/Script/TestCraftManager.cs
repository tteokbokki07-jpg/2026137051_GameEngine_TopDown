using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TestRecipeDataBase;

public class TestCraftManager : MonoBehaviour
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
            };

            Array.Sort(recipeItems);

            bool match =
                selectedItems[0] == recipeItems[0] &&
                selectedItems[1] == recipeItems[1] &&
                selectedItems[2] == recipeItems[2];

            if (match)
            {
                string msg = "제작 성공!  레시피 : " + recipe.recipeName + "  결과물 ID : " + recipe.resultID + "  획득 개수 : " + recipe.resultCount;
                Debug.Log(msg);
                if (TestResultText != null) TestResultText.text = msg;
                return;
            }
        }

        string failMsg = "제작 실패";
        Debug.Log(failMsg);
        if (TestResultText != null) TestResultText.text = failMsg;
    }
}