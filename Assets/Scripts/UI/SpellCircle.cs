using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCircle : MonoBehaviour
{
    public GameObject circle;
    // Start is called before the first frame update
    void Awake()
    {
        circle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            circle.SetActive(true);
        }
        else
        {
            circle.SetActive(false);
        }
    }

}
