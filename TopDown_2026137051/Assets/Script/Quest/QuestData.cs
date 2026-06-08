using System;
using UnityEngine;

[Serializable]
public class QuestData
{
    public int questID;

    public string questName;

    [TextArea]
    public string questDescription;
}