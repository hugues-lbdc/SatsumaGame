using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class autoSpawn : MonoBehaviourPunCallbacks
{
    public Player player;

    public GameObject[] listSpawn;
    public GameObject spawn;

    public GameObject Samurai;
    public GameObject Boss;

    public int distanceSpawn;

    public float spawnRate = 3f;

    public int maxSpawnInMap;
    private int maxSpawnManche;
    public int maxSpawnLastManche;
    
    public bool BossCanSpawn;

    public int nManche = 1;
    public float timeNextManche;
    private float debutDeNouvelleManche;

    private float nextSpawn;
    private int nr;
    private float distance;
    private int nb;
    private int numeroIA = 1;

    public GameObject AffichageMenuUI;
    public int NumberManche = 1;
    public float temps = 3;
    public int tempsInt;

    public int bossLife ;
    private int samuLife ;


    private void Start()
    {
        bossLife = 0;
        samuLife = 0;
        listSpawn = GameObject.FindGameObjectsWithTag("spawn samurai");
        maxSpawnInMap = 50;
        maxSpawnManche = 2;
        maxSpawnLastManche = 2;
        timeNextManche = Time.time + 3;
        BossCanSpawn = true;
    }

    
    void Update()
    {

        foreach (KeyValuePair<string, Player> p in GameManager.players)
        {
            player = p.Value;
            //WhereSpawn() sert a determiner quel spawn va etre utiliser par la prochaine IA 
            WhereSpawn();
            if (Time.time > timeNextManche && maxSpawnManche > 0 && Time.time > nextSpawn)
                //on appel CanSpawn() qui s'occupe du spawn de l'IA 
            {
                CanSpawn();
                if (AffichageMenuUI.activeInHierarchy)
                {
                    AffichageMenuUI.SetActive(false);
                }

                if (BossCanSpawn)
                    BossSpawn();
            }

            //Si max spawn manche est a 0 et qu'il n'y a plus d'IA dans le jeux
            if (maxSpawnManche == 0 && GameManager.Ennemis.Count == 0)
                //On appel changeManche() qui passe a la manche suivante
            {
                ChangeManche();
            }
        }
    }

    
    
    
    public void CanSpawn()
    {
        nextSpawn = Time.time + new Random().Next(1,3);
        GameObject GO = PhotonNetwork.Instantiate(Samurai.name, spawn.transform.position, Quaternion.identity);
        GO.GetComponent<NavMeshAgent>().speed = new Random().Next(2, 4);
        GO.name += numeroIA;
        numeroIA++;
        print(GO.name);
        GameManager.RegisterEnemie(GO.name, GO.AddComponent<Ennemi>());
        GameManager.GetEnemie(GO.name).ApplyDammage(-samuLife);
        maxSpawnManche--;

        print(GameManager.Ennemis.Count);
    }

    public void BossSpawn()
    {
        BossCanSpawn = false;
        GameObject GO = PhotonNetwork.Instantiate(Boss.name, spawn.transform.position, Quaternion.identity);
        GO.name += numeroIA;
        bossLife += 50 * (nManche / 5);
        GameManager.RegisterEnemie(GO.name, GO.AddComponent<Ennemi>());
        GameManager.GetEnemie(GO.name).ApplyDammage(-bossLife);
    }

    public void ChangeManche()
    {
        
        maxSpawnManche = maxSpawnLastManche + new Random().Next(maxSpawnLastManche);
        maxSpawnLastManche = maxSpawnManche;
        nManche++;
        debutDeNouvelleManche = Time.time;
        timeNextManche = Time.time + 5;
        numeroIA = 1;
        samuLife += new Random().Next(3, 7);
        
        AffichageMenuUI.SetActive(true);

        if (new Random().Next(4) == 1)
            BossCanSpawn = true;
    }

    
    void WhereSpawn()
    {
        spawn = listSpawn[0];
        float distance = Vector3.Distance(player.transform.position, spawn.transform.position);
        foreach (GameObject gameObject in listSpawn)
        {
            if (Vector3.Distance(player.transform.position, gameObject.transform.position) < distance && distance > 4)
            {
                spawn = gameObject;
                distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
            }
        }
    }
}
