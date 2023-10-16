using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;

    enum AbilityState
    {
        Ready,
        Active,
        Cooldown,
    }

    private AbilityState state;

    public KeyCode key;


    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case AbilityState.Ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject);
                }
                break;
            case AbilityState.Active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Cooldown;
                    cooldownTime = ability.cooldownTime;
                }
                break;
            case AbilityState.Cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Ready;
                }
                break;

        }
    }
}
