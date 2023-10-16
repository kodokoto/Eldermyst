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

    AbilityState state;

    KeyCode key;


    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case AbilityState.Ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate();
                }
                break;
            case AbilityState.Active:
                if (activeTimer > 0)
                {
                    activeTimer -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Cooldown;
                    cooldownTimer = ability.cooldownTime;
                }
                break;
            case AbilityState.Cooldown:
                if (cooldownTimer > 0)
                {
                    cooldownTimer -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Ready;
                }
                break;

        }
    }
}
