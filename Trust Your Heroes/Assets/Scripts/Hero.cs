using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script attached to every hero, contains information about hero

public class Hero : MonoBehaviour
{
    public int health = 100;
    //Dmg reduction
    public int armor = 0;
    public int magicResistance = 0;
    //Dmg
    public int mainAttackDmg = 20;
    public int ability1Dmg = 20;
    public int ability2Dmg = 20;
    //Range
    public int mainAttackRange = 1;
    public int ability1Range = 2;
    public int ability2Range = 2;
    //Type of attack, true if magic, false if regular
    public bool mainAttackType = false;
    public bool ability1Type = false;
    public bool ability2Type = false;
    //Effects
    public string[] mainAttackEffects;
    public string[] ability1Effects;
    public string[] ability2Effects;
    //True if ability2 is passive
    public bool ability2Passive = false;

    //Which ability is being used
    public static int abilityType = -1;

    //Description
    public string heroName = "";
    public string description = "";
    public string mainAttack = "";
    public string firstAbility = "";
    public string secondAbility = "";

    private Slider healthBar;
    //True if hero isn't spawned in heroes panel or play panel
    private bool game = false;

    void Start()
    {
        abilityType = -1;
        //Find health bar
        string s = name + "(Clone)";
        if (YourHeroTeam.heroNames[0] == s)
        {
            healthBar = GameObject.Find("HealthBar1").GetComponent<Slider>();
            game = true;
        }
        else if (YourHeroTeam.heroNames[1] == s)
        {
            healthBar = GameObject.Find("HealthBar2").GetComponent<Slider>();
            game = true;
        }
        else if (YourHeroTeam.heroNames[2] == s)
        {
            healthBar = GameObject.Find("HealthBar3").GetComponent<Slider>();
            game = true;
        }
        else if (YourHeroTeam.heroNames[3] == s)
        {
            healthBar = GameObject.Find("HealthBar4").GetComponent<Slider>();
            game = true;
        }
        if (game)
            healthBar.maxValue = health;
    }

    void Update()
    {
        //Death
        if(health <= 0)
        {
            SpawnGrid.cells[GetComponent<MoveHero>().GetX(), GetComponent<MoveHero>().GetZ()].tag = "Cell";
            GetComponent<Animator>().SetBool("Death", true);
            StartCoroutine(Death());
        }
    }

    //Destroy hero after animation ends
    IEnumerator Death()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    //Update healthbar
    void FixedUpdate()
    {
        if (game)
            healthBar.value = health;
    }

    //Done before every attack
    void ReadyForAttack(int a)
    {
        PlaceHero.movement = false;
        if (PlaceHero.heroIsSelected) //If there was a selected hero, turn off lights of his available cells
        {
            GetComponent<MoveHero>().TurnOffLights();
        }
        else
            PlaceHero.heroIsSelected = true;
        PlaceHero.heroSelected = gameObject;
        abilityType = a;
    }

    //Main attack
    public void MainAttack()
    {
        ReadyForAttack(0);
        FindEnemies(mainAttackRange);
    }

    //Ability1
    public void Ability1()
    {
        ReadyForAttack(1);
    }

    //Ability2
    public void Ability2()
    {
        if (!ability2Passive)
        {
            ReadyForAttack(2);
        }
    }

    //Find enemies and light up available cells
    void FindEnemies(int range)
    {
        for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
        {
            for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
            {
                if(SpawnGrid.cells[i, j].tag == "EnemyCell" && (Mathf.Abs(GetComponent<MoveHero>().GetX() - i) + Mathf.Abs(GetComponent<MoveHero>().GetZ() - j)) <= range)
                {
                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                }
            }
        }
    }
}
