using UnityEngine;

public class PlayInventory : MonoBehaviour
{
    public int Fruit1 = 0;
    public int Fruit2 = 0;
    public int Fruit3 = 0;
    
    public static PlayInventory Instance; //싱글톤 설정

    private void Awake()
    {
        // 이미 존재하는 경우 중복 제거
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 씬이 바뀌어도 유지
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // 씬에서 "Inventory" 오브젝트를 찾아 TestInventory에 접근한 뒤
        // itemData 리스트에서 ItemID가 1인 항목의 itemCount를 1 증가시키고 로그 출력
        GameObject inventoryObj = GameObject.Find("Inventory");
        if (inventoryObj == null)
            Debug.LogWarning("Start: Inventory 오브젝트를 찾을 수 없습니다.");

        TestInventory testInv = inventoryObj.GetComponent<TestInventory>();
        if (testInv == null)
            Debug.LogWarning("Start: Inventory 오브젝트에 TestInventory 컴포넌트가 없습니다.");

        bool found = false;
        foreach (var item in testInv.itemData)
        {
            if ((int)item.itemID == 1)
            {
                item.itemCount += 1f;
                Debug.Log($"Start: ItemID 1 ({item.itemName})의 itemCount를 1 증가시켰습니다. 현재 개수: {item.itemCount}");
                found = true;
                break;
            }
        }

        if (!found)
            Debug.LogWarning("Start: ItemID 1인 아이템을 찾지 못했습니다.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
