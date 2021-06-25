using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.U2D;

public class Player : MonoBehaviourPunCallbacks
{
    [SerializeField] 
    private float maxHealth = 100f;

    //[SyncVar]
    private float currentHealth = 100f;

    public float GetHealthPct()
    {
        return (float)currentHealth / maxHealth;

    }

    public int money = 0;
    

    private void awake()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        //Debug.Log(transform.name + "a maintenant : " + currentHealth + "point de vie");
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        GameManager.players[name].GetComponent<PlayerController>().enabled = false;
        GameManager.players[name].GetComponent<PlayerShoot>().enabled = false;
        GameManager.UnregisterPlayer(name);
        //Destroy(transform.gameObject);
    }
    
}
