using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [Header("UI References")]
    [Tooltip("The GameObject containing the PNG background and the TextMeshPro text.")]
    [SerializeField] private GameObject dialogueBoxPrefab;
    
    [Header("Settings")]
    [Tooltip("How long the dialogue box stays on screen before disappearing (in seconds).")]
    [SerializeField] private float displayDuration = 3f;
    [Tooltip("Offset on the Y axis to place the dialogue box above the NPC's head.")]
    [SerializeField] private float yOffset = 4.5f;
    [SerializeField] Sprite boxM;
    [SerializeField] Sprite boxS;
    GameObject dialogueBox;

    private void Awake()
    {
        // Setup Singleton
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Spawns a dialogue box above the specified NPC.
    /// </summary>
    /// <param name="message">The text string to display.</param>
    /// <param name="npcTransform">The transform of the NPC saying the dialogue.</param>
    public void ShowDialogue(string message, Transform npcTransform)
    {
        // 1. Вычисляем позицию над головой NPC
        Vector3 spawnPosition = npcTransform.position + new Vector3(-2.5f, yOffset, 0);

        // 2. Создаем префаб диалогового окна в этой позиции
        dialogueBox = Instantiate(dialogueBoxPrefab, spawnPosition, Quaternion.identity);

        // 3. Устанавливаем текст
        TextMeshProUGUI textComponent = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        
        if (textComponent != null)
        {
            textComponent.text = message;
            textComponent.ForceMeshUpdate();
            int lineCount = textComponent.textInfo.lineCount;
            if (lineCount == 2)
            {
                dialogueBox.GetComponent<SpriteRenderer>().sprite = boxM;
            } else if (lineCount == 1)
            {
                dialogueBox.GetComponent<SpriteRenderer>().sprite = boxS;
            }
        }
        else
        {
            Debug.LogError("DialogueManager: Не найден компонент TextMeshPro в префабе dialogueBoxPrefab!");
        }

        // 4. Привязываем окно к NPC, чтобы оно двигалось вместе с ним
        dialogueBox.transform.SetParent(npcTransform);

        // 5. Запускаем таймер на удаление окна
        StartCoroutine(DestroyDialogueBoxAfterTime(dialogueBox));
    }

    private IEnumerator DestroyDialogueBoxAfterTime(GameObject dialogueBox)
    {
        yield return new WaitForSeconds(displayDuration);
        
        if (dialogueBox != null)
        {
            Destroy(dialogueBox);
        }
    }
}
