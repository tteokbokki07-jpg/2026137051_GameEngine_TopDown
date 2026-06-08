using System.Collections.Generic;
using UnityEngine;

public class TestRecipeDataBase : MonoBehaviour
{
    [System.Serializable]
    public class RecipeData
    {
        public string recipeName;

        public int material1ID;
        public int material2ID;
        public int material3ID;

        public int resultID;
        public int resultCount = 1;
    }
    public List<RecipeData> recipes = new();

}
