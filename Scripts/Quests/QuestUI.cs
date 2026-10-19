using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questLogText;
    [SerializeField] private TextMeshProUGUI questStepText; // Drag your UI Text element here in the Inspector

    // A dictionary to keep track of the text we want to show for each quest ID
    private Dictionary<string, string> questDisplayStrings = new Dictionary<string, string>();

    private void Start()
    {
        // Subscribe to the quest state change event
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDestroy()
    {
        // Unsubscribe when the UI is destroyed
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // We only want to show quests that are in progress or ready to finish
        if (quest.state == QuestState.IN_PROGRESS || quest.state == QuestState.CAN_FINISH)
        {
            // Format how it looks: "Find the Sword (IN_PROGRESS)"
            string status = quest.state == QuestState.CAN_FINISH ? " (Ready to Turn In!)" : "";
            //questDisplayStrings[quest.info.id] = quest.info.displayName + status;
            string currentObjective = quest.GetCurrentStepName();
            questDisplayStrings[quest.info.id] = quest.info.displayName + ": " + currentObjective + status;
        }
        else
        {
            // If the quest is finished or not started yet, remove it from the log
            if (questDisplayStrings.ContainsKey(quest.info.id))
            {
                questDisplayStrings.Remove(quest.info.id);
            }
        }

        UpdateUI(quest);
    }

    private void UpdateUI(Quest quest)
    {
        questLogText.text = "<b>Задачи:</b>\n";
        
        if (questDisplayStrings.Count == 0)
        {
            questLogText.text += "Нет задач.";
            return;
        }

        // Loop through our tracked quests and add them to the text block
        foreach (string questString in questDisplayStrings.Values)
        {
            questLogText.text += "- " + questString + "\n";
        }
        questStepText.text = quest.GetCurrentStepName();
    }
}
