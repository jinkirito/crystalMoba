using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/**
 * La clase NetManage es la que se encarga como su nombre indica del manejo de la red y tambien del manejo de las ventanas y canvas que tienen que mostrarse en cada momento
 */
public class NetManage : MonoBehaviour
{
    public Transform redNexusHolder;
    public Transform blueNexusHolder;

    public static NetManage current;
    public Text connectionState;
    private Transform[] RojoSpawnPoints;
    private Transform[] AzulSpawnPoints;
    public GameObject canvas;
    private string playerPrefabName;
    public string version;
    public LoginWindow loginWindow;
    public PlayerHub playerHub;
    public GameObject sceneCamera;
    public GameObject mainCamera;
    private float timeToRespawn;
    private float time;
    private string username;
    private int userTeam;
    public LobbyWindow lobbyWindow;
    public CreateRoomWindow createRoomWindow;
    public GameObject audiologin;
    public GameObject audiopartida;
    public SeleccionJugador seleccionJugador;
    public VictoryWindow victoryWindow;
    public GameObject lostConexion;


    /**
     * en esta funcion start loq ue hace el netmanage es buscar los puntos indicados en el mapa de aparecer de el equipo azul y rojo 
     */
    void Start()
    {
           
        int i;
        current = this;
        GameObject bsm = GameObject.FindGameObjectWithTag("AzulSpanw");
        GameObject rsm = GameObject.FindGameObjectWithTag("RojoSpanw");
        

        this.AzulSpawnPoints = new Transform[bsm.transform.childCount];
        this.RojoSpawnPoints = new Transform[rsm.transform.childCount];

      for( i = 0;i < bsm.transform.childCount; i++)
        {
            this.AzulSpawnPoints[i] = bsm.transform.GetChild(i);
        }

        for ( i = 0; i < rsm.transform.childCount; i++)
        {
            this.RojoSpawnPoints[i] = rsm.transform.GetChild(i);
        }
        
        
        this.timeToRespawn = 5;
        this.time = this.timeToRespawn;
    }

    //====================================================
    /**
     * la funcion multiplayer lo que hace es conectarnos al servisor con la version que le pasemos
     */
    public void Multiplayer()
    {
      

            // usa el archivo setting de nuestro PhotonNetwork asset
            PhotonNetwork.ConnectUsingSettings(this.version);
       
    }

    //====================================================
    /**
     * La funcion single player lo que hacia era activar el modo ofline y despues unirnos al lobby
     */
    public void SinglePlayer()
    {
        PhotonNetwork.offlineMode = true;
        this.OnJoinedLobby();
    }

    //====================================================
    /**
     * la funcion OnJoinedLobby lo que hace es desactivar los paneles de loginwindows y el de victorywindows y mostrar su panel
     */
    void OnJoinedLobby()
    {
          
        this.loginWindow.gameObject.SetActive(false);
        this.lobbyWindow.gameObject.SetActive(true);
        this.victoryWindow.gameObject.SetActive(false);
     
       
    }

    //======================================================
    /**
     * esta funcion lo que hacia es que si fallaba al unirse a una sala automaticamente creaba una sala y se unia.
     */
    void OnPhotonJoinRoomFailed()
    {
        //PhotonNetwork.CreateRoom("Sala 1");
        
    }

    //=======================================================
    /**
     * esta funcion OnJoinedRoom lo que hace es desactivar las ventanas q no hacen falta y activar la de selecciona un jugador una vez entre en esta ventana en el mapa apareceran automaticamente los nexos 
     */
    void OnJoinedRoom()
    {
        



        this.loginWindow.gameObject.SetActive(false);
        this.createRoomWindow.gameObject.SetActive(false);
        this.lobbyWindow.gameObject.SetActive(false);
        this.seleccionJugador.gameObject.SetActive(true);
        
        //si soy el jugador que ha creado la sala entonces crea los nexos en su sitio correspondiente
        if (PhotonNetwork.player.IsMasterClient)
        {
            PhotonNetwork.Instantiate("RedNexus", this.redNexusHolder.position, this.redNexusHolder.rotation, 0);
            PhotonNetwork.Instantiate("BlueNexus", this.blueNexusHolder.position, this.blueNexusHolder.rotation, 0);
        }
    }

    //=======================================================
    /**
     * funcion cuando se muera el jugador desactiva la camara principal y pone la de la escena  el tiempo a 0
     */
    public void PlayerDeath()
    {
        this.sceneCamera.SetActive(true);
        this.mainCamera.SetActive(false);
        this.time = 0;
    }
    // funcion cuando se destruya un nexo
    public void OnGameEnded(int looserTeam)
    {
        this.victoryWindow.gameObject.SetActive(true);
        if(looserTeam == 0)
        {
            this.victoryWindow.victoryText.text = "Ha ganado el equipo Rojo!!!";

        }
        else
        {
            this.victoryWindow.victoryText.text = "Ha ganado el equipo Azul!!!";
        }
    }

