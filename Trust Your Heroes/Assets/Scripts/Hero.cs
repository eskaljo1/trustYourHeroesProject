using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script attached to every hero, contains information about hero

public class Hero : MonoBehaviour
{
    public int health = 100;
    //Dmg
    public int mainAttackDmg = 20;
    public int ability1Dmg = 20;
    public int ability2Dmg = 20;
    //Range
    public int mainAttackRange = 1;
    public int ability1Range = 2;
    public int ability2Range = 2;
    //Cooldowns
    public int ability1Cooldown = 0;
    public int ability2Cooldown = 0;
    public int ability1C = 0;
    public int ability2C = 0;
    //Effects
    public string[] mainAttackEffects;
    public string[] ability1Effects;
    public string[] ability2Effects;
    //True if ability2 is passive
    public bool ability2Passive = false;
    //Statuses
    public bool evasiveness = false;
    public int evasivenessDuration = 0;
    public bool stun = false;
    public int stunDuration = 0;
    public bool poisoned = false;
    public int poisonDuration = 0;
    public bool slow = false;
    public int slowDuration = 0;
    public int shield = 0;
    public int shieldDuration = 0;
    public int buff = 0;
    public int buffDuration = 0;
    public int debuff = 0;
    public int debuffDuration = 0;
    public int movement = 0;
    public int movementDuration = 0;

    //Targets enemy or cell
    public bool isTargetingAbility1 = true;
    public bool isTargetingAbility2 = true;

    //Which ability is being used
    public static int abilityType = -1;

    //Name of ability
    public string mainAttackName = "";
    public string ability1Name = "";
    public string ability2Name = "";
    //Description
    public string heroName = "";
    public string description = "";
    public string firstAbility = "";
    public string secondAbility = "";

    private Slider healthBar = null;
    public AudioSource mainAttackAudio;
    public AudioSource ability1Audio;
    public AudioSource ability2Audio;
    public AudioSource deathAudio;

    public ParticleSystem mainAttackParticles;
    public ParticleSystem ability1Particles;
    public ParticleSystem ability2Particles;

    public ParticleSystem entangledParticles;
    public ParticleSystem slownessParticles;
    public ParticleSystem debuffParticles;
    public ParticleSystem poisonParticles;
    
    public Text ability1CooldownCounter;
    public Text ability2CooldownCounter;
    //True if hero isn't spawned in heroes panel or play panel
    private bool game = false;

    public PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        abilityType = -1;

        ability1C = ability1Cooldown;
        ability2C = ability2Cooldown;

