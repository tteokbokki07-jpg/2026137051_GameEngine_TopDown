using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public List<QuestData> questList = new List<QuestData>();

    public int currentQuestIndex = -1;

    private void Start()
    {
        StartQuest(0);
    }

    public void StartQuest(int index)
    {
        currentQuestIndex = index;

        Debug.Log("퀘스트 시작 : " +
                  questList[currentQuestIndex].questName);
    }
}