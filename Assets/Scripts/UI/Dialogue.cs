using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI line;
    public GameObject panel;

    void Awake()
    {
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
        StartCoroutine(ShowMessageCoroutine(level, levelInstructions));
    }

    public void ShowStory(string[] story)
    {
        Debug.Log("Showing story message");
        panel.SetActive(true);
        StartCoroutine(ShowStoryCoroutine(story));
    }

    private IEnumerator ShowStoryCoroutine(string[] story)
    {
        Debug.Log("Coroutine started");
        for (int i=0; i<story.Length; i++)
        {
            line.text = story[i];
            yield return new WaitForSeconds(2f);
        }
        panel.SetActive(false);
    }
}