using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;
    private int currentQuestStepIndex;
    
    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
    }
    
    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }
    
    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public string GetCurrentStepName()
    {
        if (info.questStepNames != null && currentQuestStepIndex < info.questStepNames.Length)
        {
            return info.questStepNames[currentQuestStepIndex];
        }
        return "В процессе..."; // Резервный текст, если вы забыли вписать название шага
    }
    
    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id);
        }
    }
    
    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else 
        {
            Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that there's no current step: QuestId=" + info.id + ", stepIndex=" + currentQuestStepIndex);
        }
        return questStepPrefab;
    }
}
