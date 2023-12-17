using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFreezable
{
    bool IsFrozen { get; set; }
    void Freeze(int damage)
    {
        IsFrozen = true;
    }

    void Unfreeze()
    {
        IsFrozen = false;
    }
}
