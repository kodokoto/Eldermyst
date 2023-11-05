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
        LevelUp = gameObject.GetComponent<TextMeshProUGUI>();
        LevelUp.enabled = false;
    }

    private IEnumerator ShowMessageCoroutine(int level, string levelInstructions)
    {
        LevelUp.enabled = true;
        LevelUp.text = "Level Up!!";
        yield return new WaitForSeconds(1f);
        LevelUp.text = "You are now level " + level + ". " + levelInstructions;
        yield return new WaitForSeconds(5.0f);
        LevelUp.enabled = false;
    }

    public void ShowMessage(int level, string levelInstructions = "")
    {
        Debug.Log("Showing level up message");
        StartCoroutine(ShowMessageCoroutine(level, levelInstructions));
    }
}