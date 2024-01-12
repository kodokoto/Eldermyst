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
        index++;
    }

    public IEnumerator DisplayText(string t)
    {
        _isDialogueDoneDisplaying = false;
        for (int i = 0; i < t.Length; i++)
        {
            this.text.text = t.Substring(0, i+1);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        _isDialogueDoneDisplaying = true;
    }
}