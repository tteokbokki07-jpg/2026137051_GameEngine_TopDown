using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public int itemID;
    public int count;
}

[System.Serializable]
public class InventoryDataList
{
    public List<InventoryData> inventorySlots = new();
}

public static class InventorySaver
{
    private const string FILE = "Player_Inventory.json";

    private static string FilePath =>
        Path.Combine(Application.persistentDataPath, FILE);

    // 아이템 저장
    public static void SaveItem(int itemID, int count)
    {
        InventoryDataList list = LoadInternal();

        // 이미 존재하는 아이템인지 확인
        InventoryData existingItem =
            list.inventorySlots.Find(x => x.itemID == itemID);

        if (existingItem != null)
        {
            // 개수 갱신
            existingItem.count = count;
        }
        else
        {
            // 새 아이템 추가
            list.inventorySlots.Add(new InventoryData
            {
                itemID = itemID,
                count = count
            });
        }

        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(FilePath, json);
    }

    // 전체 데이터 불러오기
    public static InventoryDataList LoadInventory()
    {
        return LoadInternal();
    }

    // 저장파일 삭제
    public static void DeleteSave()
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
    }

    private static InventoryDataList LoadInternal()
    {
        if (!File.Exists(FilePath))
        {
            return new InventoryDataList();
        }

        string json = File.ReadAllText(FilePath);

        InventoryDataList list =
            JsonUtility.FromJson<InventoryDataList>(json);

        return list ?? new InventoryDataList();
    }
}