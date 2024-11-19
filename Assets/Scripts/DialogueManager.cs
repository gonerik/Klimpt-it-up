using System.Collections;
using System.Collections.Generic;
using Intertables;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [Header("UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Dialogue Data")]
    public List<DialogueLine> dialogueLines;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;


    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // Add an AudioSource component in the inspector

    void Start()
    {
        Instance = this;
        dialoguePanel.SetActive(false); // Initially hide the dialogue panel
    }

    public void StartDialogue(List<DialogueLine> lines)
    {
        dialogueLines = lines;
        currentLineIndex = 0;
        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        CharacterController2D.Instance.setCanMove(false); // Disable player movement during the cutscene

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        // Stop any currently playing audio before proceeding
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (currentLineIndex < dialogueLines.Count)
        {
            DialogueLine line = dialogueLines[currentLineIndex];
            speakerNameText.text = line.speakerName;
            dialogueText.text = line.text;

            // If there is an audio clip, play it
            if (line.voiceLine != null)
            {
                audioSource.clip = line.voiceLine;
                audioSource.Play();
            }

            currentLineIndex++;
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        // Stop any audio still playing
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        CharacterController2D.Instance.setCanMove(true); // Re-enable player movement
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextLine();
        }
    }
}

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea] public string text;
    public AudioClip voiceLine;
}