using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public GameObject SettingPanal;
    public GameObject InventoryPanal;
    public bool isPaused = false;

    public InventoryUI inventoryUI;
    private void Awake()
    {
        inventoryUI = GetComponent<InventoryUI>();
        if (inventoryUI == null)
            inventoryUI = FindFirstObjectByType<InventoryUI>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            Time.timeScale = 0;
            SettingPanal.SetActive(true);
            Debug.Log("ESC");
            isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            Time.timeScale = 1;
            SettingPanal.SetActive(false);
            InventoryPanal.SetActive(false);
            Debug.Log("ESC_down");
            isPaused = false;
        }
    }
    public void GameStop()
    {
        Time.timeScale = 0;
        SettingPanal.SetActive(true);
        isPaused = true;
    }
    public void GamePause()
    {
        Time.timeScale = 1;
        SettingPanal.SetActive(false);
        isPaused = false;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void GoStage()
    {
        SceneManager.LoadScene("Play_1");
        Time.timeScale = 1;
        isPaused = false;
    }
    public void GoCraft()
    {
        SceneManager.LoadScene("Crafting");
    }
    public void GoShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void InventoryClear()
    {
        TestInventory inventory = TestInventory.Instance;
        inventory.ClearItem();
    }

    // 저장 버튼에 연결: 현재 인벤토리와 돈을 JSON 파일로 저장
    public void SaveGame()
    {
        TestInventory inventory = TestInventory.Instance;
        if (inventory == null)
        {
            Debug.LogWarning("SaveGame: TestInventory 인스턴스를 찾을 수 없습니다.");
            return;
        }

        TestInventorySaveSystem.Save(inventory);
    }

    // 불러오기 버튼에 연결: 저장된 JSON 파일을 현재 인벤토리에 적용 후 UI 갱신
    public void LoadGame()
    {
        TestInventory inventory = TestInventory.Instance;
        if (inventory == null)
        {
            Debug.LogWarning("LoadGame: TestInventory 인스턴스를 찾을 수 없습니다.");
            return;
        }

        TestInventorySaveSystem.Load(inventory);

        if (inventoryUI != null)
            inventoryUI.RefreshInventory();
    }
}
