using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase configura la interfaz del usuario
public class NetPlayer : MonoBehaviour
{
    
    public GameObject playerDataCanvasPrfb;
    private GameObject playerDataCanvas;
    private Rigidbody rigbody;
    private Vector3 realPosition;
    private Quaternion realRotation;
    private PhotonView photonView;
    private Animator animConroller;



    //==============================
    /**
     * En la funcion start lo que hace es coger los spriters y las barras para poder ser configuradas las barras de cooldown time de las habilidades y el de la vida y el mana 
     * tambien gestiona que si hacemos una habilidad por ejemplo la de ml haga la animacion, tambiena hace una copia en la red para que los otros jugadores vean las animaciones y su vida
     */
    void Start()
    {
        this.animConroller = GetComponent<Animator>();
        //PhotonView pv = GetComponent<PhotonView>();
        this.photonView = GetComponent<PhotonView>();
        object[] data;
        data = photonView.instantiationData;
        if (photonView.isMine)
        {
            // Nuestro presonaje 
            GameObject netmanagerObj;
            CombatSystem cs;
            NetManage netmanagerScript;
            rigbody = GetComponent<Rigidbody>();
            GetComponent<PlayerControl>().enabled= true;
            rigbody.isKinematic = false;
            this.photonView = GetComponent<PhotonView>();
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

            if(camera != null)
            {
               PlayerCamera pc = camera.GetComponent<PlayerCamera>();
                pc.enabled = true;
                pc.player = GetComponent<PlayerControl>();
            }

             netmanagerObj = GameObject.FindGameObjectWithTag("NetManager");
            cs = GetComponent<CombatSystem>();
             netmanagerScript = netmanagerObj.GetComponent<NetManage>();

            cs.HPSlider = netmanagerScript.playerHub.playerHPbar;
            cs.MNSlider = netmanagerScript.playerHub.playerManabar;

            if(cs.q_skill != null)
            {
                netmanagerScript.playerHub.skill_qImg.sprite = cs.q_skill.skillSprite;
                cs.q_skill.coolDownSlider = netmanagerScript.playerHub.skill_q;
            }
            if (cs.e_skill != null)
            {
                netmanagerScript.playerHub.skill_eImg.sprite = cs.e_skill.skillSprite;
                cs.e_skill.coolDownSlider = netmanagerScript.playerHub.skill_e;
            }
            if (cs.ml_skill != null)
            {
                netmanagerScript.playerHub.skill_mlImg.sprite = cs.ml_skill.skillSprite;
                cs.ml_skill.coolDownSlider = netmanagerScript.playerHub.skill_ml;
            }
            if (cs.mr_skill != null)
            {
                netmanagerScript.playerHub.skill_mrImg.sprite = cs.mr_skill.skillSprite;
                cs.mr_skill.coolDownSlider = netmanagerScript.playerHub.skill_mr;
            }
            Debug.Log((int)data[1]);
            cs.SetTeam((int)data[1]);
        }
        else
        {
            //copia el personaje en la red
            PlayerDataCanvas pcb;
            CombatSystem cs;

            this.playerDataCanvas =(GameObject)Instantiate(this.playerDataCanvasPrfb,
                this.transform.position + Vector3.up * 2,
                Quaternion.Euler(new Vector3(50, 0, 0)));

            pcb = this.playerDataCanvas.GetComponent<PlayerDataCanvas>();
            pcb.targetPlayer = this.transform;
            pcb.playerName.text = (string)data[0];

            cs = GetComponent<CombatSystem>();
            cs.HPSlider = pcb.playerHPBar;
            Debug.Log((int)data[1]);
            cs.SetTeam((int)data[1]);
           
            
        }
        
    }



    private void OnDestroy()
    {
        if(this.playerDataCanvas != null)
        {
            Destroy(this.playerDataCanvas);
        }
    }

    void Update()
    {
        if (!this.photonView.isMine)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.realPosition, Time.deltaTime * 10);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.realRotation, Time.deltaTime * 10);
        }

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // enviar datos por la red
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
            stream.SendNext(this.animConroller.GetFloat("VelX"));
            stream.SendNext(this.animConroller.GetFloat("VelY"));
           // stream.SendNext(this.animConroller.get);
        }
        else
        {
            // recibir datos por la red
            this.realPosition= (Vector3)stream.ReceiveNext();
            this.realRotation = (Quaternion)stream.ReceiveNext();
            this.animConroller.SetFloat("VelX", (float)stream.ReceiveNext());
            this.animConroller.SetFloat("VelY", (float)stream.ReceiveNext());
        }
    }

    // Update is called once per frame
   
}
