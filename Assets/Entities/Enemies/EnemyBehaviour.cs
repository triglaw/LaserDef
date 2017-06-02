using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
	public float health = 200f;
	public GameObject projectile;
	public float projectileSpeed = 10;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 150;
	public AudioClip fireSound;
	public AudioClip deathSound;

	private ScoreKeeper scoreKeeper;

	void Start(){
		scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log (collider);
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			health -= missile.GetDamage ();
			missile.Hit ();
			if (health <= 0) {
				Destroy (gameObject);
				AudioSource.PlayClipAtPoint (deathSound, transform.position);
				scoreKeeper.Score(scoreValue);
			}
		}
	}

	void Update(){
		float probability = Time.deltaTime * shotsPerSecond;
		if(Random.value < probability){
			Fire ();

		}
	}

	void Fire(){
		//Vector3 startPosition = transform.position + new Vector3 (0, -1, 0);
		GameObject missile = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, -projectileSpeed);
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
	}
}
