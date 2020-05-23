using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LobbyWindow : MonoBehaviour
{
    public RectTransform roomListPanel;
    public GameObject roomSlotPrb;
    private List<GameObject> actualRoomSlots;

    private void Start()
    {
        this.actualRoomSlots = new List<GameObject>();
    }


    public void RefreshRoomList()
    {
        // Limpia la lista de salas 
        if(this.actualRoomSlots.Count > 0)
        {
            foreach(GameObject slot in this.actualRoomSlots)
            {
                Destroy(slot);
            }
            this.actualRoomSlots.Clear();
        }
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        int j = 100;
        foreach(RoomInfo room in rooms)
        {
            GameObject rsgo = Instantiate(this.roomSlotPrb) as GameObject;
            RectTransform rst = rsgo.GetComponent<RectTransform>();
            RoomSlotData rsd = rsgo.GetComponent<RoomSlotData>();

            rst.parent = this.roomListPanel;
            rst.anchoredPosition = new Vector2(0, j);
            rst.localScale = Vector3.one;

            rsd.roomNameLabel.text = room.name;
            rsd.playersLabel.text = room.playerCount + "/" + room.maxPlayers;
            rsd.connectButton.onClick.AddListener(() => NetManage.current.JoinRoom(room.name));
            j -= 40;
        }
    }
}
