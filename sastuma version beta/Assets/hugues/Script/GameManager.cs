// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to handle typical game management requirements
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
	#pragma warning disable 649

	/// <summary>
	/// Game manager.
	/// Connects and watch Photon Status, Instantiate Player
	/// Deals with quiting the room and the game
	/// Deals with level loading (outside the in room synchronization)
	/// </summary>
	public class GameManager : MonoBehaviourPunCallbacks
    {
	    
	    
	    //public GameObject playerPrefab;
    
	    private const string playerIdPrefix = "Player";

	    public static Dictionary<string, Player> players = new Dictionary<string, Player>();

	    public static Dictionary<string, Ennemi> Ennemis = new Dictionary<string, Ennemi>();

	    public int numPlayer = 1;

	    /*private void Start()
	    {
	        if (playerPrefab != null)
	            PhotonNetwork.Instantiate(this.playerPrefab.name, PlayerSpawn(), Quaternion.identity, 0);
	    }*/
    
	    private Vector3 PlayerSpawn()
	    {
		    //int random = new Random().Next(0, 3);
		    int random = 1;
		    Vector3 result = new Vector3(278, (float) 0.84, 187);
		    switch (random)
		    {
			    case 0:
				    return new Vector3(278, (float) 0.84, 187);
			    case 1:
				    return new Vector3(280, (float) 0.84, 187);
			    case 2:
				    return new Vector3(278, (float) 0.84, 189);
		    }
		    return new Vector3(280, (float) 0.84, 189);
        
	    }

	    public static void RegisterPlayer(string netID, Player player)
	    {
		    string playerId = playerIdPrefix + netID;
		    players.Add(playerId, player);
		    player.transform.name = playerId;
	    }

	    public static void UnregisterPlayer(string playerId)
	    {
		    players.Remove(playerId);
	    }
    
	    public static Player GetPlayer(string playerId)
	    {
		    return players[playerId];
	    }
    
	    public static void RegisterEnemie(string name, Ennemi ennemi)
	    {
		    Ennemis.Add(name, ennemi);
	    }

	    public static void UnregisterEnemie(string name)
	    {
		    Ennemis.Remove(name);
	    }
    
	    public static Ennemi GetEnemie(string ennemi)
	    {
		    return Ennemis[ennemi];
	    }
	    
	    
	    
	    

		#region Public Fields

		static public GameManager Instance;

		#endregion

		#region Private Fields

		private GameObject instance;

        [Tooltip("The prefab to use for representing the player")]
        [SerializeField]
        private GameObject playerPrefab;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
		{
			Instance = this;

			// in case we started this demo with the wrong scene being active, simply load the menu scene
			if (!PhotonNetwork.IsConnected)
			{
				SceneManager.LoadScene("PunBasics-Launcher");

				return;
			}

			if (playerPrefab == null) { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

				Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
			} else {


				if (PlayerManager.LocalPlayerInstance==null)
				{
				    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

					// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
					GameObject player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(276.1844f,4f,194.75f), Quaternion.identity, 0);
					RegisterPlayer("Player"+numPlayer,player.GetComponent<Player>());
					numPlayer += 1;
				}else{

					Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
				}


			}

		}

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// </summary>
		void Update()
		{
			// "back" button of phone equals "Escape". quit app if that's pressed
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				QuitApplication();
			}
		}

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when a Photon Player got connected. We need to then load a bigger scene.
        /// </summary>
        /// <param name="other">Other.</param>
        public void OnPlayerEnteredRoom( Player other  )
		{
			//Debug.Log( "OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

			if ( PhotonNetwork.IsMasterClient )
			{
				Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

				LoadArena();
			}
		}

		/// <summary>
		/// Called when a Photon Player got disconnected. We need to load a smaller scene.
		/// </summary>
		/// <param name="other">Other.</param>
		public void OnPlayerLeftRoom( Player other  )
		{
			//Debug.Log( "OnPlayerLeftRoom() " + other.NickName ); // seen when other disconnects

			if ( PhotonNetwork.IsMasterClient )
			{
				Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

				LoadArena(); 
			}
		}

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public override void OnLeftRoom()
		{
			SceneManager.LoadScene("PunBasics-Launcher");
		}

		#endregion

		#region Public Methods

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

		public void QuitApplication()
		{
			Application.Quit();
		}

		#endregion

		#region Private Methods

		void LoadArena()
		{
			if ( ! PhotonNetwork.IsMasterClient )
			{
				Debug.LogError( "PhotonNetwork : Trying to Load a level but we are not the master Client" );
			}

			Debug.LogFormat( "PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount );

			PhotonNetwork.LoadLevel("PunBasics-Room for "+PhotonNetwork.CurrentRoom.PlayerCount);
		}

		#endregion

	}
}