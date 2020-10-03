using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
	[SerializeField] private float speed = 15f;
	[SerializeField] private Color color = Color.blue;
	private Vector2 sizePlayer = new Vector2(0.30f, 0.05f);

	private Vector2 camSize;
	private PhotonView photonView;
	private Vector2 screenPercentPosition;
	private Vector3 position;
	private float maxScreenPosition = 0f;

	void Start()
	{
		photonView = GetComponent<PhotonView>();

		GetComponent<SpriteRenderer>().material.color = color;

		transform.localScale = new Vector3( NetworkGameManager.GetScreenToWorldWidth * sizePlayer.x,
			NetworkGameManager.GetScreenToWorldHeight * sizePlayer.y, 1f);

		screenPercentPosition = new Vector2(0f, (-NetworkGameManager.GetScreenToWorldHeight / 2 + transform.localScale.y / 2) /
		                                        ( NetworkGameManager.GetScreenToWorldHeight / 2));
		transform.localPosition = new Vector3( 0f, screenPercentPosition.y * NetworkGameManager.GetScreenToWorldHeight / 2, 0f );
		maxScreenPosition = ( NetworkGameManager.GetScreenToWorldWidth / 2 - transform.localScale.x / 2) /
		                    ( NetworkGameManager.GetScreenToWorldWidth / 2);

		if ( photonView.IsMine && !PhotonNetwork.IsMasterClient ) {
			Camera.main.transform.Rotate( new Vector3(0,0,180));
			transform.localPosition = new Vector3( 0f, -screenPercentPosition.y * NetworkGameManager.GetScreenToWorldHeight / 2, 0f );
		}
	}


	void Update()
	{
		if (photonView.IsMine)
		{
			int masterClient;
			if (PhotonNetwork.IsMasterClient)
				masterClient = 1;
			else
				masterClient = -1;

			screenPercentPosition.y = masterClient * ( -NetworkGameManager.GetScreenToWorldHeight / 2 + transform.localScale.y / 2) /
			                          ( NetworkGameManager.GetScreenToWorldHeight / 2);
			
			if (Input.GetKey(KeyCode.RightArrow))
			{
				screenPercentPosition.x += masterClient * speed / 10f * Time.deltaTime;
			}

			if (Input.GetKey(KeyCode.LeftArrow))
			{
				screenPercentPosition.x -= masterClient * speed / 10f * Time.deltaTime;
			}

			if ( screenPercentPosition.x > maxScreenPosition ) {
				screenPercentPosition.x = maxScreenPosition;
			}

			if ( screenPercentPosition.x < -maxScreenPosition ) {
				screenPercentPosition.x = -maxScreenPosition;
			}

			//if ( Input.touchCount != 1 )
			// return;
			//if ( Input.touches[0].phase == TouchPhase.Moved ) {
			// if ( transform.position.x + transform.localScale.x / 2 + Input.touches[0].deltaPosition.x > camSize.x ||
			//      transform.position.x - transform.localScale.x / 2 - Input.touches[0].deltaPosition.x < -camSize.x ) {
			//  return;
			// }
			// transform.position += new Vector3( Input.touches[0].deltaPosition.x * ( speed * Time.deltaTime ), 0f );
			//}

			transform.localScale = new Vector3( NetworkGameManager.GetScreenToWorldWidth * sizePlayer.x, NetworkGameManager.GetScreenToWorldHeight * sizePlayer.y, 1f );
			transform.position = new Vector3( screenPercentPosition.x * NetworkGameManager.GetScreenToWorldWidth / 2,
				screenPercentPosition.y * NetworkGameManager.GetScreenToWorldHeight / 2, 0f );
			maxScreenPosition = ( NetworkGameManager.GetScreenToWorldWidth / 2 - transform.localScale.x / 2 ) / ( NetworkGameManager.GetScreenToWorldWidth / 2 );
		}
		if(!photonView.IsMine){
			transform.localScale = new Vector3( NetworkGameManager.GetScreenToWorldWidth * sizePlayer.x,
				NetworkGameManager.GetScreenToWorldHeight * sizePlayer.y, 1f );
			transform.position = new Vector3( screenPercentPosition.x * NetworkGameManager.GetScreenToWorldWidth / 2,
				screenPercentPosition.y * NetworkGameManager.GetScreenToWorldHeight / 2, 0f );
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(screenPercentPosition);
		}
		else
		{
			screenPercentPosition = (Vector2) stream.ReceiveNext();
		}
	}
}