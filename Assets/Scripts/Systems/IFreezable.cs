using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFreezable
{
    public bool IsFrozen { get; set; }
    public void Freeze()
    {
        IsFrozen = true;
    }

    public void Unfreeze()
    {
        IsFrozen = false;
    }
}
