using UnityEngine;

public class EndPoint : MonoBehaviour
{

    public GameObject MainEnemy;
    public GameObject winScreen;

    private void Update()
    {
       if (MainEnemy == null)
        {
            winScreen.SetActive(true);
        }
    }
}

