using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that is connected to every gridcell, when clicked on cell selected hero moves

public class PlaceHero : MonoBehaviour
{
    //Coordinates
    private int x = 0;
    private int z = 0;

    //For movement
    public int distance = 0;

    //Static variables
    public static bool gameBegun = false; //When Ready button is clicked the game starts
    public static bool heroIsSelected = false; //true if a hero is selected
    public static bool movement = true; //true if movement, false if attack
    public static GameObject heroSelected = null; //The selected hero
    
    void Start()
    {
        heroSelected = null;
        gameBegun = false;
        heroIsSelected = false;
    }

    void Update()
    {
        if (gameBegun) //When game starts there is no need for FirstRowCell tag
        {
            if(tag == "FirstRowCell")
                tag = "Cell";
        }
    }

    void OnMouseDown()
    {
        if (heroIsSelected)
        {
            if (movement) //if movement, else attack
            {
                if (tag == "FirstRowCell") //If the selected cell has FirstRowCell tag the game hasn't started yet, spawns hero on cell
                {
                    heroIsSelected = false;
                    if (heroSelected) //If a hero is selected on the board, move him from his current cell
                    {
                        //Change tag from OccupiedCell to FirstRowCell
                        SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag = "FirstRowCell";
                        //Move
                        MoveSelectedHero();
                    }
                    else //else spawn hero of the selected hero icon
                    {
                        heroSelected = Instantiate(Resources.Load<GameObject>("Models/Heroes/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1] + "/source/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1]), transform.position, Quaternion.identity);
                        heroSelected.GetComponent<MoveHero>().SetCoordinates(x, z);
                        PlaceHeroButtons.spawned = true;
                        PlaceHeroButtons.heroesPlaced++;
                    }
                    //Turn off lights
                    GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
                    for (int i = 0; i < firstRowCells.Length; i++)
                    {
                        firstRowCells[i].GetComponentInChildren<Light>().intensity = 0;
                    }
                    tag = "OccupiedCell";
                }
                if (gameBegun) //If game has begun, move hero to cell if it is available
                {
                    if (GetComponentInChildren<Light>().intensity == 15) //Checks if cell is available
                    {
                        MoveSelectedHero();
                        tag = "OccupiedCell";
                    }
                }
            }
            else //for attacks
            {
                if (GetComponentInChildren<Light>().intensity == 15)
                {
                    heroSelected.GetComponent<MoveHero>().TurnOffLights();
                    ChangeRotationOfHero();
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
                    GameObject enemy = null;
                    for (int i = 0; i < enemies.Length; i++)
                        if (enemies[i].GetComponent<MoveHero>().GetX() == x && enemies[i].GetComponent<MoveHero>().GetZ() == z)
                        {
                            enemy = enemies[i];
                            break;
                        }
                    if (Hero.abilityType == 0)
                    {
                        heroSelected.GetComponent<Animator>().SetTrigger("MainAttack");
                        StartCoroutine(MainAttack(enemy));
                    }
                    else if (Hero.abilityType == 1)
                    {
                        heroSelected.GetComponent<Animator>().SetTrigger("Ability1");
                        StartCoroutine(Ability1(enemy));
                    }
                    else if (Hero.abilityType == 2)
                    {
                        heroSelected.GetComponent<Animator>().SetTrigger("Ability2");
                        StartCoroutine(Ability2(enemy));
                    }
                }
            }
        }
    }

    IEnumerator MainAttack(GameObject enemy)
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < heroSelected.GetComponent<Hero>().mainAttackEffects.Length; i++)
        {
            switch (heroSelected.GetComponent<Hero>().mainAttackEffects[i])
            {
                case "Poison":
                    int p = Random.Range(1, 5);
                    if (enemy.GetComponent<Hero>().status == "" && p == 1)
                    {
                        enemy.GetComponent<Hero>().status = "Poisoned";
                        enemy.GetComponent<Hero>().statusDuration = 2;
                    }
                    break;
                case "Entangle":
                    int a = Random.Range(1, 6);
                    if (enemy.GetComponent<Hero>().status == "" && a == 1)
                    {
                        enemy.GetComponent<Hero>().status = "Entangled";
                        enemy.GetComponent<Hero>().statusDuration = 1;
                    }
                    break;
                case "Direct":
                    enemy.GetComponent<Animator>().SetTrigger("Hit");
                    yield return new WaitForSeconds(0.5f);
                    enemy.GetComponent<Hero>().health -= heroSelected.GetComponent<Hero>().mainAttackDmg;
                    break;
            }
        }
        heroSelected = null;
        heroIsSelected = false;
    }

    IEnumerator Ability1(GameObject enemy)
    {
        yield return new WaitForSeconds(2.0f);
        
        heroSelected = null;
        heroIsSelected = false;
    }

    IEnumerator Ability2(GameObject enemy)
    {
        yield return new WaitForSeconds(2.0f);

        heroSelected = null;
        heroIsSelected = false;
    }

    //Moves hero over time
    public IEnumerator MoveToPosition(Vector3 target, float timeToMove)
    {
        //Erasmo's grass needs to disappear below
        if (heroSelected.name == "Erasmo(Clone)") 
                  target = new Vector3(target.x, target.y - 0.05f, target.z);
        //Turns off all the cell lights
        heroSelected.GetComponent<MoveHero>().TurnOffLights();
        var currentPos = heroSelected.transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            heroSelected.transform.position = Vector3.Lerp(currentPos, target, t);
            yield return null;
        }
        heroSelected.GetComponent<Animator>().SetBool("Walking", false);
        //Changes the tag of the cell the hero is currently on
        if (SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag == "OccupiedCell")
        {
            SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag = "Cell";
        }
        //Changes the coordinates in MoveHero
        heroSelected.GetComponent<MoveHero>().SetCoordinates(x, z);
        heroSelected = null;
    }

    void MoveSelectedHero() //Move hero
    {
        if (tag != "FirstRowCell")
        {
            ChangeRotationOfHero();
            //Play animation for walking
            heroSelected.GetComponent<Animator>().SetBool("Walking", true);
            //Animation speed set, if Tommy then all animations are 1s, the rest is dependable on distance
            if (heroSelected.name == "TommyApe(Clone)" || (Mathf.Abs(x - heroSelected.GetComponent<MoveHero>().GetX()) + Mathf.Abs(z - heroSelected.GetComponent<MoveHero>().GetZ())) == 1 && heroSelected.GetComponent<MoveHero>().movement != 1)
                StartCoroutine(MoveToPosition(transform.position, 1.0f));
            else if (Mathf.Abs(x - heroSelected.GetComponent<MoveHero>().GetX()) == 1 && Mathf.Abs(z - heroSelected.GetComponent<MoveHero>().GetZ()) == 1)
                StartCoroutine(MoveToPosition(transform.position, 1.2f));
            else
                StartCoroutine(MoveToPosition(transform.position, 2.0f));
        }
        else //If the game has not begun there is no need for animation
            StartCoroutine(MoveToPosition(transform.position, 0.0f));
    }

    //Change heros rotation so he is facing the cell he needs to move to
    void ChangeRotationOfHero()
    {
        if(x < heroSelected.GetComponent<MoveHero>().GetX())
        {
            if (z < heroSelected.GetComponent<MoveHero>().GetZ())
            {
                heroSelected.transform.rotation = Quaternion.Euler(0.0f, -135.0f, 0.0f);
            }
            else if (z > heroSelected.GetComponent<MoveHero>().GetZ())
            {
                heroSelected.transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
            }
            else
            {
                heroSelected.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            }
        }
        else if(x > heroSelected.GetComponent<MoveHero>().GetX())
        {
            if (z < heroSelected.GetComponent<MoveHero>().GetZ())
            {
                heroSelected.transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f);
            }
            else if (z > heroSelected.GetComponent<MoveHero>().GetZ())
            {
                heroSelected.transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
            }
            else
            {
                heroSelected.transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }
        }
        else
        {
            if (z < heroSelected.GetComponent<MoveHero>().GetZ())
            {
                heroSelected.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else if (z > heroSelected.GetComponent<MoveHero>().GetZ())
            {
                heroSelected.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
        }
    }

    public void SetPosition(int xx, int zz) //Set coordinates
    {
        x = xx;
        z = zz;
    }
    
    public void ChangeDistance(int d) //Set distance
    {
        distance = d;
    }
}
