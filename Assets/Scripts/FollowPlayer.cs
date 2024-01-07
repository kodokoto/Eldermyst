using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);
        gameObject.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);
        gameObject.transform.position = pos;
    }
}
