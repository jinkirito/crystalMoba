﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataCanvas : MonoBehaviour
{
    public Transform targetPlayer;
    public Text playerName;
    public Slider playerHPBar;


    
    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.targetPlayer.position + Vector3.up * 2;
    }
}