        mainAttackParticles = null;
        ability1Particles = null;
        ability2Particles = null;
        ParticleSystem[] p = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem i in p)
        {
            if (i.gameObject.name == "MainAttackParticles")
                mainAttackParticles = i;
            else if (i.gameObject.name == "Ability1Particles")
                ability1Particles = i;
            else if (i.gameObject.name == "Ability2Particles")
                ability2Particles = i;
            else if (i.gameObject.name == "EntangledParticles")
                entangledParticles = i;
            else if (i.gameObject.name == "DebuffParticles")
                debuffParticles = i;
            else if (i.gameObject.name == "SlownessParticles")
                slownessParticles = i;
            else if (i.gameObject.name == "PoisonParticles")
                poisonParticles = i;
        }
    }
    
    public void FindHealthBar()
    {
        //Find health bar
        if (YourHeroTeam.heroNames[0] + "(Clone)" == name)
        {
            if (GameObject.Find("HealthBar1"))
                healthBar = GameObject.Find("HealthBar1").GetComponent<Slider>();
        }
        else if (YourHeroTeam.heroNames[1] + "(Clone)" == name)
        {
            if (GameObject.Find("HealthBar2"))
                healthBar = GameObject.Find("HealthBar2").GetComponent<Slider>();
        }
        else if (YourHeroTeam.heroNames[2] + "(Clone)" == name)
        {
            if (GameObject.Find("HealthBar3"))
                healthBar = GameObject.Find("HealthBar3").GetComponent<Slider>();
        }
        else if (YourHeroTeam.heroNames[3] + "(Clone)" == name)
        {
            if (GameObject.Find("HealthBar4"))
                healthBar = GameObject.Find("HealthBar4").GetComponent<Slider>();
        }
        if (healthBar != null)
        {
            healthBar.maxValue = health;
            game = true;
        }
    }

    [PunRPC]
    void DeathRPC()
    {
        SpawnGrid.cells[GetComponent<MoveHero>().GetX(), GetComponent<MoveHero>().GetZ()].tag = "Cell";
        GetComponent<Animator>().SetBool("Death", true);
        deathAudio.Play();
    }

    void Update()
    {
        //Death
        if(health <= 0)
        {
            photonView.RPC("DeathRPC", RpcTarget.All);
            if (photonView.IsMine) StartCoroutine(Death());
        }
    }

    //Destroy hero after animation ends
    IEnumerator Death()
    {
        yield return new WaitForSeconds(2.0f);
        if (NetworkManager.firstPlayer)
            PlaceHeroButtons.player1DeadHeroes++;
        else
            PlaceHeroButtons.player2DeadHeroes++;
        PhotonNetwork.Destroy(gameObject);
    }

    //Update healthbar
    void FixedUpdate()
    {
        if (game && health != healthBar.value)
        {
            if (health > healthBar.maxValue) health = (int) healthBar.maxValue;
            healthBar.value = health;
        }
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
        if (!stun && PlaceHero.gameBegun && !MoveHero.performing)
        {
            if ((MoveHero.player1Move && NetworkManager.firstPlayer) || (!MoveHero.player1Move && !NetworkManager.firstPlayer))
            {
                ReadyForAttack(0);
                FindEnemies(mainAttackRange, true, false);
            }
        }
    }

    //Ability1
    public void Ability1()
    {
        if (!stun && PlaceHero.gameBegun && !MoveHero.performing && ability1C == ability1Cooldown)
        {
            if ((MoveHero.player1Move && NetworkManager.firstPlayer) || (!MoveHero.player1Move && !NetworkManager.firstPlayer))
            {
                ReadyForAttack(1);
                if (ability1Range > 0)
                {
                    for (int i = 0; i < ability1Effects.Length; i++)
                        if (ability1Effects[i] == "Heal" || ability1Effects[i] == "Totem")
                        {
                            FindEnemies(ability1Range, isTargetingAbility1, true);
                            return;
                        }
                    FindEnemies(ability1Range, isTargetingAbility1, false);
                }
                else
                    ZeroRangeAbility(true);
            }
        }
    }

    //Ability2
    public void Ability2()
    {
        if (!ability2Passive && !stun && PlaceHero.gameBegun && !MoveHero.performing && ability2C == ability2Cooldown)
        {
            if ((MoveHero.player1Move && NetworkManager.firstPlayer) || (!MoveHero.player1Move && !NetworkManager.firstPlayer))
            {
                ReadyForAttack(2);
                if (ability2Range > 0)
                {
                    for (int i = 0; i < ability2Effects.Length; i++)
                        if (ability2Effects[i] == "Heal" || ability2Effects[i] == "Totem")
                        {
                            FindEnemies(ability2Range, isTargetingAbility2, true);
                            return;
                        }
                    FindEnemies(ability2Range, isTargetingAbility2, false);
                }
                else
                    ZeroRangeAbility(true);
            }
        }
    }

    //Find enemies and light up available cells
    void FindEnemies(int range, bool isTargeting, bool isHeal)
    {
        for (int i = 0; i < SpawnGrid.cells.GetLength(0); i++)
        {
            for (int j = 0; j < SpawnGrid.cells.GetLength(1); j++)
            {
                if (isTargeting)
                {
                    if (NetworkManager.firstPlayer)
                    {
                        if (isHeal)
                        {
                            if (SpawnGrid.cells[i, j].tag == "OccupiedCell" && (Mathf.Abs(GetComponent<MoveHero>().GetX() - i) + Mathf.Abs(GetComponent<MoveHero>().GetZ() - j)) <= range)
                            {
                                if (i == GetComponent<MoveHero>().GetX() && j == GetComponent<MoveHero>().GetZ())
                                { }
                                else
                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                            }
                        }
                        else
                        {
                            if (SpawnGrid.cells[i, j].tag == "EnemyCell" && (Mathf.Abs(GetComponent<MoveHero>().GetX() - i) + Mathf.Abs(GetComponent<MoveHero>().GetZ() - j)) <= range)
                            {
                                SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                            }
                        }
                    }
                    else
                    {
                        if (isHeal)
                        {
                            if (SpawnGrid.cells[i, j].tag == "EnemyCell" && (Mathf.Abs(GetComponent<MoveHero>().GetX() - i) + Mathf.Abs(GetComponent<MoveHero>().GetZ() - j)) <= range)
                            {
                                if (i == GetComponent<MoveHero>().GetX() && j == GetComponent<MoveHero>().GetZ())
                                { }
                                else
                                    SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                            }
                        }
                        else
                        {
                            if (SpawnGrid.cells[i, j].tag == "OccupiedCell" && (Mathf.Abs(GetComponent<MoveHero>().GetX() - i) + Mathf.Abs(GetComponent<MoveHero>().GetZ() - j)) <= range)
                            {
                                SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                            }
                        }
                    }
                }
                else
                {
                    if (isHeal)
                    {
                        if ((Mathf.Abs(GetComponent<MoveHero>().GetX() - i) + Mathf.Abs(GetComponent<MoveHero>().GetZ() - j)) <= range)
                        {
                            if (SpawnGrid.cells[i, j].tag != "Cell")
                            { }
                            else if (i == GetComponent<MoveHero>().GetX() && j == GetComponent<MoveHero>().GetZ())
                            { }
                            else
                                SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                        }
                    }
                    else
                    {
                        if ((Mathf.Abs(GetComponent<MoveHero>().GetX() - i) + Mathf.Abs(GetComponent<MoveHero>().GetZ() - j)) <= range)
                        {
                            if (abilityType == 1 && ability1Name == "Purgatory" && i == GetComponent<MoveHero>().GetX() && j == GetComponent<MoveHero>().GetZ())
                            { }
                            else if (i == GetComponent<MoveHero>().GetX() && j == GetComponent<MoveHero>().GetZ())
                            { }
                            else
                                SpawnGrid.cells[i, j].GetComponentInChildren<Light>().intensity = 15;
                        }
                    }
                }
            }
        }
    }

    IEnumerator StartAbility1()
    {
        yield return new WaitForSeconds(1.0f);
        ability1Audio.Play();
        if (ability1Particles != null)
            ability1Particles.Play();
        yield return new WaitForSeconds(0.5f);
        if (ability1Particles != null && gameObject.name != "TommyApe(Clone)" && gameObject.name != "Ohm(Clone)")
                ability1Particles.Stop();
        for (int i = 0; i < ability1Effects.Length; i++)
        {
            switch (ability1Effects[i])
            {
                case "Area":
                    GameObject[] enemies;
                    if (MoveHero.player1Move)
                        enemies = GameObject.FindGameObjectsWithTag("Player2");
                    else
                        enemies = GameObject.FindGameObjectsWithTag("Player");
                    for (int j = 0; j < enemies.Length; j++)
                        if ((Mathf.Abs(enemies[j].GetComponent<MoveHero>().GetX() - GetComponent<MoveHero>().GetX()) + Mathf.Abs(enemies[j].GetComponent<MoveHero>().GetZ() - GetComponent<MoveHero>().GetZ())) == 1)
                        {
                            bool evade = false;
                            if (enemies[j].GetComponent<Hero>().evasiveness)
                            {
                                int e = Random.Range(0, 2);
                                if (e == 1)
                                    evade = true;
                            }
                            if (!evade)
                            {
                                for (int k = 0; k < ability1Effects.Length; k++)
                                    if (ability1Effects[k] == "Entangle")
                                    {
                                        int a = Random.Range(1, 6);
                                        if (!enemies[j].GetComponent<Hero>().stun && a == 1)
                                        {
                                            enemies[j].GetComponent<Hero>().stun = true;
                                            enemies[j].GetComponent<Hero>().stunDuration = 1;
                                            enemies[j].GetComponent<Hero>().entangledParticles.Play();
                                        }
                                    }
                                enemies[j].GetComponent<Animator>().SetTrigger("Hit");
                                int dmg = (int)(ability1Dmg + (buff / 100.0 * ability1Dmg) - (debuff / 100.0 * ability1Dmg));
                                enemies[j].GetComponent<Hero>().health -= (int)(dmg - (enemies[j].GetComponent<Hero>().shield / 100.0 * dmg));
                            }
                        }
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "Evasiveness":
                    evasiveness = true;
                    evasivenessDuration = 2;
                    break;
                case "Taunt":

                    break;
            }
        }
        yield return new WaitForSeconds(2.0f);
        GetComponent<PhotonView>().RPC("ChangeTurn", RpcTarget.All);
        PlaceHero.heroIsSelected = false;
        PlaceHero.heroSelected = null;
    }

    IEnumerator StartAbility2()
    {
        if (gameObject.name != "Hor(Clone)")
            yield return new WaitForSeconds(1.0f);
        else
            yield return new WaitForSeconds(0.5f);
        ability2Audio.Play();
        if (ability2Particles != null)
            ability2Particles.Play();
        yield return new WaitForSeconds(0.5f);
        if (ability2Particles != null && gameObject.name != "Hor(Clone)" && gameObject.name != "Z(Clone)" && gameObject.name != "Ohm(Clone)" && gameObject.name != "Makas(Clone)" && gameObject.name != "Xavier(Clone)")
            ability2Particles.Stop();
        for (int i = 0; i < ability2Effects.Length; i++)
        {
            switch (ability2Effects[i])
            {
                case "Area":
                    GameObject[] enemies;
                    if (MoveHero.player1Move)
                        enemies = GameObject.FindGameObjectsWithTag("Player2");
                    else
                        enemies = GameObject.FindGameObjectsWithTag("Player");
                    for (int j = 0; j < enemies.Length; j++)
                        if ((Mathf.Abs(enemies[j].GetComponent<MoveHero>().GetX() - GetComponent<MoveHero>().GetX()) + Mathf.Abs(enemies[j].GetComponent<MoveHero>().GetZ() - GetComponent<MoveHero>().GetZ())) == 1)
                        {
                            bool evade = false;
                            if (enemies[j].GetComponent<Hero>().evasiveness)
                            {
                                int e = Random.Range(0, 2);
                                if (e == 1)
                                    evade = true;
                            }
                            if (!evade)
                            {
                                enemies[j].GetComponent<Animator>().SetTrigger("Hit");
                                int dmg = (int)(ability2Dmg + (buff / 100.0 * ability2Dmg) - (debuff / 100.0 * ability2Dmg));
                                enemies[j].GetComponent<Hero>().health -= (int)(dmg - (enemies[j].GetComponent<Hero>().shield / 100.0 * dmg));
                            }
                        }
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "Shield":
                    shield = 50;
                    shieldDuration = 2;
                    break;
                case "Buff":
                    buff = 100;
                    buffDuration = 3;
                    break;
                case "Trance":
                    movement = 1;
                    movementDuration = 2;
                    buff = 50;
                    buffDuration = 2;
                    break;
            }
        }
        PlaceHero.heroIsSelected = false;
        PlaceHero.heroSelected = null;
    }

    [PunRPC]
    void Ability1RPC()
    {
        ability1C = 0;
        GetComponent<Animator>().SetTrigger("Ability1");
        StartCoroutine(StartAbility1());
    }

    [PunRPC]
    void Ability2RPC()
    {
        ability2C = 0;
        GetComponent<Animator>().SetTrigger("Ability2");
        StartCoroutine(StartAbility2());
    }

    IEnumerator ChangeTurnAfterAbility()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<PhotonView>().RPC("ChangeTurn", RpcTarget.All);
    }

    //For zero range abilities
    void ZeroRangeAbility(bool isAbility1)
    {
        photonView.RPC("ChangePerforming", RpcTarget.All);
        if (abilityType == 1)
        {
            ability1C = 0;
            photonView.RPC("Ability1RPC", RpcTarget.All);
        }
        else if (abilityType == 2)
        {
            ability2C = 0;
            photonView.RPC("Ability2RPC", RpcTarget.All);
        }
        StartCoroutine(ChangeTurnAfterAbility());
    }

    //Reduce cooldowns by 1
    void Cooldowns()
    {
        //Abilities
        if (photonView.IsMine)
        {
            if (ability1C != ability1Cooldown)
            {
                ability1C++;
                ability1CooldownCounter.text = (ability1Cooldown - ability1C).ToString();
            }
            if (ability1C == ability1Cooldown)
            {
                ability1CooldownCounter.text = "";
            }
            if (!ability2Passive)
            {
                if (ability2C != ability2Cooldown)
                {
                    ability2C++;
                    ability2CooldownCounter.text = (ability2Cooldown - ability2C).ToString();
                }
                if (ability2C == ability2Cooldown)
                {
                    ability2CooldownCounter.text = "";
                }
            }
        }
        //Evasiveness
        if (evasivenessDuration != -1)
        {
            evasivenessDuration--;
            if (evasivenessDuration == -1 && gameObject.name != "Sent(Clone)")
            {
                evasiveness = false;
                if (gameObject.name == "TommyApe(Clone)")
                    ability1Particles.Stop();
            }
        }
        //Stun
        if (stunDuration != -1)
        {
            stunDuration--;
            if (stunDuration == -1)
            {
                stun = false;
                entangledParticles.Stop();
            }
        }
        //Poison
        if (poisonDuration != -1)
        {
            poisonDuration--;
            if (poisonDuration == -1)
            {
                poisoned = false;
                poisonParticles.Stop();
            }
            else
                health -= 10;
        }
        //Slowness
        if (slowDuration != -1)
        {
            slowDuration--;
            if (slowDuration == -1)
            {
                slow = false;
                slownessParticles.Stop();
            }
        }
        //Shield
        if (shieldDuration != -1)
        {
            shieldDuration--;
            if (shieldDuration == -1)
            {
                shield = 0;
                if (gameObject.name == "Ohm(Clone)" || gameObject.name == "Xavier(Clone)")
                    ability2Particles.Stop();
            }
        }
        //Buff
        if (buffDuration != -1)
        {
            buffDuration--;
            if (buffDuration == -1)
            {
                buff = 0;
                if (gameObject.name == "Hor(Clone)" || gameObject.name == "Z(Clone)")
                    ability2Particles.Stop();
            }
        }
        //Debuff
        if (debuffDuration != -1)
        {
            debuffDuration--;
            if (debuffDuration == -1)
            {
                debuff = 0;
                debuffParticles.Stop();
            }
        }
        //Movement
        if (movementDuration != -1)
        {
            movementDuration--;
            if (movementDuration == -1)
                movement = 0;
        }
}

    [PunRPC]
    void ChangeTurn()
    {
        MoveHero.player1Move = !MoveHero.player1Move;
        MoveHero.performing = !MoveHero.performing;
        if (MoveHero.player1Move)
        {
            if (NetworkManager.firstPlayer)
            {
                GameObject[] allies = GameObject.FindGameObjectsWithTag("Player");
                for(int i = 0; i < allies.Length; i++)
                    allies[i].GetComponent<Hero>().Cooldowns();
                SwitchCamera.moveText.GetComponent<Text>().text = "Your move";
            }
            else
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < enemies.Length; i++)
                    enemies[i].GetComponent<Hero>().Cooldowns();
                SwitchCamera.moveText.GetComponent<Text>().text = "Opponent's move";
            }
        }
        else
        {
            if (!NetworkManager.firstPlayer)
            {
                GameObject[] allies = GameObject.FindGameObjectsWithTag("Player2");
                for (int i = 0; i < allies.Length; i++)
                    allies[i].GetComponent<Hero>().Cooldowns();
                SwitchCamera.moveText.GetComponent<Text>().text = "Your move";
            }
            else
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player2");
                for (int i = 0; i < enemies.Length; i++)
                    enemies[i].GetComponent<Hero>().Cooldowns();
                SwitchCamera.moveText.GetComponent<Text>().text = "Opponent's move";
            }
        }
    }
}
