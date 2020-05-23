using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * La clase combat system es la qie se encarga de gestionar el sistema de combate el que indica los controles para poder hacer las habilidades ,
 * tambien gestiona si es una nexo o no y de que equipo es para poder hacerle daño, aqui es donde se gestiona de que equipo es cada jugador.
 */

public class CombatSystem : MonoBehaviour
{
    private int maxHp;
    public int currenHp;
    private PhotonView pv;
    public Skill q_skill;
    public Skill e_skill;
    public Skill ml_skill;
    public Skill mr_skill;
    public Slider HPSlider;
    public Slider MNSlider;
    private int currenMana;
    private int maxMana;
    private float manaRecargaTime;
    private float time;
    private int team;
    private Animator anim;
    public bool isNexus;
    public int NexusTeam;

    //==============================
    void Start()
    {
        this.maxHp = 100;
        this.maxMana = 100;
        this.currenMana = this.maxMana;
        this.currenHp = this.maxHp;
        this.manaRecargaTime = 1;
        this.time = 0;
        if(isNexus == true)
        {
            this.SetTeam(NexusTeam);
        }
        
        this.pv = GetComponent<PhotonView>();
        this.anim = GetComponent<Animator>();
        if (this.q_skill != null)
        {
            
            this.q_skill.SetPhotonView(this.pv);
            this.q_skill.SetCombatSystem(this);
            
        }

        if (this.e_skill != null)
        {
            
            this.e_skill.SetPhotonView(this.pv);
            this.e_skill.SetCombatSystem(this);
        }

        if (this.ml_skill != null)
        {
            
            this.ml_skill.SetPhotonView(this.pv);
            this.ml_skill.SetCombatSystem(this);
        }

        if (this.mr_skill != null)
        {
            
            this.mr_skill.SetPhotonView(this.pv);
            this.mr_skill.SetCombatSystem(this);

        }

    }

    //==============================================
    void Update()
    {
        if (this.pv.isMine && !this.isNexus)
        {
            //Recarga el mana
            this.time += Time.deltaTime;
            if(this.time >= this.manaRecargaTime)
            {
                this.time = 0;
                this.Modifymana(10);
            }

            //Input de las habilidades
            // las habilidades compruean si tiene una accion seleccionada si lo tiene despues compruba el coste de mana y si tenemos suficiente para ejecutarla
            // si tenemos suficiente y es ejecutada restara al mana del personaje el coste de la habilidada
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if(this.q_skill != null)
                {
                    if (this.q_skill.GetManaCost() <= this.currenMana)
                    {
                        if (this.q_skill.Execute())
                        {
                            this.Modifymana(-this.q_skill.GetManaCost());
                        }
                        
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (this.e_skill != null)
                {
                    if (this.e_skill.GetManaCost() <= this.currenMana)
                    {
                        if (this.e_skill.Execute())
                        {
                            this.Modifymana(-this.e_skill.GetManaCost());
                        }
                    }
                }
            }
           else if (Input.GetButtonDown("Fire1"))
            {
               if(this.ml_skill != null)
                {
                    if (this.ml_skill.GetManaCost() <= this.currenMana)
                    {
                        
                            if (this.ml_skill.Execute())
                            {
                                this.anim.SetTrigger("Atack");
                                this.Modifymana(-this.ml_skill.GetManaCost());
                            }
                        
                    }
                }
            }
           else if (Input.GetButtonDown("Fire2"))
            {
                if (this.mr_skill != null)
                {
                    if (this.mr_skill.GetManaCost() <= this.currenMana)
                    {
                        if (this.mr_skill.Execute())
                        {
                            this.Modifymana(-this.mr_skill.GetManaCost());
                        }
                    }
                }
            }
        }
    }

    public void Modifymana(int amount)
    {
        this.currenMana += amount;
        if(this.currenMana < 0)
        {
            this.currenMana = 0;
        }
        if(this.currenMana > this.maxMana)
        {
            this.currenMana = this.maxMana;
        }
        this.MNSlider.value = this.currenMana;
    }
    //==========================================
    public PhotonView GetPhotonview()
    {
        return this.pv;
    }


    public int GetTeam()
    {
        return this.team;
    }

    public void SetTeam(int _team)
    {
        this.team = _team;
    }

    //==========================================
    [PunRPC]
   public void ModifyHealth ( int amount)
    {
        this.currenHp += amount;

        if(currenHp > this.maxHp)
        {
            this.currenHp = this.maxHp;
        }
        Debug.Log("me han echo daño");
        if (this.currenHp <= 0)
        {
            this.currenHp = 0;

            //Hacemos algo al morir
            if (this.pv.isMine)
            {
                //al morir el jugador llama al netmanage y activa la funcion de camara de muerte y destruye el objeto en toda la red
                NetManage go = GameObject.FindGameObjectWithTag("NetManager").GetComponent<NetManage>();
                
                if (this.isNexus)
                {
                    go.OnGameEnded(this.team);
                }
                else
                {
                    go.PlayerDeath();
                }
                PhotonNetwork.Destroy(this.gameObject);
            }

        }
        // muestra en el Hub nuestra vida a traves de un slider 
        if(this.HPSlider != null)
        {
            this.HPSlider.value = this.currenHp;
        }

    
    }

    
    
}
