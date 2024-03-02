using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daikyu : AbstractDamageAbility, ISustainableAbility
{
    protected GameObject projectilePrefab;
    protected float fullChargeTime;
    protected float fullChargeDamageMultiplier;

    protected bool stopSustaining;
    protected IDamageModifier damageModifier;

    public float FullChargeTime => fullChargeTime;
    public float PreCastDelay => preCastDelay;

    public Daikyu(MonoBehaviour owner, GameObject caster, DaikyuData daikyuData, IEnergyManager energyManager, IModifierManager modifierManager, ITurning turning, ITeam team) : base(owner, caster, daikyuData, energyManager, modifierManager, turning, team)
    {
        projectilePrefab = daikyuData.projectilePrefab;
        fullChargeTime = daikyuData.fullChargeTime;
        fullChargeDamageMultiplier = daikyuData.fullChargeDamageMultiplier;
    }

    public override void StartCast()
    {
        if (damageModifier != null)
        {
            modifierManager.RemoveModifier(damageModifier);
        }

        base.StartCast();
    }

    public override void BreakCast()
    {
        stopSustaining = false;

        base.BreakCast();
    }

    protected override IEnumerator ReleaseStrikeCoroutine()
    {
        float startChargeTime = Time.time;

        yield return new WaitUntil(() => (startChargeTime + fullChargeTime <= Time.time) || stopSustaining);

        float additionalDamage = (fullChargeDamageMultiplier - 1f) * Mathf.Clamp01((Time.time - startChargeTime) / fullChargeTime);
        damageModifier = new RelativeDamageModifier(1f + additionalDamage);

        modifierManager.AddModifier(damageModifier);

        IProjectile projectile = Object.Instantiate(projectilePrefab, caster.transform.position, Quaternion.Euler(new Vector3(0f, (float)turning.Direction, 0f))).GetComponent<IProjectile>();
        projectile.Release(caster, damageData, turning.Direction, team, modifierManager, this);

        energyManager.ChangeCurrentEnergy(-cost);
        finishCooldownTime = Time.time + cooldown;
        ReleaseCastEvent.Invoke();

        yield return new WaitForSeconds(postCastDelay);

        BreakCast();
    }

    public void StopSustaining()
    {
        if (IsPerforming)
        {
            stopSustaining = true;
        }            
    }
}
