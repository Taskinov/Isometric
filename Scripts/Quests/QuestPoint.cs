using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    public bool playerIsNear = false;
    public string questId;
    public QuestState currentQuestState;
    private PlayerInput playerInput;
    [SerializeField] GameObject player;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void Start()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        playerInput = player.GetComponent<PlayerInput>();
    }

    private void OnDestroy()
    {
        if (GameEventsManager.instance != null)
        {
            GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        }
    }

    private void Update()
    {
        // For testing, trigger interactions when pressing space
        if (playerIsNear /*&& playerInput.actions["Action"].WasPressedThisFrame()*/)
        {
            if (currentQuestState == QuestState.CAN_START)
            {
                GameEventsManager.instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState == QuestState.CAN_FINISH)
            {
                GameEventsManager.instance.questEvents.FinishQuest(questId);
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id == questId)
        {
            currentQuestState = quest.state;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
