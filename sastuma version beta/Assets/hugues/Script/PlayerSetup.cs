using System.Runtime.Serialization;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    Camera sceneCamera;

    private void Start()
    {
        //photon
        if (!photonView.IsMine)
        {
            //Desactivation des componentes des autres joueurs
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }

            gameObject.layer = LayerMask.NameToLayer(remoteLayerName);

        }
        else
        {
            //Desactivation de la Main caméra
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            //Création du UI du joueur
            playerUIInstance = Instantiate(playerUIPrefab);
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    /*public void OnStartClient()
    {
        //base.OnStartClient();
        
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        
        Player player = GetComponent<Player>();
        
        GameManager.RegisterPlayer(netId,player);
    }*/

    private void OnDisable()
    {
        //Destruction du UI du joueur
        //Destroy(playerUIInstance);

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}
