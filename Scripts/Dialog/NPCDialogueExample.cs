using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogueExample : MonoBehaviour
{
    public string[] dialogueText = {"Привет, путник!", "Когда-то и меня вела дорога приключений", "Как дела? У меня хорошо"};
    public int dialogLine = 0;
    private bool isSpeaking = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) 
        {
            if ((DialogueManager.instance != null) && (!isSpeaking))
            {
                DialogueManager.instance.ShowDialogue(dialogueText[dialogLine], this.transform);
                dialogLine = (dialogLine >= dialogueText.Length - 1) ? 0 : dialogLine + 1;
                isSpeaking = true;
                Invoke("SpeakCD", 3f);
            }
        }
    }

    private void SpeakCD()
    {
        isSpeaking = false;
    }
}
