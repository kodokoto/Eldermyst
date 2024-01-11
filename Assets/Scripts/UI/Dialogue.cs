using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int index;
    private List<string> dialogue; 

    private bool _isDialogueDoneDisplaying = true;

    public void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetDialogue(List<string> dialogue)
    {
        if (_isDialogueDoneDisplaying)
        {
            this.dialogue = dialogue;
            index = 0;
            ShowCurrentLine();
        }
    }

    public bool AdvanceLine()
    {
        if (index < dialogue.Count)
        {
            ShowCurrentLine();
            index++;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowCurrentLine()
    {
        StartCoroutine(DisplayText(dialogue[index]));
    }

    public IEnumerator DisplayText(string text)
    {
        _isDialogueDoneDisplaying = false;
        for (int i = 0; i < text.Length; i++)
        {
            this.text.text = text.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
        _isDialogueDoneDisplaying = true;
    }
}