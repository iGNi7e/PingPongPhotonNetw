using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkGameManager : MonoBehaviourPunCallbacks
{
	private Vector2 camSize;
	private Object[] Sprites;
	private static Vector2 aspectRatio;

	public static float GetScreenToWorldHeight
	{
		get
		{
			Vector2 topRightCorner = new Vector2( 1, 1 );
			Vector2 edgeVector = Camera.main.ViewportToWorldPoint( topRightCorner );
			var height = edgeVector.y * 2;
			return height;
		}
	}

	public static float GetScreenToWorldWidth
	{
		get
		{
			Vector2 topRightCorner = new Vector2( 1, 1 );
			Vector2 edgeVector = Camera.main.ViewportToWorldPoint( topRightCorner );
			var width = edgeVector.x * 2;
			return width;
		}
	}

	void Start()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        
    }

    public void Leave()
    {
	    PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
	    //When isLocal  player leave from room
	    SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
	    Debug.LogFormat("{0} entered room.", newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
	    Debug.LogFormat( "{0} left room.", otherPlayer.NickName );
    }
}
