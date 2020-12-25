using Photon.Pun;
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
        movement = true;
        heroSelected = null;
        gameBegun = false;
        heroIsSelected = false;
    }

    void Update()
    {
        if (gameBegun) //When game starts there is no need for FirstRowCell tag
        {
            PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
            if (tag == "FirstRowCell" || tag == "LastRowCell")
                photonView.RPC("SetCellTag", RpcTarget.All, "Cell");
        }
    }

    [PunRPC]
    void HeroIsPlacedOnCell(int id)
    {
        PhotonView p = PhotonView.Find(id);
        if (p.gameObject.tag == "Player")
            tag = "OccupiedCell";
        else if(p.gameObject.tag == "Player2")
            tag = "EnemyCell";
    }

    [PunRPC]
    void HeroIsMovedFromCell(int t, string s)
    {
        PhotonView p = PhotonView.Find(t);
        p.gameObject.tag = s;
    }

    [PunRPC]
    void SetCellTag(string t)
    {
        tag = t;
    }

    [PunRPC]
    void SetHeroTag(int t)
    {
        PhotonView p = PhotonView.Find(t);
        p.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        p.gameObject.tag = "Player2";
    }

    [PunRPC]
    void SetHeroCoords(int t)
    {
        PhotonView p = PhotonView.Find(t);
        p.gameObject.GetComponent<MoveHero>().SetCoordinates(x, z);
    }

    [PunRPC]
    void SetHeroWalking(int t, bool b)
    {
        PhotonView p = PhotonView.Find(t);
        p.gameObject.GetComponent<Animator>().SetBool("Walking", b);
    }

    void OnMouseDown()
    {
        if (heroIsSelected)
        {
            int id;
            if (movement) //if movement, else attack
            {
                if ((tag == "FirstRowCell" && NetworkManager.firstPlayer) || (tag == "LastRowCell" && !NetworkManager.firstPlayer)) //If the selected cell has FirstRowCell tag the game hasn't started yet, spawns hero on cell
                {
                    heroIsSelected = false;
                    if (heroSelected) //If a hero is selected on the board, move him from his current cell
                    {
                        //Change tag from OccupiedCell to FirstRowCell
                        if (NetworkManager.firstPlayer)
                        {
                            PhotonView p = PhotonView.Get(GetComponent<PhotonView>());
                            p.RPC("HeroIsMovedFromCell", RpcTarget.All, SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].GetComponent<PhotonView>().ViewID, "FirstRowCell");
                        }
                        else
                        {
                            PhotonView p = PhotonView.Get(GetComponent<PhotonView>());
                            p.RPC("HeroIsMovedFromCell", RpcTarget.All, SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].GetComponent<PhotonView>().ViewID, "LastRowCell");
                        }
                        id = heroSelected.GetComponent<PhotonView>().ViewID;
                        //Move
                        MoveSelectedHero();
                    }
                    else //else spawn hero of the selected hero icon
                    {
                        PhotonView p = PhotonView.Get(GetComponent<PhotonView>());
                        if (NetworkManager.firstPlayer)
                            heroSelected = PhotonNetwork.Instantiate("Models/Heroes/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1] + "/source/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1], transform.position, Quaternion.identity);
                        else
                        {
                            heroSelected = PhotonNetwork.Instantiate("Models/Heroes/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1] + "/source/" + YourHeroTeam.heroNames[PlaceHeroButtons.heroSelected - 1], transform.position, Quaternion.identity);
                            p.RPC("SetHeroTag", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID);
                        }
                        heroSelected.GetComponent<Hero>().FindHealthBar();
                        p.RPC("SetHeroCoords", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID);
                        PlaceHeroButtons.spawned = true;
                        PlaceHeroButtons.heroesPlaced++;
                        id = heroSelected.GetComponent<PhotonView>().ViewID;
                    }
                    //Turn off lights
                    if (NetworkManager.firstPlayer)
                    {
                        GameObject[] firstRowCells = GameObject.FindGameObjectsWithTag("FirstRowCell");
                        for (int i = 0; i < firstRowCells.Length; i++)
                        {
                            firstRowCells[i].GetComponentInChildren<Light>().intensity = 0;
                        }
                    }
                    else
                    {
                        GameObject[] lastRowCells = GameObject.FindGameObjectsWithTag("LastRowCell");
                        for (int i = 0; i < lastRowCells.Length; i++)
                        {
                            lastRowCells[i].GetComponentInChildren<Light>().intensity = 0;
                        }
                    }
                    PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
                    photonView.RPC("HeroIsPlacedOnCell", RpcTarget.All, id);
                }
                if (gameBegun) //If game has begun, move hero to cell if it is available
                {
                    if (GetComponentInChildren<Light>().intensity == 15) //Checks if cell is available
                    {
                        id = heroSelected.GetComponent<PhotonView>().ViewID;
                        MoveSelectedHero();
                        PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
                        photonView.RPC("HeroIsPlacedOnCell", RpcTarget.All, id);
                    }
                }
            }
            else //for attacks
            {
                if (GetComponentInChildren<Light>().intensity == 15)
                {
                    heroSelected.GetComponent<MoveHero>().TurnOffLights();
                    ChangeRotationOfHero();
                    GameObject[] enemies;
                    if (MoveHero.player1Move)
                        enemies = GameObject.FindGameObjectsWithTag("Player2");
                    else
                        enemies = GameObject.FindGameObjectsWithTag("Player");
                    GameObject enemy = null;
                    for (int i = 0; i < enemies.Length; i++)
                        if (enemies[i].GetComponent<MoveHero>().GetX() == x && enemies[i].GetComponent<MoveHero>().GetZ() == z)
                        {
                            enemy = enemies[i];
                            break;
                        }
                    if(enemy == null)
                    {
                        if (MoveHero.player1Move)
                            enemies = GameObject.FindGameObjectsWithTag("Player");
                        else
                            enemies = GameObject.FindGameObjectsWithTag("Player2");
                        for (int i = 0; i < enemies.Length; i++)
                            if (enemies[i].GetComponent<MoveHero>().GetX() == x && enemies[i].GetComponent<MoveHero>().GetZ() == z)
                            {
                                enemy = enemies[i];
                                break;
                            }
                    }
                    bool evade = false;
                    if (enemy != null)
                        if (enemy.GetComponent<Hero>().evasiveness)
                        {
                            int e = Random.Range(0, 2);
                            if (e == 1)
                                evade = true;
                        }
                    int p = Random.Range(1, 5);
                    int a = Random.Range(1, 6);
                    PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
                    if (Hero.abilityType == 0)
                    {
                        photonView.RPC("MainAttackRPC", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID, enemy.GetComponent<PhotonView>().ViewID, evade, a, p);
                    }
                    else if (Hero.abilityType == 1)
                    {
                        heroSelected.GetComponent<Hero>().ability1C = 0;
                        if (enemy != null)
                            photonView.RPC("Ability1RPC", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID, enemy.GetComponent<PhotonView>().ViewID, evade, p);
                        else
                            photonView.RPC("Ability1RPC", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID, -13, evade, p);
                    }
                    else if (Hero.abilityType == 2)
                    {
                        heroSelected.GetComponent<Hero>().ability2C = 0;
                        if (enemy != null)
                            photonView.RPC("Ability2RPC", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID, enemy.GetComponent<PhotonView>().ViewID, evade);
                        else
                            photonView.RPC("Ability2RPC", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID, -13, evade);
                    }
                    StartCoroutine(ChangeTurnAfterAbility());
                }
            }
        }
    }

    IEnumerator ChangeTurnAfterAbility()
    {
        yield return new WaitForSeconds(4.0f);
        heroSelected.GetComponent<PhotonView>().RPC("ChangePerforming", RpcTarget.All);
        heroSelected.GetComponent<PhotonView>().RPC("ChangeTurn", RpcTarget.All);
    }

    [PunRPC]
    void MainAttackRPC(int heroId, int enemyId, bool evade, int a, int p)
    {
        PhotonView hero = PhotonView.Find(heroId);
        PhotonView enemy = PhotonView.Find(enemyId);
        heroSelected = hero.gameObject;
        heroSelected.GetComponent<Animator>().SetTrigger("MainAttack");
        StartCoroutine(MainAttack(enemy.gameObject, evade, a, p));
    }

    [PunRPC]
    void Ability1RPC(int heroId, int enemyId, bool evade, int p)
    {
        PhotonView hero = PhotonView.Find(heroId);
        heroSelected = hero.gameObject;
        heroSelected.GetComponent<Animator>().SetTrigger("Ability1");
        if (enemyId != -13)
        {
            PhotonView enemy = PhotonView.Find(enemyId);
            StartCoroutine(Ability1(enemy.gameObject, evade, p));
        }
        else
        {
            StartCoroutine(Ability1(null, evade, p));
        }
    }

    [PunRPC]
    void Ability2RPC(int heroId, int enemyId, bool evade)
    {
        PhotonView hero = PhotonView.Find(heroId);
        heroSelected = hero.gameObject;
        heroSelected.GetComponent<Animator>().SetTrigger("Ability2");
        if (enemyId != -13)
        {
            PhotonView enemy = PhotonView.Find(enemyId);
            StartCoroutine(Ability2(enemy.gameObject, evade));
        }
        else
        {
            StartCoroutine(Ability2(null, evade));
        }
    }

    IEnumerator MainAttack(GameObject enemy, bool evade, int a, int p)
    {
        if (heroSelected.name == "TommyApe(Clone)")
        {
            heroSelected.GetComponent<Hero>().mainAttackAudio.Play();
            heroSelected.GetComponent<Hero>().mainAttackParticles.Play();
            yield return new WaitForSeconds(1.0f);
            heroSelected.GetComponent<Hero>().mainAttackParticles.Stop();
        }
        else if (heroSelected.name == "Creek(Clone)")
        {
            yield return new WaitForSeconds(0.5f);
            heroSelected.GetComponent<Hero>().mainAttackAudio.Play();
            heroSelected.GetComponent<Hero>().mainAttackParticles.Play();
            yield return new WaitForSeconds(1.0f);
            heroSelected.GetComponent<Hero>().mainAttackParticles.Stop();
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
            heroSelected.GetComponent<Hero>().mainAttackAudio.Play();
            if (heroSelected.GetComponent<Hero>().mainAttackParticles != null)
                heroSelected.GetComponent<Hero>().mainAttackParticles.Play();
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < heroSelected.GetComponent<Hero>().mainAttackEffects.Length; i++)
        {
            switch (heroSelected.GetComponent<Hero>().mainAttackEffects[i])
            {
                case "Poison":
                    if (!evade)
                    {
                        if (!enemy.GetComponent<Hero>().poisoned && p == 1)
                        {
                            enemy.GetComponent<Hero>().poisoned = true;
                            enemy.GetComponent<Hero>().poisonDuration = 2;
                            enemy.GetComponent<Hero>().poisonParticles.Play();
                        }
                    }
                    break;
                case "Entangle":
                    if (!evade)
                    {
                        if (!enemy.GetComponent<Hero>().stun && a == 1)
                        {
                            enemy.GetComponent<Hero>().stun = true;
                            enemy.GetComponent<Hero>().stunDuration = 1;
                            enemy.GetComponent<Hero>().entangledParticles.Play();
                        }
                    }
                    break;
                case "Direct":
                    if (!evade)
                    {
                        enemy.GetComponent<Animator>().SetTrigger("Hit");
                        yield return new WaitForSeconds(0.5f);
                        int dmg = (int)(heroSelected.GetComponent<Hero>().mainAttackDmg + (heroSelected.GetComponent<Hero>().buff / 100.0 * heroSelected.GetComponent<Hero>().mainAttackDmg) - (heroSelected.GetComponent<Hero>().debuff / 100.0 * heroSelected.GetComponent<Hero>().mainAttackDmg));
                        enemy.GetComponent<Hero>().health -= (int)(dmg - (enemy.GetComponent<Hero>().shield / 100.0 * dmg));
                    }
                    break;
            }
        }
        heroIsSelected = false;
    }

    IEnumerator Ability1(GameObject enemy, bool evade, int p)
    {
        bool skipstun = false;
        if (heroSelected.GetComponent<Hero>().ability1Name == "Gas bomb")
            yield return new WaitForSeconds(1.5f);
        else if(heroSelected.GetComponent<Hero>().ability1Name == "Frostbolt" || heroSelected.GetComponent<Hero>().ability1Name == "Nature's gift")
        { }
        else
            yield return new WaitForSeconds(0.5f);
        heroSelected.GetComponent<Hero>().ability1Audio.Play();
        if (heroSelected.GetComponent<Hero>().ability1Particles != null)
            heroSelected.GetComponent<Hero>().ability1Particles.Play();
        else
        {
            for (int i = 0; i < heroSelected.GetComponent<Hero>().ability1Effects.Length; i++)
                switch (heroSelected.GetComponent<Hero>().ability1Effects[i])
                {
                    case "Poison":
                        GameObject g = GameObject.Instantiate(Resources.Load<GameObject>("Models/AbilityEffects/GasBombPrefab"), new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                        g.GetComponent<ParticleSystem>().Play();
                        yield return new WaitForSeconds(1.5f);
                        Destroy(g);
                        break;
                }
        }
        if (heroSelected.GetComponent<Hero>().ability1Name != "Gas bomb")
            yield return new WaitForSeconds(1.5f);
        if (heroSelected.GetComponent<Hero>().ability1Particles != null)
            heroSelected.GetComponent<Hero>().ability1Particles.Stop();
        for (int i = 0; i < heroSelected.GetComponent<Hero>().ability1Effects.Length; i++)
        {
            switch (heroSelected.GetComponent<Hero>().ability1Effects[i])
            {
                case "Area":
                    GameObject[] enemies;
                    if (MoveHero.player1Move)
                        enemies = GameObject.FindGameObjectsWithTag("Player2");
                    else
                        enemies = GameObject.FindGameObjectsWithTag("Player");
                    for (int j = 0; j < enemies.Length; j++)
                        if ((Mathf.Abs(enemies[j].GetComponent<MoveHero>().GetX() - x) + Mathf.Abs(enemies[j].GetComponent<MoveHero>().GetZ() - z)) <= 1)
                        {
                            if (heroSelected.GetComponent<MoveHero>().GetX() == enemies[j].GetComponent<MoveHero>().GetX() && heroSelected.GetComponent<MoveHero>().GetZ() == enemies[j].GetComponent<MoveHero>().GetZ())
                                continue;
                            if (!enemies[j].GetComponent<Hero>().evasiveness)
                            {
                                for (int k = 0; k < heroSelected.GetComponent<Hero>().ability1Effects.Length; k++)
                                    if (heroSelected.GetComponent<Hero>().ability1Effects[k] == "Stun")
                                    {
                                        if (!enemies[j].GetComponent<Hero>().stun)
                                        {
                                            enemies[j].GetComponent<Hero>().stun = true;
                                            enemies[j].GetComponent<Hero>().stunDuration = 1;
                                            enemies[j].GetComponent<Hero>().entangledParticles.Play();
                                        }
                                    }
                                enemies[j].GetComponent<Animator>().SetTrigger("Hit");
                                int d = (int)(heroSelected.GetComponent<Hero>().ability1Dmg + (heroSelected.GetComponent<Hero>().buff / 100.0 * heroSelected.GetComponent<Hero>().ability1Dmg) - (heroSelected.GetComponent<Hero>().debuff / 100.0 * heroSelected.GetComponent<Hero>().ability1Dmg));
                                enemies[j].GetComponent<Hero>().health -= (int)(d - (enemies[j].GetComponent<Hero>().shield / 100.0 * d));
                                for (int k = 0; k < heroSelected.GetComponent<Hero>().ability1Effects.Length; k++)
                                    if (heroSelected.GetComponent<Hero>().ability1Effects[k] == "Poison")
                                    {
                                        if (!enemies[j].GetComponent<Hero>().poisoned && p == 1)
                                        {
                                            enemies[j].GetComponent<Hero>().poisoned = true;
                                            enemies[j].GetComponent<Hero>().poisonDuration = 2;
                                            enemies[j].GetComponent<Hero>().poisonParticles.Play();
                                        }
                                    }
                            }
                            else
                                skipstun = true;
                        }
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "Totem":
                    if (NetworkManager.firstPlayer && MoveHero.player1Move)
                    {
                        GameObject totem = PhotonNetwork.Instantiate("Models/Heroes/Nazz/Totems/HealTotem/source/HealTotem", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                        totem.GetComponent<PhotonView>().RPC("SetTag", RpcTarget.All, true);
                    }
                    else if(!NetworkManager.firstPlayer && !MoveHero.player1Move)
                    {
                        GameObject totem = PhotonNetwork.Instantiate("Models/Heroes/Nazz/Totems/HealTotem/source/HealTotem", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                        totem.GetComponent<PhotonView>().RPC("SetTag", RpcTarget.All, false);
                    }
                    break;
                case "Heal":
                    yield return new WaitForSeconds(0.5f);
                    enemy.GetComponent<Hero>().health += heroSelected.GetComponent<Hero>().ability1Dmg;
                    break;
                case "Slow":
                    if (!enemy.GetComponent<Hero>().slow && !evade)
                    {
                        enemy.GetComponent<Hero>().slow = true;
                        enemy.GetComponent<Hero>().slowDuration = 2;
                        enemy.GetComponent<Hero>().slownessParticles.Play();
                    }
                    break;
                case "Stun":
                    if (enemy != null)
                        if (!enemy.GetComponent<Hero>().stun && !evade && !skipstun)
                        {
                            enemy.GetComponent<Hero>().stun = true;
                            enemy.GetComponent<Hero>().stunDuration = 2;
                            enemy.GetComponent<Hero>().entangledParticles.Play();
                        }
                    break;
                case "Direct":
                    if (!evade)
                    {
                        if (enemy != null)
                        {
                            enemy.GetComponent<Animator>().SetTrigger("Hit");
                            for(int j = 0; j < heroSelected.GetComponent<Hero>().ability1Effects.Length; j++)
                                if(heroSelected.GetComponent<Hero>().ability1Effects[j] == "Slice")
                                {
                                    GameObject gg = GameObject.Instantiate(Resources.Load<GameObject>("Models/AbilityEffects/Purgatory"), new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1.0f, enemy.transform.position.z), Quaternion.identity);
                                    yield return new WaitForSeconds(1.0f);
                                    Destroy(gg);
                                }
                            yield return new WaitForSeconds(0.5f);
                            int dmg = (int)(heroSelected.GetComponent<Hero>().ability1Dmg + (heroSelected.GetComponent<Hero>().buff / 100.0 * heroSelected.GetComponent<Hero>().ability1Dmg) - (heroSelected.GetComponent<Hero>().debuff / 100.0 * heroSelected.GetComponent<Hero>().ability1Dmg));
                            enemy.GetComponent<Hero>().health -= (int)(dmg - (enemy.GetComponent<Hero>().shield / 100.0 * dmg));
                        }
                    }
                    break;
                case "Purgatory":
                    GameObject[] enemies2;
                    if (MoveHero.player1Move)
                        enemies2 = GameObject.FindGameObjectsWithTag("Player2");
                    else
                        enemies2 = GameObject.FindGameObjectsWithTag("Player");
                    GameObject[] g = new GameObject[2];
                    for (int j = 0; j < enemies2.Length; j++)
                        if ((Mathf.Abs(enemies2[j].GetComponent<MoveHero>().GetX() - x) + Mathf.Abs(enemies2[j].GetComponent<MoveHero>().GetZ() - z)) <= 1)
                        {
                            if (heroSelected.GetComponent<MoveHero>().GetX() == enemies2[j].GetComponent<MoveHero>().GetX() && heroSelected.GetComponent<MoveHero>().GetZ() == enemies2[j].GetComponent<MoveHero>().GetZ())
                                continue;
                            if ((heroSelected.GetComponent<MoveHero>().GetX() == x && heroSelected.GetComponent<MoveHero>().GetX() == enemies2[j].GetComponent<MoveHero>().GetX()) || (heroSelected.GetComponent<MoveHero>().GetZ() == z && heroSelected.GetComponent<MoveHero>().GetZ() == enemies2[j].GetComponent<MoveHero>().GetZ())) {
                                bool ev = false;
                                if (enemies2[j].GetComponent<Hero>().evasiveness)
                                {
                                    int e = Random.Range(0, 2);
                                    if (e == 1)
                                        ev = true;
                                }
                                if (!ev)
                                {
                                    enemies2[j].GetComponent<Animator>().SetTrigger("Hit");
                                    g[i] = GameObject.Instantiate(Resources.Load<GameObject>("Models/AbilityEffects/Purgatory"), new Vector3(enemies2[j].transform.position.x, enemies2[j].transform.position.y + 1.0f, enemies2[j].transform.position.z), Quaternion.identity);
                                    int d = (int)(heroSelected.GetComponent<Hero>().ability1Dmg + (heroSelected.GetComponent<Hero>().buff / 100.0 * heroSelected.GetComponent<Hero>().ability1Dmg) - (heroSelected.GetComponent<Hero>().debuff / 100.0 * heroSelected.GetComponent<Hero>().ability1Dmg));
                                    enemies2[j].GetComponent<Hero>().health -= (int)(d - (enemies2[j].GetComponent<Hero>().shield / 100.0 * d));
                                }
                            }
                        }
                    yield return new WaitForSeconds(1.0f);
                    if(g[0] != null)
                        Destroy(g[0]);
                    if (g[1] != null)
                        Destroy(g[1]);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "Earthquake":
                    GameObject earth = Instantiate(Resources.Load<GameObject>("Models/AbilityEffects/EarthquakePrefab"), new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
                    yield return new WaitForSeconds(1.5f);
                    Destroy(earth);
                    break;
                case "Flare":
                    GameObject flare = Instantiate(Resources.Load<GameObject>("Models/AbilityEffects/FlarePrefab"), new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity);
                    yield return new WaitForSeconds(1.5f);
                    Destroy(flare);
                    break;
            }
        }
        heroIsSelected = false;
    }

    IEnumerator Ability2(GameObject enemy, bool evade)
    {
        yield return new WaitForSeconds(0.5f);
        heroSelected.GetComponent<Hero>().ability2Audio.Play();
        if (heroSelected.GetComponent<Hero>().ability2Particles != null)
            heroSelected.GetComponent<Hero>().ability2Particles.Play();
        yield return new WaitForSeconds(1.5f);
        if (heroSelected.GetComponent<Hero>().ability2Particles != null)
            heroSelected.GetComponent<Hero>().ability2Particles.Stop();
        for (int i = 0; i < heroSelected.GetComponent<Hero>().ability2Effects.Length; i++)
        {
            switch (heroSelected.GetComponent<Hero>().ability2Effects[i])
            {
                case "Area":
                    GameObject[] enemies;
                    if (MoveHero.player1Move)
                        enemies = GameObject.FindGameObjectsWithTag("Player2");
                    else
                        enemies = GameObject.FindGameObjectsWithTag("Player");
                    for (int j = 0; j < enemies.Length; j++)
                        if ((Mathf.Abs(enemies[j].GetComponent<MoveHero>().GetX() - x) + Mathf.Abs(enemies[j].GetComponent<MoveHero>().GetZ() - z)) <= 1)
                        {
                            if (heroSelected.GetComponent<MoveHero>().GetX() == enemies[j].GetComponent<MoveHero>().GetX() && heroSelected.GetComponent<MoveHero>().GetZ() == enemies[j].GetComponent<MoveHero>().GetZ())
                                continue;
                            if (!enemies[j].GetComponent<Hero>().evasiveness)
                            {
                                for (int k = 0; k < heroSelected.GetComponent<Hero>().ability2Effects.Length; k++)
                                    if (heroSelected.GetComponent<Hero>().ability2Effects[k] == "Slow")
                                    {
                                        if (!enemies[j].GetComponent<Hero>().slow)
                                        {
                                            enemies[j].GetComponent<Hero>().slow = true;
                                            enemies[j].GetComponent<Hero>().slowDuration = 1;
                                            enemies[j].GetComponent<Hero>().slownessParticles.Play();
                                        }
                                    }
                                enemies[j].GetComponent<Animator>().SetTrigger("Hit");
                                int d = (int)(heroSelected.GetComponent<Hero>().ability2Dmg + (heroSelected.GetComponent<Hero>().buff / 100.0 * heroSelected.GetComponent<Hero>().ability2Dmg) - (heroSelected.GetComponent<Hero>().debuff / 100.0 * heroSelected.GetComponent<Hero>().ability2Dmg));
                                enemies[j].GetComponent<Hero>().health -= (int)(d - (enemies[j].GetComponent<Hero>().shield / 100.0 * d));
                            }
                        }
                    yield return new WaitForSeconds(0.5f);

                    break;
                case "Debuff":
                    if (!evade)
                    {
                        enemy.GetComponent<Hero>().debuff = 50;
                        enemy.GetComponent<Hero>().debuffDuration = 2;
                        enemy.GetComponent<Hero>().debuffParticles.Play();
                    }
                    break;
                case "Stun":
                    if (!enemy.GetComponent<Hero>().stun && !evade)
                    {
                        enemy.GetComponent<Hero>().stun = true;
                        enemy.GetComponent<Hero>().stunDuration = 2;
                        enemy.GetComponent<Hero>().entangledParticles.Play();
                    }
                    break;
                case "Direct":
                    if (!evade)
                    {
                        enemy.GetComponent<Animator>().SetTrigger("Hit");
                        yield return new WaitForSeconds(0.5f);
                        int dmg = (int)(heroSelected.GetComponent<Hero>().ability2Dmg + (heroSelected.GetComponent<Hero>().buff / 100.0 * heroSelected.GetComponent<Hero>().ability2Dmg) - (heroSelected.GetComponent<Hero>().debuff / 100.0 * heroSelected.GetComponent<Hero>().ability2Dmg));
                        enemy.GetComponent<Hero>().health -= (int)(dmg - (enemy.GetComponent<Hero>().shield / 100.0 * dmg));
                    }
                    break;
                case "Totem":
                    if (NetworkManager.firstPlayer && MoveHero.player1Move)
                    {
                        GameObject totem = PhotonNetwork.Instantiate("Models/Heroes/Nazz/Totems/TrapTotem/source/TrapTotem", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                        totem.GetComponent<PhotonView>().RPC("SetTag", RpcTarget.All, true);
                    }
                    else if (!NetworkManager.firstPlayer && !MoveHero.player1Move)
                    {
                        GameObject totem = PhotonNetwork.Instantiate("Models/Heroes/Nazz/Totems/TrapTotem/source/TrapTotem", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                        totem.GetComponent<PhotonView>().RPC("SetTag", RpcTarget.All, false);
                    }
                    break;
                case "Barrage":
                    break;
            }
        }
        heroIsSelected = false;
    }

    //Moves hero over time
    public IEnumerator MoveToPosition(Vector3 target, float timeToMove, bool changeTurn)
    {
        if(changeTurn)
            heroSelected.GetComponent<PhotonView>().RPC("ChangePerforming", RpcTarget.All);
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
        PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
        photonView.RPC("SetHeroWalking", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID, false);
        //Changes the tag of the cell the hero is currently on
        if (SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag == "OccupiedCell" || SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].tag == "EnemyCell")
        {
            PhotonView p = PhotonView.Get(GetComponent<PhotonView>());
            p.RPC("HeroIsMovedFromCell", RpcTarget.All, SpawnGrid.cells[heroSelected.GetComponent<MoveHero>().GetX(), heroSelected.GetComponent<MoveHero>().GetZ()].GetComponent<PhotonView>().ViewID, "Cell");
        }
        //Changes the coordinates in MoveHero and turn
        photonView.RPC("SetHeroCoords", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID);
        if (changeTurn)
        {
            yield return new WaitForSeconds(1.0f);
            heroSelected.GetComponent<PhotonView>().RPC("ChangeTurn", RpcTarget.All);
        }
        heroSelected = null;
    }

    void MoveSelectedHero() //Move hero
    {
        if (tag != "FirstRowCell" && tag != "LastRowCell")
        {
            ChangeRotationOfHero();
            //Play animation for walking
            PhotonView photonView = PhotonView.Get(GetComponent<PhotonView>());
            photonView.RPC("SetHeroWalking", RpcTarget.All, heroSelected.GetComponent<PhotonView>().ViewID, true);
            //Animation speed set, if Tommy then all animations are 1s, the rest is dependable on distance
            if (heroSelected.name == "TommyApe(Clone)" || (Mathf.Abs(x - heroSelected.GetComponent<MoveHero>().GetX()) + Mathf.Abs(z - heroSelected.GetComponent<MoveHero>().GetZ())) == 1 && heroSelected.GetComponent<MoveHero>().movement != 1)
                StartCoroutine(MoveToPosition(transform.position, 1.0f, true));
            else if (Mathf.Abs(x - heroSelected.GetComponent<MoveHero>().GetX()) == 1 && Mathf.Abs(z - heroSelected.GetComponent<MoveHero>().GetZ()) == 1)
                StartCoroutine(MoveToPosition(transform.position, 1.2f, true));
            else
                StartCoroutine(MoveToPosition(transform.position, 2.0f, true));
        }
        else //If the game has not begun there is no need for animation
            StartCoroutine(MoveToPosition(transform.position, 0.0f, false));
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
