using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{

    public Dialogue story;
    public string[] Lines;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Story interacted");
            story.ShowStory(Lines);
        }
        Destroy(gameObject);
    }
}
