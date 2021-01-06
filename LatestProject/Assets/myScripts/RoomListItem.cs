using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;


public class RoomListItem : MonoBehaviour
{
    public TMP_Text Text;
    public RoomInfo roomInfo;

    public void SetUp(RoomInfo Info)
    {
        roomInfo = Info;
        Text.text = Info.Name;
    }

    public void OnClick()
    {
        Launch.GetClass.JoinRoom(roomInfo);
    }
}
