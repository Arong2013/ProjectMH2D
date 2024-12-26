using UnityEngine;


public class MonsterPart : MonoBehaviour, ICombatable, IHarvestable
{
    [SerializeField] public int BasePartID;
    public MonsterMarcine monsterMarcine { get; private set; }
    MonsterPartData monsterPartData;

    public void Init(MonsterMarcine monsterMarcine,MonsterPartData  monsterPartData)
    {
        this.monsterMarcine = monsterMarcine;
        this.monsterPartData = monsterPartData;
    }
    public void TakeDamge(DamgeData damgeData)
    {
        var dmg = damgeData.Dmg * monsterPartData.DisDMG * 0.01f;
        monsterPartData.HP -= dmg;
        Instantiate(ParticleResourceData.Instance.GetParticle("Blood"), transform.position, Quaternion.identity);
        if (monsterPartData.HP <= 0)
            monsterMarcine.SetAnimatorValue(CharacterAnimeIntName.HitType, 1);
        monsterMarcine.TakeDamge(damgeData);
    }
    public bool CanBeHarvested()
    {
        return monsterMarcine.IsHarvest;
    }
    public void StartHarvest()
    {
        
    }
    public void EndHarvest()
    {
        
    }
    public int GetHarvestReward()
    {
        return Utils.GetRandomItemFromDropTable(monsterPartData.DropItems);
    }

    
}