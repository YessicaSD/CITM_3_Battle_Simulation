using UnityEngine.UI;
using UnityEngine;
public class Stab: BattleAction
{
    public Character enemy;
    float percentage = 1;
    public Stab(string name, ref Text log, Character enemy, float percentage): base(name, ref log)
    {
        this.enemy = enemy;
        this.percentage = percentage;
    }
    public override void Execute(Character activeCharacter)
    {
        base.Execute(activeCharacter);
        enemy.currentLife -= (activeCharacter.strength * percentage);
        if (enemy.currentLife < 0)
        {
            enemy.currentLife = 0;
        }
        enemy.UpdateLifeBar();
    }
}
public class DaggerCut : BattleAction
{
    public Character enemy;
    float damagePercentage = 1;
    float healPercentage = 1;
    float manaNeed = 1;
   
    public DaggerCut(string name, ref Text log, Character enemy, float damagePercentage, float healPercentage, float manaNeed) : base(name, ref log)
    {
        this.manaNeed = manaNeed;
        this.enemy = enemy;
        this.damagePercentage = damagePercentage;
        this.healPercentage = healPercentage;
    }
    public override void Execute(Character activeCharacter)
    {
        base.Execute(activeCharacter);
        float damage = (activeCharacter.strength * damagePercentage);
        enemy.currentLife -= damage;
        if (enemy.currentLife < 0)
        {
            enemy.currentLife = 0;
        }
        enemy.UpdateLifeBar();
        activeCharacter.currentLife += damage * healPercentage;
    }
    public override bool IsValid(Character activeCharacter)
    {
        return activeCharacter.currentMana >= manaNeed;
    }
}
public class GeaBlessing: AttackAction
{
    float percentagedamageToYourself;
    float percentageDamageToEnemy;
    float manaNeed;

   public GeaBlessing(string name, ref Text log, Character enemy, float percentagedamageToYourself, float percentageDamageToEnemy, float manaNeed) :base(name, ref log, ref enemy)
    {
        this.manaNeed = manaNeed;
        this.percentagedamageToYourself = percentagedamageToYourself;
        this.percentageDamageToEnemy = percentageDamageToEnemy;
    }
    public override void Execute(Character activeCharacter)
    {
        base.Execute(activeCharacter);
        activeCharacter.currentMana -= manaNeed;

        float randomValue = Random.Range(1, 6);
        if(randomValue > activeCharacter.level)
        {
            ReduceLife(activeCharacter, percentagedamageToYourself);
        }
        else
        {
            ReduceLife(enemy, percentageDamageToEnemy);
        }
        
    }
    public override bool IsValid(Character activeCharacter)
    {
        return activeCharacter.currentMana >= manaNeed;
    }
}
public class MainCharacterScript : Character
{
    [Header("Cut Paramenters")]
    public float percentageOfDamageAtCut;
    public float percentageOfHealAtCut;
    public float manaNeedCut;
    [Header("Gea Blessing Paramenters")]
    public float percentageOfDamagingYourself;
    public float percentageOfDamagingEnemy;
    public float manaNeedGeaBlessing;
    // Start is called before the first frame update
    void Start()
    {
        loadInfo();
        AllActions.Add(new Stab("stab", ref log, enemy, defaultAttackPercentage));
        AllActions.Add(new DaggerCut("dagger cut", ref log, enemy, percentageOfDamageAtCut, percentageOfHealAtCut, manaNeedCut));
        AllActions.Add(new GeaBlessing("Gea Blessing", ref log, enemy, percentageOfDamagingYourself, percentageOfDamagingEnemy, manaNeedGeaBlessing));
        AllActions.Add(new HealAction("heal", ref log));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
