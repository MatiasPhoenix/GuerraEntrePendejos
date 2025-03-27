using System;
using UnityEngine;

public class EnemyUnit : BaseUnit
{
    [Header("Enemy Selected Object")]
    public GameObject enemySelectedObject;

    public void ActiveUnitSelected() => enemySelectedObject.SetActive(true);
    public void DesactiveUnitSelected() => enemySelectedObject.SetActive(false);
}
