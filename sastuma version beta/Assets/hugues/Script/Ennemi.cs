using System.Linq;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class Ennemi : MonoBehaviourPunCallbacks
{
    //Distance entre le joueur et l'ennemi
    private float Distance;
 
    // Cible de l'ennemi
    public Transform Target;

    // Portée des attaques
    [SerializeField] private float attackRangeS;
    
    private float attackRange = 2;
    
 
    // Cooldown des attaques
    [SerializeField] private float attackRepeatTimeS;
    
    private float attackRepeatTime = 2f;
    private float attackTime;
 
    // Montant des dégâts infligés
    [SerializeField] private float theDamageS;
    
    private float theDamage = 10;
 
    // Agent de navigation
    private UnityEngine.AI.NavMeshAgent agent;
 
    // Animations de l'ennemi
    private Animation animations;
 
    // Vie de l'ennemi
    [SerializeField] private float lifeS;

    public float life = 20;
    private bool isDead = false;


    // Start is called before the first frame update
    void Start ()
    {
        print(name);
        if (tag == "wizard")
        {
            life = 100;
            theDamage = 25;
            attackRepeatTime = 3;
            attackRange = 2;
        }
        
        agent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        animations = gameObject.GetComponent<Animation>();
        attackTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // si l'ennemi n'est pas mort et qu'il y a enore des joueur en jeux alors:
        if (!isDead || GameManager.players.Count>0)
        {
            // On cherche le joueur en permanence
            Target = WhoIs();
            // On calcule la distance entre le joueur et l'ennemi, en fonction de cette distance on effectue diverses actions
            Distance = Vector3.Distance(Target.position, transform.position);

            // Quand l'ennemi est proche mais pas assez pour attaquer on le suis
            if (Distance > attackRange)
                chase();
            // Quand l'ennemi est assez proche pour attaquer on attaque
            else
                attack();
        }
        //Quand il n'y a plus de joueurs on tue toutes les entitee
        if (GameManager.players.Count == 0)
        {
            Dead();
        }
    }

    public void SetLife(float life)
    {
        this.life = life;
    }
    
    void chase()
    {
        animations.Play("Run");
        agent.destination = Target.position;
    }
    
    void attack()
    {
        // empeche l'ennemi de traverser le joueur
        agent.destination = transform.position;
 
        //Si pas de cooldown
        if (Time.time > attackTime) 
        {
            animations.Play("Attack");
            Debug.Log("L'ennemi a envoyé " + theDamage + " points de dégâts");
            attackTime = Time.time + attackRepeatTime;
            GameManager.players[Target.name].TakeDamage(theDamage);
        }
    }
    
    Transform WhoIs()
    {
        Transform tr = GameObject.Find("PlayerPlayer1").transform;
        foreach (var player in GameManager.players)
        {
            if (Vector3.Distance(tr.position, transform.position)> Vector3.Distance( player.Value.transform.position, transform.position))
                tr = player.Value.transform;
        }
        return tr;
    }
    
    public void ApplyDammage(float TheDammage)
    {
        if (!isDead)
        {
            life = life - TheDammage;
            //print(gameObject.name + "a subit " + TheDammage + " points de dégâts.");
 
            if(life <= 0)
            {
                Dead();
            }
        }
    }
    
    public void Dead()
    {
        isDead = true;
        GameManager.UnregisterEnemie(name);
        print(GameManager.Ennemis.Count);
        //animations.Play("die");
        Destroy(transform.gameObject);
        tag = "EnnemiDead";
    }
}
