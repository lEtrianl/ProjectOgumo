using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationAbility : AbstractAbility, ISustainableAbility
{
    protected RegenerationAbilityData regenerationAbilityData;
    protected float healthPerSecond;
    protected float maxRegenerationTime;
    protected float impactPeriod;

    protected IHealthManager healthManager;

    public override bool CanBeUsed => IsPerforming == false && IsOnCooldown == false && energyManager.Energy.currentEnergy >= cost * impactPeriod && healthManager.Health.currentHealth < healthManager.Health.maxHealth;


    public RegenerationAbility(MonoBehaviour owner, RegenerationAbilityData regenerationAbilityData, IEnergyManager energyManager, IHealthManager healthManager) : base(owner, regenerationAbilityData, energyManager)
    {
        this.regenerationAbilityData = regenerationAbilityData;
        healthPerSecond = regenerationAbilityData.healthPerSecond;
        maxRegenerationTime = regenerationAbilityData.maxRegenerationTime;
        impactPeriod = regenerationAbilityData.impactPeriod;

        this.healthManager = healthManager;
    }

    protected override IEnumerator ReleaseStrikeCoroutine()
    {
        float endCastTime = Time.time + maxRegenerationTime;
        finishCooldownTime = Time.time + cooldown;

        while (Time.time < endCastTime && energyManager.Energy.currentEnergy > cost * impactPeriod && healthManager.Health.currentHealth < healthManager.Health.maxHealth)
        {
            healthManager.ChangeCurrentHealth(healthPerSecond * impactPeriod);

            energyManager.ChangeCurrentEnergy(-cost * impactPeriod);

            ReleaseCastEvent.Invoke();

            yield return new WaitForSeconds(impactPeriod);
        }
    }

    public void StopSustaining()
    {
        BreakCast();
    }
}
