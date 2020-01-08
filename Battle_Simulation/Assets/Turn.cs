using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Turn : MonoBehaviour
{
    public List<Character> characters;
    bool startBattle = false;
    bool doRound = true;
    public int numberOfRounds;
    int current_round = 0;
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
            if(doRound)
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].currentLife <= 0)
                    {
                        doRound = false;
                        Debug.Log(characters[i].name + "have death");

                        //Debug.Log("Battle End");
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
            else
            {
                ++current_round;
                doRound = true;
                if(current_round > numberOfRounds)
                {
                    current_round = 0;
                    startBattle = false;
                    for (int i = 0; i < 1; i++)
                    {
                        characters[i].Report();
                        characters[i].numOfTimesLose = 0;
                        characters[i].numOfTimesWin = 0;
                    }
                    
                }
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].currentLife <= 0)
                    {
                        characters[i].numOfTimesLose++;
                    }
                    else
                    {
                        characters[i].numOfTimesWin++;
                    }
                        characters[i].ResetValues();

                }
            }
           
        }
    }
    public void ActiveTurn()
    {
        if(!startBattle)
        {
            startBattle = true;
           
        }
    }
}
