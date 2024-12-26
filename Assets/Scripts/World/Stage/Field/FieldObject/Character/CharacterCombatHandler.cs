using System.Xml;
public class DamgeData
{
    public float Dmg;
    public int DamgeAnimeType;
    public object target;

    public DamgeData(float Dmg, int DamgeAnimeType, object target)
    {
        this.Dmg = Dmg;
        this.DamgeAnimeType = DamgeAnimeType;
        this.target = target;   
    }
}
public abstract class CharacterCombatHandler
{
    public abstract void TakeDamage(DamgeData damgeData);
}
