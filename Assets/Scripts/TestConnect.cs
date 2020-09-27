using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        print("Connecting to server...");
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
	    print( "Connected to server" );
        print(PhotonNetwork.LocalPlayer.NickName);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected( DisconnectCause cause ) {
	    print("Disconnected from server for reason: " + cause.ToString());
    }

    public override void OnJoinedLobby()
    {
	    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions {MaxPlayers = 2}, TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
	    print( "OnJoinedRoom" );
	    PhotonNetwork.Instantiate("Player", new Vector3(0,0,0), Quaternion.identity);
    }
}
