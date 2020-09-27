using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	public Text logText;
	private void Start()
	{
		PhotonNetwork.NickName = "Player_" + Random.Range(0, 999);
		Log("Player's name is " + PhotonNetwork.NickName );
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.GameVersion = "1.0.0";
		PhotonNetwork.ConnectUsingSettings();
		PhotonNetwork.SendRate = 20;
		PhotonNetwork.SerializationRate = 20;

	}

	public override void OnConnectedToMaster()
	{
		Log("Connected to server.");

	}

	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions{MaxPlayers = 2});
	}

	public void JoinRoom()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnJoinedRoom()
	{
		Log("Joined Room");
		PhotonNetwork.LoadLevel("Game");
	}

	private void Log(string message)
	{
		Debug.Log(message);
		logText.text += "\n";
		logText.text += message;
	}
}
