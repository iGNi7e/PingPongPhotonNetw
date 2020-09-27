using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasController : MonoBehaviour
{
	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private Color color = Color.blue;

	private RectTransform rectTransform;
	private Vector2 camSize;
	private Vector2 canvasSize;
	private PhotonView photonView;

	void Start() {
		photonView = GetComponent<PhotonView>();
		rectTransform = GetComponent<RectTransform>();
		GetComponent<Image>().color = color;

		camSize = new Vector2( Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize );
		canvasSize = FindObjectOfType<Canvas>().GetComponent<CanvasScaler>().referenceResolution;
		rectTransform.anchoredPosition = new Vector3( 0f, rectTransform.sizeDelta.y / 2, 0f );
		//Debug.Log( rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x / 2 );
		//Debug.Log( canvasSize.x );
	}

	void Update() {
		//if ( !photonView.IsMine )
		//	return;

		if ( Input.GetKey( KeyCode.RightArrow ) ) {
			if ( rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x / 2 + ( speed * 100f * Time.deltaTime ) > canvasSize.x/2 ) {
				rectTransform.anchoredPosition = new Vector2( (canvasSize.x / 2) - rectTransform.sizeDelta.x / 2, rectTransform.anchoredPosition.y );
				return;
			}
			rectTransform.anchoredPosition += Vector2.right * ( speed * 100f * Time.deltaTime );
		}
		if ( Input.GetKey( KeyCode.LeftArrow ) ) {
			if ( rectTransform.anchoredPosition.x - rectTransform.sizeDelta.x / 2 - ( speed * 100f * Time.deltaTime ) < -canvasSize.x/2 ) {
				rectTransform.anchoredPosition = new Vector2( ( -canvasSize.x / 2 ) + rectTransform.sizeDelta.x / 2, rectTransform.anchoredPosition.y );
				return;
			}
			rectTransform.anchoredPosition += Vector2.left * ( speed * 100f * Time.deltaTime );
		}

		return;

		if ( Input.touchCount != 1 )
			return;
		//if ( Input.touches[0].phase == TouchPhase.Began )
		//{
		//	Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		//	if (ray.origin.x < transform.position.x + transform.localScale.x / 2 ||
		//	    ray.origin.x > transform.position.x - transform.localScale.x / 2)
		//	{
		//		checkFingerInput = true;
		//	}
		//}
		if ( Input.touches[0].phase == TouchPhase.Moved ) {
			if ( transform.position.x + transform.localScale.x / 2 + Input.touches[0].deltaPosition.x > camSize.x ||
				 transform.position.x - transform.localScale.x / 2 - Input.touches[0].deltaPosition.x < -camSize.x ) {
				return;
			}
			transform.position += new Vector3( Input.touches[0].deltaPosition.x * ( speed * Time.deltaTime ), 0f );
		}

	}
}
