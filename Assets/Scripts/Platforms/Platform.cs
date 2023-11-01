using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform : MonoBehaviour
{
    public BoxCollider col;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        gameObject.layer = LayerMask.NameToLayer("Ground");
    }

}
