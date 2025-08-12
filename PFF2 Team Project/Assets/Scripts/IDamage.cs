using UnityEngine;

public interface IDamage 
{
    public void takeDamage(int amount);
    public void takeSlow(int amount, float slowtime);
    
}
