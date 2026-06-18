using System.Collections.Generic;
using System.IO;
using UnityEngine;

// TestInventory(아이템 목록)와 Money를 JSON 파일로 저장하고 불러오는 클래스
// itemSprite, itemName은 저장하지 않습니다 - TestInventory.itemData는 인스펙터에서
// 미리 등록해 둔 슬롯이라, 불러올 때 itemID로 같은 슬롯을 찾아 itemCount만 덮어씁니다.
public static class TestInventorySaveSystem
{
    private const string FILE_NAME = "TestInventory_Save.json";

    private static string FilePath =>
        Path.Combine(Application.persistentDataPath, FILE_NAME);

    [System.Serializable]
    private class SavedItem
    {
        public int itemID;
        public int itemCount;
    }

    [System.Serializable]
    private class SaveData
    {
        public int money;
        public List<SavedItem> items = new List<SavedItem>();
    }

    // 인벤토리 전체(아이템 + 돈) 저장
    public static void Save(TestInventory inventory)
    {
        if (inventory == null || inventory.itemData == null)
        {
            Debug.LogWarning("TestInventorySaveSystem.Save: 저장할 인벤토리가 없습니다.");
            return;
        }

        SaveData data = new SaveData { money = inventory.Money };

        foreach (TestInventory.ItemData item in inventory.itemData)
        {
            data.items.Add(new SavedItem
            {
                itemID = (int)item.itemID,
                itemCount = (int)item.itemCount
            });
        }

        string json = JsonUtility.ToJson(data, true);

        try
        {
            string directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(FilePath, json);
            Debug.Log($"인벤토리 저장 완료: {FilePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"TestInventorySaveSystem.Save: 저장 중 오류가 발생했습니다.\n{e}");
        }
    }

    // 저장된 데이터를 현재 인벤토리에 적용 (itemSprite, itemName은 그대로 유지됨)
    public static void Load(TestInventory inventory)
    {
        if (inventory == null || inventory.itemData == null)
        {
            Debug.LogWarning("TestInventorySaveSystem.Load: 불러올 인벤토리가 없습니다.");
            return;
        }

        if (!File.Exists(FilePath))
        {
            Debug.Log("TestInventorySaveSystem.Load: 저장 파일이 없습니다.");
            return;
        }

        try
        {
            string json = File.ReadAllText(FilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null)
            {
                Debug.LogWarning("TestInventorySaveSystem.Load: 저장 데이터를 읽는 데 실패했습니다.");
                return;
            }

            inventory.Money = data.money;

            foreach (SavedItem saved in data.items)
            {
                TestInventory.ItemData target =
                    inventory.itemData.Find(x => (int)x.itemID == saved.itemID);

                if (target != null)
                {
                    target.itemCount = saved.itemCount;
                }
                else
                {
                    Debug.LogWarning(
                        $"TestInventorySaveSystem.Load: ItemID {saved.itemID}를 인벤토리에서 찾을 수 없습니다. " +
                        "인스펙터에 해당 아이템 슬롯이 등록되어 있는지 확인하세요.");
                }
            }

            Debug.Log("인벤토리 불러오기 완료");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"TestInventorySaveSystem.Load: 불러오기 중 오류가 발생했습니다.\n{e}");
        }
    }

    public static bool HasSaveFile() => File.Exists(FilePath);

    public static void DeleteSave()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}