using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InvisibilityData", menuName = "Data/Abilities/Invisibility/New Invisibility Data")]
public class InvisibilityData : BaseAbilityData
{
    [Header("Invisibility")]
    [Min(0f)]
    public float duration = 10f;
    [Min(0f)]
    public float fadeTime = 1f;
    [Min(0f)]
    public float appearanceTime = 1f;
    [Min(0.01f)]
    public float checkFinishEffectPeriod = 1f;

    public float visibilityRate = 0.03f;
    public float refreshRate = 0.02f;
}
