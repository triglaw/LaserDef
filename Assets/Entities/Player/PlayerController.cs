using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject Projectile;
	public float speed = 0.1f;
	public float padding = 1f;
	public float projectileSpeed;
	public float firingRate;
	public float health = 250f;
	public AudioClip fireSound;

	float xmin;
	float xmax;

	void Start(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));

		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}

	void Fire(){
		Vector3 startPosition = transform.position + new Vector3 (0, 1, 0);
		GameObject beam = Instantiate (Projectile, startPosition, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
		AudioSource.PlayClipAtPoint(fireSound, transform.position, 1f);
	}
		
	void Update(){

		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
			transform.position += Vector3.left * speed * Time.deltaTime;
			}

		else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){			
			transform.position += Vector3.right * speed * Time.deltaTime;
			}
		//press space to shoot laser
		if(Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating ("Fire", 0.00000000001f, firingRate);
		}else if(Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke();
		}

		// restrict the player to the gamespace
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			Debug.Log ("Player collided with a missile");
			health -= missile.GetDamage ();
			missile.Hit ();
			if (health <= 0) {
				Destroy (gameObject);
			}
		}
	}
}