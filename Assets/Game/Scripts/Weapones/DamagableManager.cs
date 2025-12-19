using System.Collections.Generic;
using System.Linq;

public class DamagableManager
{
    private List<IMovable> _allDamagables = new List<IMovable>();
    
    public List<IMovable> GetAllDamagables() => _allDamagables.ToList();

    public void RegisterDamagable(IMovable damagable)
    {
        if (!_allDamagables.Contains(damagable))
            _allDamagables.Add(damagable);
    }

    public void UnregisterDamagable(IMovable damagable) => _allDamagables.Remove(damagable);
}
