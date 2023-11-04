using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpMessage : MonoBehaviour
{
    TextMeshProUGUI LevelUp;

    void Start()
    {
        LevelUp = GetComponentInChildren<TextMeshProUGUI>();
        LevelUp.enabled = false;
    }

    private IEnumerator ShowMessageCoroutine(int level, string levelInstructions)
    {
        LevelUp.enabled = true;
        LevelUp.text = "Level Up!!";
        yield return new WaitForSeconds(1.5f);
        LevelUp.text = "You are now level " + level + ". " + levelInstructions;
        yield return new WaitForSeconds(3.0f);
        LevelUp.enabled = false;
    }

    public void ShowMessage(int level, string levelInstructions = "")
    {
        StartCoroutine(ShowMessageCoroutine(level, levelInstructions));
    }
}