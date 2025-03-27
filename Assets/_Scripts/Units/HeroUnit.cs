using _Scripts.Tiles;
using UnityEngine;

public class HeroUnit : BaseUnit
{
    [Header("Unit Selected Object")]
    public GameObject heroSelectedObject;

    public void ActiveUnitSelected() => heroSelectedObject.SetActive(true);
    public void DesactiveUnitSelected() => heroSelectedObject.SetActive(false);
}
