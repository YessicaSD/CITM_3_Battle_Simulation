using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleAction:MonoBehaviour
{
    public virtual void Execute(Character activeCharacter)
    {}
    public virtual bool IsValid(Character activeCharacter)
    { return true; }
};

public class AttackAction: BattleAction
{
    public Character enemy;
    float percentage = 1;
    public AttackAction(Character enemy, float percentage)
    {
        this.enemy = enemy;
        this.percentage = percentage;
    }
    public AttackAction(Character enemy)
    {
        this.enemy = enemy;
    }
    public override void Execute(Character activeCharacter)
    {
        
        Debug.Log(activeCharacter.name + "is attacking");
        if (enemy.defence >= activeCharacter.strength)
        {
            return;
        }
        enemy.currentLife -= (activeCharacter.strength*percentage - enemy.defence);
        if(enemy.currentLife<0)
        {
            enemy.currentLife = 0;
        }
            enemy.UpdateLifeBar();
    }

}
public class SpecialAttackAction: AttackAction
{
    public string name;
    public float manaNeed;
    public float damage;
    public float heal;
    public SpecialAttackAction(string name,float damage,float manaNeed, float heal, Character enemy) : base(enemy)
    {
        this.damage = damage;
        this.manaNeed = manaNeed;
        this.name = name;
    }
    public override void Execute(Character activeCharacter)
    {
        Debug.Log(activeCharacter.name + "have used " + name);
        if(damage>0)
        {
            if (enemy.defence >= damage)
            {
                return;
            }
            enemy.currentLife -= (damage - enemy.defence);
            if (enemy.currentLife < 0)
            {
                enemy.currentLife = 0;
            }
            enemy.UpdateLifeBar();
        }
     
        activeCharacter.currentMana -= manaNeed;
        if(activeCharacter.currentMana<0)
        {
            activeCharacter.currentMana = 0;
        }
       
        activeCharacter.UpdateManaBar();
    }
    public override bool IsValid(Character activeCharacter)
    {
        return activeCharacter.currentMana>=manaNeed;
    }
}

public class DefenceAction: BattleAction
{
    public override void Execute(Character activeCharacter)
    {}
}

public class HealAction : BattleAction
{
    public override void Execute(Character activeCharacter)
    {
        Debug.Log(activeCharacter.name + "is healing");
        activeCharacter.currentLife += activeCharacter.healingValue;
        if(activeCharacter.currentLife> activeCharacter.life)
        {
            activeCharacter.currentLife = activeCharacter.life;
        }
        activeCharacter.UpdateLifeBar();
    }
    public override bool IsValid(Character activeCharacter)
    {
        return activeCharacter.currentLife != activeCharacter.life;
    }
}
[System.Serializable]
public struct SpecialAttackInfo
{
    public string name;
    public float manaNeed;
    public float damage;
    public float heal;
    public SpecialAttackInfo(string name, float manaNeed, float damage, float heal)
    {
        this.name = name;
        this.manaNeed = manaNeed;
        this.damage = damage;
        this.heal = heal;
    }
}
public class Character : MonoBehaviour
{
    Text nameTxt;
    public string name;

    public float life;
    [HideInInspector] public float currentLife;
    public float maxMana;
    [HideInInspector] public float currentMana;

    public float strength;
    public float defence;
    public float speed;
    public float healingValue;
    public float manaRegeneration;
    public Character enemy;
    public float defaultDamagePercentage;
    public List<SpecialAttackInfo> specialAttacks = new List<SpecialAttackInfo>();

    List<BattleAction> AllActions = new List<BattleAction>();
    List<BattleAction> PossibleActions = new List<BattleAction>();

    [HideInInspector] public Slider lifeBar;
    [HideInInspector] public Slider ManaBar;
    
    // Start is called before the first frame update
    void Start()
    {
        nameTxt = gameObject.GetComponent<Text>();
        nameTxt.text = name;
        foreach (SpecialAttackInfo attack in specialAttacks)
        {
            AllActions.Add(new SpecialAttackAction(attack.name, attack.damage, attack.manaNeed, attack.heal, enemy));
        }
      

        AllActions.Add(new AttackAction(enemy, defaultDamagePercentage));
        AllActions.Add(new HealAction());
        currentLife = life;
        lifeBar = transform.Find("LifeBar").GetComponent<Slider>();
        ManaBar = transform.Find("ManaBar").GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void AddPossibleActions()
    {
        PossibleActions.Clear();
        for (int i = 0; i < AllActions.Count; ++i)
        {
            if(AllActions[i].IsValid(this))
            {
                PossibleActions.Add(AllActions[i]);
            }
        }
      
    }
    public void UpdateLifeBar()
    {
        lifeBar.value = currentLife / life;
       
    }
    public void UpdateManaBar()
    {
        ManaBar.value = currentMana / maxMana;
    }
    public BattleAction ChooseAction()
    {
        return PossibleActions[Random.Range(0, PossibleActions.Count)];
    }
}
