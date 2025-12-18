using System.Collections.Generic;
using System.Linq;

public class DamagableManager
{
    private List<ICharacter> _allDamagables = new List<ICharacter>();
    
    public List<ICharacter> GetAllDamagables() => _allDamagables.ToList();

    public void RegisterDamagable(ICharacter damagable)
    {
        if (!_allDamagables.Contains(damagable))
            _allDamagables.Add(damagable);
    }

    public void UnregisterDamagable(ICharacter damagable) => _allDamagables.Remove(damagable);
}
