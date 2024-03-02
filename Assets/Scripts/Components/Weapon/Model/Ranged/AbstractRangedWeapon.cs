using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRangedWeapon : AbstractWeapon
{
    protected Transform projectileSpawnPoint;

    protected GameObject projectilePrefab;

    protected AbstractRangedWeapon(MonoBehaviour owner, GameObject weaponOwner, IModifierManager weaponModifierManager, ITeam team, ITurning turning, Transform projectileSpawnPoint, RangedWeaponData rangedWeaponData) : base(owner, weaponOwner, rangedWeaponData, weaponModifierManager, team, turning)
    {
        this.projectileSpawnPoint = projectileSpawnPoint;
        projectilePrefab = rangedWeaponData.projectilePrefab;
    }
}
