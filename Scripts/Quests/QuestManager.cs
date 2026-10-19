using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    // quest start requirements
    private int currentPlayerLevel;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void Start()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;

        // broadcast the initial state of all quests on startup
        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void OnDestroy()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
            GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
            GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
        }
    }

    private void Update()
    {
        // Loop through all quests
        foreach (Quest quest in questMap.Values)
        {
            // if we're now meeting the requirements, switch over to the CAN_START state
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        // Check player level
        if (currentPlayerLevel < quest.info.levelRequirement)
        {
            return false;
        }

        // Check prerequisites
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                return false;
            }
        }

        return true;
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private void StartQuest(string id) 
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);////////////////////////////////////////////////////////////////////
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        // Add functionality to give gold and experience
        Debug.Log("Claimed rewards for quest " + quest.info.id);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        // Loads all QuestInfoSO scriptable objects under the Resources/Quests folder
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }
}
