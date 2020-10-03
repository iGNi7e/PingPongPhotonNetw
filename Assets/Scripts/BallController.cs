using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour, IPunObservable
{
	private float speed = 10f;
    [SerializeField] private Color color = Color.red;

    private Vector2 direction = new Vector2(0,0);
    private Vector2 screenPercentPosition;
    private float sizeBall;
    // Start is called before the first frame update
    void Start()
    {
	    Spawn();
    }

    private void Update()
    {
	    if (PhotonNetwork.IsMasterClient)
	    {
		    transform.Translate( direction * speed * Time.deltaTime );
			screenPercentPosition = new Vector2(transform.position.x/(NetworkGameManager.GetScreenToWorldWidth /2),
				transform.position.y/(NetworkGameManager.GetScreenToWorldHeight/2));
			if ( Mathf.Abs( transform.position.x ) + transform.localScale.x / 2 > NetworkGameManager.GetScreenToWorldWidth / 2 ) {
			    direction = new Vector2( -direction.x, direction.y );
		    }
		    if ( Mathf.Abs( transform.position.y ) + transform.localScale.y / 2 > NetworkGameManager.GetScreenToWorldHeight / 2 ) {
			    direction = new Vector2( direction.x, -direction.y );
		    }
	    }
	    else
	    {
		    transform.position = new Vector3( screenPercentPosition.x * NetworkGameManager.GetScreenToWorldWidth/2,
				screenPercentPosition .y * NetworkGameManager.GetScreenToWorldHeight/2,
				0f);
		}
    }

    private void Spawn()
    {
	    sizeBall = Random.Range( 0.03f, 0.05f );
	    transform.localScale = new Vector3( NetworkGameManager.GetScreenToWorldWidth * sizeBall,
		    NetworkGameManager.GetScreenToWorldHeight * sizeBall, 1f );
        float randXpos = Random.Range( -NetworkGameManager.GetScreenToWorldWidth/2 + transform.localScale.x / 2,
	        NetworkGameManager.GetScreenToWorldWidth/2 - transform.localScale.x / 2 );
	    transform.position = new Vector3(randXpos, 0f,0f);
	    direction = new Vector2(Mathf.Pow(-1, Random.Range(1,3) ) * Random.Range(0.30f,0.70f),
		    Mathf.Pow( -1, Random.Range( 1, 3 ) ) * Random.Range(0.30f,0.70f));
        speed = Random.Range(3f, 5f);
	    color = Random.ColorHSV();
	    GetComponent<SpriteRenderer>().color = color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

	    if ( other.transform.tag != "Player" )
		    return;
	    
        if (transform.position.x > other.transform.position.x)
        {
	        float x = Mathf.Abs(Mathf.Abs( transform.position.x ) - Mathf.Abs(other.transform.position.x)) / 
	                  other.transform.localScale.x / 2;

	        direction = new Vector2( direction.x + x/2, -direction.y );
        }
        else
        {
	        float x = Mathf.Abs( Mathf.Abs( transform.position.x ) - Mathf.Abs( other.transform.position.x ) ) /
	                  other.transform.localScale.x / 2;

	        direction = new Vector2( direction.x - x/2, -direction.y );
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
	    if (stream.IsWriting)
	    {
            stream.SendNext(new Vector3(color.r,color.g,color.b));
            stream.SendNext(screenPercentPosition);
	    }
	    else
	    {
		    Vector3 temp = (Vector3)stream.ReceiveNext();
			color = new Color(temp.x,temp.y,temp.z);
		    screenPercentPosition = (Vector2) stream.ReceiveNext();
	    }
    }
}
