using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI line;
    public GameObject panel;
    public GameObject button;
    private int index;
    private string[] story; 

    void Awake()
    {
        button.SetActive(false);
        panel.SetActive(false);
    }

    private IEnumerator ShowMessageCoroutine(int level, string levelInstructions)
    {
        Debug.Log("Coroutine started");
        Debug.Log("Level: " + level);
        line.text = "Level Up!!";
        yield return new WaitForSeconds(1f);
        line.text = "You are now level " + level + ". " + levelInstructions;
        yield return new WaitForSeconds(5.0f);
        panel.SetActive(false);
    }

    public void ShowMessage(int level, string levelInstructions = "")
    {
        Debug.Log("Showing level up message");
        panel.SetActive(true);
        button.SetActive(false);
        StartCoroutine(ShowMessageCoroutine(level, levelInstructions));
    }

    public void ShowStory(string[] Lines)
    {
        Debug.Log("Showing story message");
        StopAllCoroutines();
        panel.SetActive(true);
        button.SetActive(true);
        story = Lines;
        index = 0;
        printNextLine();
    }

    public void printNextLine()
    {
        if (index < story.Length)
        {
            line.text = story[index];
            index++;
        }
        else
        {
            button.SetActive(false);
            panel.SetActive(false);
        }
    }

}