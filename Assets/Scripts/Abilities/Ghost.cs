using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Ghost : Spell
{
    public override void Activate(GameObject parent)
    {
        Debug.Log("Ghost spell activated");
        parent.GetComponent<IGhost>().Immaterialize();
        parent.GetComponent<Renderer>().material.color = Color.black;
    }

    public override void Deactivate(GameObject parent)
    {
        Debug.Log("Ghost spell deactivated");
        parent.GetComponent<IGhost>().Materialize();
        parent.GetComponent<Renderer>().material.color = Color.white;
    }
    // public LayerMask StickyTarget;
    // public LayerMask WallTarget;
    // private Collider[] StickyCheck;
    // private Collider[] WallCheck;


    // // Start is called before the first frame update
    // public override void Activate(GameObject parent)
    // {
    //     Player player = parent.GetComponent<Player>();
    //     StickyCheck = Physics.OverlapSphere(player.transform.position, radius, StickyTarget);
    //     WallCheck = Physics.OverlapSphere(player.transform.position, radius, WallTarget);
    //     if (StickyCheck.Length != 0)
    //     {
    //         for(int i=0; i < StickyCheck.Length; i++)
    //         {
    //             Debug.Log("Sticky wall here");
    //             StickyCheck[i].isTrigger = true;

    //         }
    //     }
    //     if (WallCheck.Length != 0)
    //     {
    //         for(int i=0; i < WallCheck.Length; i++)
    //         {
    //             GameObject p = WallCheck[i].gameObject;
    //             if (p.transform.localScale.y > p.transform.localScale.x)
    //             {
    //                 Debug.Log("Wall here");
    //                 WallCheck[i].isTrigger = true;

    //             }
    //         }
    //     }

    // }

    // // Update is called once per frame
    // public override void Deactivate(GameObject parent)
    // {
    //     Debug.Log("Spell done");
    //     for(int i = 0; i < StickyCheck.Length; i++)
    //     {
    //         StickyCheck[i].isTrigger = false;
    //     }
    //     for (int i=0;i< WallCheck.Length; i++)
    //     {
    //         WallCheck[i].isTrigger = false;
    //     }
    // }
}
