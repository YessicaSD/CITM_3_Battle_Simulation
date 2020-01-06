using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turn : MonoBehaviour
{
    public List<Character> characters;
    bool startBattle = false;
    private void OrderCharacters()
    {
        characters.OrderByDescending(x => x.speed);
    }
    // Start is called before the first frame update
    void Start()
    {
        OrderCharacters();
    }
    private void Update()
    {
        if(startBattle)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if (characters[i].currentLife <= 0)
                {
                    startBattle = false;
                    Debug.Log("Battle End");
                    return;
                }
            }
                for (int i = 0; i < characters.Count; i++)
            {
                
                characters[i].currentMana += characters[i].manaRegeneration;
                if (characters[i].currentMana > characters[i].maxMana)
                {
                    characters[i].currentMana = characters[i].maxMana;
                }
                characters[i].UpdateManaBar();
                characters[i].AddPossibleActions();
                characters[i].ChooseAction().Execute(characters[i]);
            }
        }
    }
    public void ActiveTurn()
    {
        startBattle = true;
    
    }
}
