using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IPunObservable
{
	[SerializeField] private float speed = 15f;
	[SerializeField] private Color color = Color.blue;
	private Vector2 sizePlayer = new Vector2(0.30f, 0.05f);

	private Vector2 camSize;
	private PhotonView photonView;
	private float widthScreen;
	private float heightScreen;
	private Vector2 screenPercentPosition;
	private Vector3 position;
	private float maxScreenPosition = 0f;

	void Start()
	{
		photonView = GetComponent<PhotonView>();

		GetComponent<SpriteRenderer>().color = color;

		camSize = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);

		widthScreen = NetworkGameManager.GetScreenToWorldWidth;
		heightScreen = NetworkGameManager.GetScreenToWorldHeight;
		transform.localScale = new Vector3(widthScreen * sizePlayer.x, heightScreen * sizePlayer.y, 1f);

		screenPercentPosition = new Vector2(0f, (-heightScreen / 2 + transform.localScale.y / 2) / (heightScreen / 2));
		transform.position = new Vector3(0f, screenPercentPosition.y * heightScreen / 2, 0f);
		maxScreenPosition = (widthScreen / 2 - transform.localScale.x / 2) / (widthScreen / 2);
	}


	void Update()
	{
		if (photonView.IsMine)
		{
			screenPercentPosition.y = (-NetworkGameManager.GetScreenToWorldHeight / 2 + transform.localScale.y / 2) /
			                          ( NetworkGameManager.GetScreenToWorldHeight / 2);
			if (Input.GetKey(KeyCode.RightArrow))
			{
				screenPercentPosition.x += speed / 10f * Time.deltaTime;
				if (screenPercentPosition.x > maxScreenPosition)
				{
					screenPercentPosition.x = maxScreenPosition;
				}
			}

			if (Input.GetKey(KeyCode.LeftArrow))
			{
				screenPercentPosition.x -= speed / 10f * Time.deltaTime;
				if (screenPercentPosition.x < -maxScreenPosition)
				{
					screenPercentPosition.x = -maxScreenPosition;
				}
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

			widthScreen = NetworkGameManager.GetScreenToWorldWidth;
			heightScreen = NetworkGameManager.GetScreenToWorldHeight;
			transform.localScale = new Vector3( widthScreen * sizePlayer.x, heightScreen * sizePlayer.y, 1f );
			transform.position = new Vector3( screenPercentPosition.x * widthScreen / 2,
				screenPercentPosition.y * NetworkGameManager.GetScreenToWorldHeight / 2, 0f );
			maxScreenPosition = ( widthScreen / 2 - transform.localScale.x / 2 ) / ( widthScreen / 2 );
		}
		else
		{
			screenPercentPosition.y = ( NetworkGameManager.GetScreenToWorldHeight / 2 -
									   NetworkGameManager.GetScreenToWorldHeight * sizePlayer.y / 2 ) /
									  ( NetworkGameManager.GetScreenToWorldHeight / 2 );

			widthScreen = NetworkGameManager.GetScreenToWorldWidth;
			heightScreen = NetworkGameManager.GetScreenToWorldHeight;
			transform.localScale = new Vector3( widthScreen * sizePlayer.x, heightScreen * sizePlayer.y, 1f );
			transform.position = new Vector3( -screenPercentPosition.x * widthScreen / 2,
				screenPercentPosition.y * NetworkGameManager.GetScreenToWorldHeight / 2, 0f );
			maxScreenPosition = ( widthScreen / 2 - transform.localScale.x / 2 ) / ( widthScreen / 2 );
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			//stream.SendNext(widthScreen);
			//stream.SendNext(heightScreen);
			stream.SendNext(screenPercentPosition);
			//stream.SendNext(maxScreenPosition);
			stream.SendNext(transform.position);
		}
		else
		{
			//widthScreen = (float) stream.ReceiveNext();
			//heightScreen = (float) stream.ReceiveNext();
			screenPercentPosition = (Vector2) stream.ReceiveNext();
			//maxScreenPosition = (float) stream.ReceiveNext();
			position = (Vector3) stream.ReceiveNext();
		}
	}
}