    /**
     * la funcion update lo que hace es mostrar el estatus del servidor todo el tiempo y contar si el tiempo que tiene el jugador despues de muerto llega a superar el tiempo
     * puesto para respauwnear o aparecer si lo supera llama a la funcion de spawnplayer
     */
    void Update()

    {
        
        
        //muestra el estado del servidor en pantalla
        this.connectionState.text = PhotonNetwork.connectionStateDetailed.ToString();
        if(this.time < this.timeToRespawn)
        {
            this.time += Time.deltaTime;
            if(this.time >= this.timeToRespawn)
            {
                //Respauwneamos
                this.SpawnPlayer();

            }
        }
    }

    /**
     * La funcion SpawnPlayer es la que se encarga de generar el jugador primero comprueba si el jugador tiene nombre si no lo tiene se genera uno por defecto y 
     * segun el equipo escogido lo generara en un punto aleatorio de los 3 puntos disponibles de cada equipo
     */
    void SpawnPlayer()
    {
        this.audiologin.SetActive(true);
        this.seleccionJugador.gameObject.SetActive(false);
        Vector3 position;
        object[] data;

        
        data = new object[2];
        if (this.loginWindow.playerNameField.text == string.Empty)
        {
            data[0] = "Renegado";
        }
        else
        {
            data[0] = this.loginWindow.playerNameField.text;
        }
        data[1] = this.loginWindow.teamSelector.value;
        this.username = (string)data[0];
        this.userTeam = (int)data[1];
        PhotonNetwork.player.name = this.username;
        this.sceneCamera.SetActive(false);
        this.mainCamera.SetActive(true);
      //  this.playerHub.gameObject.SetActive(false);

        if (this.userTeam == 0)
        {
            position = this.AzulSpawnPoints[Random.Range(0, this.AzulSpawnPoints.Length)].position;
        }
        else
        {
            position = this.RojoSpawnPoints[Random.Range(0, this.RojoSpawnPoints.Length)].position;
        }
        PhotonNetwork.Instantiate(this.playerPrefabName, position,
                                  Quaternion.identity, 0, data);
        this.playerHub.gameObject.SetActive(true);
    }


    //Gestion de ventanas

    public void goToCreateRoom()
    {
        this.lobbyWindow.gameObject.SetActive(false);
        this.createRoomWindow.gameObject.SetActive(true);
    }

    /**
     * Funcion para salir del juego
     */

    public void SalirDeljuego()
    {
        Application.Quit();
    }

   /**
    * Funcion que gestiona volver a la ventana de login en caso de perder la conxion
    */

    public void goToLoginError()
    {
        this.lostConexion.SetActive(false);
        this.loginWindow.gameObject.SetActive(true);
    }

    /**
     * Funcion que permite mostrar el mensaje de error de que no hay conexion.
     */

    public  void lostConnection()
    {
        this.lostConexion.SetActive(true);
        this.loginWindow.gameObject.SetActive(false);
    }

    /**
     * funcion que permite cancelar en la creacion de sala 
     */

    public void CancelCreateRoom()
    {
        this.lobbyWindow.gameObject.SetActive(true);
        this.createRoomWindow.gameObject.SetActive(false);
    }

    /**
     * Funcion de el boton de salir despues de acabar una partida
     */

    public void VictoriaSalir()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinLobby();
    }

    /**
     * Esta funcion nos unira a una partida aleatoria si no hay partidas no nos unira a ninguna
     */

    public void PartidaRapida()
    {
        if (PhotonNetwork.countOfRooms <= 0)
        {

        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
            this.lobbyWindow.gameObject.SetActive(false);
        }
       
    }

    /**
     * funcion donde se indica el nombre de la sala y el maximo de jugadores para poder crearse
     */
    public void CreateRoom ( string name, int numPlayers)
    {
       
        RoomOptions roomOptions = new RoomOptions() { isVisible = true, maxPlayers = (byte)numPlayers };

        PhotonNetwork.CreateRoom(name, roomOptions, null);
        this.createRoomWindow.gameObject.SetActive(false);
    }

    /**
     * Funcion que sirve para unirnos a una sala de la lista de las salas del lobby
     */

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        this.lobbyWindow.gameObject.SetActive(false);
    }

    /**
     * Funcion que indica el campeon que utiliza el jugador y lo respawnea
     */
    
    public void SetPlayerInfo()
    {
        this.playerPrefabName = this.seleccionJugador.CampeonNombre.text;
        this.SpawnPlayer();
        this.audiologin.SetActive(false);
        this.audiopartida.SetActive(true);
       // this.playerHub.gameObject.SetActive(true);
    }

    public static void ErrorConec()
    {
        
    }
    
}
