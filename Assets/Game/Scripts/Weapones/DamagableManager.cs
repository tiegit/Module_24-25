using System.Collections.Generic;
using System.Linq;

public class DamagableManager
{
    private List<IDamagable> _allDamagables = new List<IDamagable>();
    
    public List<IDamagable> GetAllDamagables() => _allDamagables.ToList();

    public void RegisterDamagable(IDamagable damagable)
    {
        if (!_allDamagables.Contains(damagable))
            _allDamagables.Add(damagable);
    }

    public void UnregisterDamagable(IDamagable damagable) => _allDamagables.Remove(damagable);
}
