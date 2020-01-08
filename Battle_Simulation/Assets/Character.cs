using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;



public class BattleAction:MonoBehaviour
{
    public string name;
    public Text log;

    public BattleAction(string name, ref Text log)
    {
        this.name = name;
        this.log = log;
    }
    public virtual void Execute(Character activeCharacter)
    {
        //log.text += "\n " + activeCharacter.name + " have used " + name;
        // Debug.Log(activeCharacter.name + "have used " + name);
    }
    public virtual bool IsValid(Character activeCharacter)
    { return true; }
};

public class AttackAction: BattleAction
{
    public Character enemy;

    public AttackAction(string name, ref  Text log,ref Character enemy): base(name, ref log)
    {
        this.enemy = enemy;
    }

    public override void Execute(Character activeCharacter)
    {
        base.Execute(activeCharacter);
    }
    public void ReduceLife(Character activeCharacter, float percentage)
    {
        activeCharacter.currentLife -= activeCharacter.strength * percentage;
        if (activeCharacter.currentLife < 0)
        {
            activeCharacter.currentLife = 0;
        }
        activeCharacter.UpdateLifeBar();
    }
}
public class DefaultAttack: AttackAction
{
    float percentageDamage;
    public DefaultAttack(string name, ref Text log, Character enemy, float damage) : base(name, ref log, ref enemy)
    {
        this.percentageDamage = damage;
    }
    public override void Execute(Character activeCharacter)
    {
        base.Execute(activeCharacter);
        enemy.currentLife -= activeCharacter.strength * percentageDamage;
        if(enemy.currentLife<0)
        {
            enemy.currentLife = 0;
        }
    }
}
public class SpecialAttackAction: AttackAction
{
    public string name;
    public float manaNeed;
    public float damage;
    public float heal;

    public SpecialAttackAction(string name, ref Text log , float damage,float manaNeed, float heal, Character enemy) : base(name, ref log, ref enemy)
    {
        this.damage = damage;
        this.manaNeed = manaNeed;
        this.name = name;
    }
    public override void Execute(Character activeCharacter)
    {
        base.Execute(activeCharacter);
       // Debug.Log(activeCharacter.name + "have used " + name);
        if(damage>0)
        {
            enemy.currentLife -= (damage);
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

public class HealAction : BattleAction
{
    public HealAction(string name, ref Text log) : base(name, ref log)
    { }
    public override void Execute(Character activeCharacter)
    {
        base.Execute(activeCharacter);
        if (activeCharacter.currentLife == 0) 
        {
            return;
        }
        activeCharacter.currentLife += activeCharacter.healingValue;
        if(activeCharacter.currentLife> activeCharacter.maxLife)
        {
            activeCharacter.currentLife = activeCharacter.maxLife;
        }
        activeCharacter.UpdateLifeBar();
    }
    public override bool IsValid(Character activeCharacter)
    {
        return activeCharacter.currentLife != activeCharacter.maxLife;
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
    [HideInInspector] public Text nameTxt;
    public string name;
    public Text log;

    public int level = 1;
    public float maxLife;
    public float maxMana;
    [HideInInspector] public float currentLife;
    [HideInInspector] public float currentMana;
    public float strength;
    public float speed;
    public float healingValue;
    public float manaRegeneration;
    public Character enemy;
    
    public List<SpecialAttackInfo> specialAttacks = new List<SpecialAttackInfo>();

    [HideInInspector] public int numOfTimesWin = 0;
    [HideInInspector] public int numOfTimesLose = 0;
    [HideInInspector] public int numOfTimesSimulations = 0;

    [HideInInspector] public List<BattleAction> AllActions = new List<BattleAction>();
    [HideInInspector] public List<BattleAction> PossibleActions = new List<BattleAction>();

    [HideInInspector] public Slider lifeBar;
    [HideInInspector] public Slider ManaBar;

    [Header("Default Attack")]
    public float defaultAttackPercentage;
    public void loadInfo()
    {
        GameObject goText = GameObject.Find("logText");
        log = goText.GetComponent<Text>();
        nameTxt = gameObject.GetComponent<Text>();
        nameTxt.text = name;
       
        lifeBar = transform.Find("LifeBar").GetComponent<Slider>();
        ManaBar = transform.Find("ManaBar").GetComponent<Slider>();
        ResetValues();
    }
    void Start()
    {
        loadInfo();

        foreach (SpecialAttackInfo attack in specialAttacks)
        {
            AllActions.Add(new SpecialAttackAction(attack.name, ref log, attack.damage, attack.manaNeed, attack.heal, enemy));
        }
        AllActions.Add(new DefaultAttack("Basic Attack", ref log,enemy, defaultAttackPercentage));
        AllActions.Add(new HealAction("Heal", ref log));
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
        lifeBar.value = currentLife / maxLife;
       
    }
    public void UpdateManaBar()
    {
        ManaBar.value = currentMana / maxMana;
    }
    public BattleAction ChooseAction()
    {
        return PossibleActions[Random.Range(0, PossibleActions.Count)];
    }
    public void ResetValues()
    {
        currentLife = maxLife;
        currentMana = maxMana;
        UpdateLifeBar();
        UpdateManaBar();
    }
    public virtual void Report()
    {
        //"Character name",
        //"Hp",
        //"Strenght",
        //"Speed",
        //"Num Wins",
        //"Num Defeats",
        //"Num simulations",
        //"TimeStamp"
        string[] content = new string[7]
        {
            name,
            maxLife.ToString(),
            strength.ToString(),
            speed.ToString(),
            numOfTimesWin.ToString(),
            numOfTimesLose.ToString(),
            (numOfTimesWin + numOfTimesLose).ToString(),
        };
        ExcelExporter.AppendToReport(content);
    }
}
