using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.Timeline;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Camera cam;

    public AudioClip shootSound;
    public AudioClip rechargeSound;

    [SerializeField]
    private LayerMask mask;
    private AudioSource audioSource;
    private Player player;
    private WeaponData currentWeapon;
    private WeaponManager weaponManager;
    private float timeRecharge;
    private float timeShoot;
    private int balles;
    

    // Start is called before the first frame update
    void Start()
    {
        timeRecharge = Time.time;
        timeRecharge = Time.time;
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();
        //Verification de l'existance de caméra chez le joueur
        if (cam == null)
        {
            Debug.LogError("Pas de caméra renseignée sur le système de tir.");
            this.enabled = false;
        }
        
        weaponManager = GetComponent<WeaponManager>();
        balles = GetComponent<WeaponManager>().primaryWeapon.balle;
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        //Lorsque le joueur appuie sur le clic gauche de la souris cela declanche le Raycaste partant de la caméra
        if (Input.GetButtonDown("Fire1") && Time.time > timeShoot)
            Shoot();
        if (Input.GetKey(KeyCode.R) && balles < GetComponent<WeaponManager>().primaryWeapon.balle || balles == 0)
            Recharge();
    }

    //[Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos,normal);
    }
    
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2f);
    }
    
    //Fonction appelee sur le serveur lorsque le joueur tir (on previent le serveur de notre tir)
    //[Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    //Fait aparaitre les effet de tir chez tout les clients / joueurs
    //[ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }
    
    
    //[Client]
    private void Shoot()
    {
        //photon
        if (!photonView.IsMine || balles <= 0)
            return;

        if (timeRecharge < Time.time)
        {
            audioSource.PlayOneShot(shootSound);
            balles--;
            timeShoot = Time.time + currentWeapon.timeForShoot;

            CmdOnShoot();

            RaycastHit hit;
            //si la raycast touche un ennemi
            //alors elle appel CmdPlayerShot avec le nom de l'ennemi et les dommaage de l'arme en main du player
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
            {
                if (hit.collider.tag == "Samu" || hit.collider.tag == "wizard")
                    CmdPlayerShot(hit.collider.name, currentWeapon.damage, player);

                CmdOnHit(hit.point, hit.normal);
            }
        }
    }

    //[Command]
    private void CmdPlayerShot(string ennemie, float damage, Player player)
    {
        //On applique les dommage a l'ennemi avec ApplyDammage
        player.money += 10;
        Ennemi enn = GameManager.GetEnemie(ennemie);
        enn.ApplyDammage(damage);
    }
    
    
    private void Recharge()
    {
        balles = GetComponent<WeaponManager>().primaryWeapon.balle;
        audioSource.PlayOneShot(rechargeSound);
        timeRecharge = Time.time + rechargeSound.length;
    }

}
