using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueBase : QuestStep
{
    private GameObject player;
    private string[] dialogueLines;
    public string fileName; 
    public TextAsset textFile;
    public GameObject canvas;
    private PlayerInput playerInput;
    public bool startDialogue = false;
    public bool dialogueFinished = false;
    private bool skipLine = false;
    private float duration = 3.5f;
    private float elapsed = 0f;
    private bool inReach = false;
    
    private void Start()
    {
        PlayerPrefs.SetString("Localisation", "RU");
        fileName = $"DialogueNPC/{name}{PlayerPrefs.GetString("Localisation")}";
        textFile = Resources.Load<TextAsset>(fileName);
        player = GameObject.Find("Player");
        playerInput = player.GetComponent<PlayerInput>();
        canvas = GameObject.Find("Canvas");
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && (canvas != null))
        {
            canvas.GetComponent<HideUI>().TalkAction();
            inReach = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && (canvas != null))
        {
            canvas.GetComponent<HideUI>().HitAction();
            inReach = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if ((playerInput.actions["Action"].WasPressedThisFrame()) && (collider.CompareTag("Player")))
        {
            if (!startDialogue)
            {
                startDialogue = true;
                if (textFile != null)
                {
                    dialogueLines = textFile.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
                }
                StartCoroutine(PlayDialogueSequence());
            } else 
            {
                skipLine = true;
            }
        }
    }
    private IEnumerator PlayDialogueSequence()
    {
        if (textFile == null)
        {
            Debug.LogError("DialogueQuestStep: не назначен текстовый файл или NPC!");
            yield break;
        }
        foreach (string line in dialogueLines)
        {
            // if (skipLine)
            // {
            //     skipLine = false;
            //     continue;
            // }
            if (line[0] == 'P')
            {
                DialogueManager.instance.ShowDialogue(line.Substring(1), player.transform);
            } else
            {
                DialogueManager.instance.ShowDialogue(line, this.transform);
            }
            while (elapsed < duration)
            {
                // Если нажата кнопка пропуска — выходим из цикла ожидания досрочно
                if (skipLine)
                {
                    //DialogueManager.DestroyDialogueBox();
                    if ((line[0] == 'P') && (player.transform.Find("Dialog(Clone)").gameObject != null))
                    {
                        Destroy(player.transform.Find("Dialog(Clone)").gameObject);
                    } else if (transform.Find("Dialog(Clone)").gameObject != null)
                    {
                        Destroy(transform.Find("Dialog(Clone)").gameObject);
                    }
                    break; 
                }
                elapsed += Time.deltaTime;
                new WaitForSeconds(0.1f);
                yield return null;
                 // Ждем следующий кадр
            }
            skipLine = false;
            elapsed = 0f;
            if (!inReach)
            {
                startDialogue = false;
                yield break;
            }
            //yield return null;
            //yield return new WaitForSeconds(3.5f);
        }
        startDialogue = false;
        dialogueFinished = true;
    }
}
