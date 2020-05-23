using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * La clase de crateroom es la que se usa para coger el numero de jugadores y el nombre para crear la sala en el NetManager
 */
public class CreateRoomWindow : MonoBehaviour
{
    public InputField roomNameField;
    public Text maxPlayersLabel;
    public int MaxNumPlayers;

    public void ModifyPlayerNum(int a)
    {
        this.MaxNumPlayers += a;
        if(this.MaxNumPlayers < 2)
        {
            this.MaxNumPlayers = 2;
        }else if(this.MaxNumPlayers >= 6)
        {
            this.MaxNumPlayers = 6;
        }

        this.maxPlayersLabel.text = this.MaxNumPlayers.ToString();
    }

    //Le manda el nombre de la sala y el maximo de jugadores al netmanage
    public void CreateRoom()
    {
        string Roomname;
        Roomname = this.roomNameField.text;
        if(Roomname == string.Empty)
        {
            Roomname = "DefaultRoom";
        }
        NetManage.current.CreateRoom(Roomname, this.MaxNumPlayers);
    }
}
