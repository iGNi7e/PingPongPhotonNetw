using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
	[SerializeField] private float speed = 10f;
    [SerializeField] private Color color = Color.red;

    private Vector2 direction = new Vector2(0,0);
    private Vector2 camSize;
    // Start is called before the first frame update
    void Start()
    {
        camSize = new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize);
	    Spawn();
    }

    private void FixedUpdate()
    {
        transform.Translate( direction * speed );
	    if (Mathf.Abs(transform.position.x) + transform.localScale.x / 2 > NetworkGameManager.GetScreenToWorldWidth /2)
	    {
		    direction = new Vector2( -direction.x, direction.y );
        }
	    if ( Mathf.Abs( transform.position.y ) + transform.localScale.y / 2 > NetworkGameManager.GetScreenToWorldHeight / 2 ) {
		    Spawn();
        }
    }

    private void Spawn()
    {
	    float sizeBall = Random.Range( 0.03f, 0.05f );
	    float width = NetworkGameManager.GetScreenToWorldWidth;
	    transform.localScale = new Vector3( width * sizeBall, width * sizeBall, 1f );
        float randXpos = Random.Range( -camSize.x + transform.localScale.x / 2, camSize.x - transform.localScale.x / 2 );
	    transform.position = new Vector3(randXpos, 0f,0f);
	    direction = new Vector2(Mathf.Pow(-1, Random.Range(1,3) ) * Random.Range(0.30f,0.70f),
		    Mathf.Pow( -1, Random.Range( 1, 3 ) ) * Random.Range(0.30f,0.70f));
        speed = Random.Range(0.2f, 0.5f);
	    color = Random.ColorHSV();
	    GetComponent<SpriteRenderer>().color = color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

	    if ( other.transform.tag != "Player" )
		    return;
	    float dirX = other.gameObject.transform.localScale.x / 2;
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
}
