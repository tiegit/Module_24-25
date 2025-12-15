using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    [SerializeField] private List<Mine> _mines = new List<Mine>();

    public void Initialize(DamagableManager damagableManager) => InitializeAllMines(damagableManager);

    private void InitializeAllMines(DamagableManager damagableManager)
    {
        foreach (Mine mine in _mines)
        {
            if (mine != null)
                mine.Initialize(damagableManager);
        }
    }
}