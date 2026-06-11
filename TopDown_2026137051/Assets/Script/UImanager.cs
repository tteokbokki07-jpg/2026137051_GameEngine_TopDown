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
}
