using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public float time;
    public float timeOut;
    protected Skill skill;
    protected PhotonView photonview;

    private void Awake()
    {
        this.time = 0;
        this.photonview = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.photonview.isMine)
        {
            this.time += Time.deltaTime;
            if(this.time > this.timeOut)
            {
                this.photonview.RPC("AutoDestroy", PhotonTargets.All, null);
            }
        }
        this.Tick();
    }

    public void SetSkill(Skill _skill)
    {
        this.skill = _skill;
    }

    [PunRPC]
    public void AutoDestroy()
    {
        if (this.photonview.isMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    protected abstract void Tick();
}
