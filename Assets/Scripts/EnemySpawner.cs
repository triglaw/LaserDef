using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float formationSpeed = 15f;
	public float width;
	public float height;
	public float spawnDelay = 0.5f;

	private bool movingRight = true;
	private float xmax;
	private float xmin;

	// Use this for initialization
	void Start () {
		float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
		//finding edges of the world space
		Vector3 leftEdge = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distanceToCamera));
		Vector3 rightEdge = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distanceToCamera));
		xmin = leftEdge.x;
		xmax = rightEdge.x;
		//filling gizmos with enemies
		SpawnUntilFull();
		}

	//drawing gizmos in editor
	public void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector3(width, height));
	}

	void EnemySpawn(){
		foreach (Transform child in transform) {
			GameObject enemy =  Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}
	}

	void SpawnUntilFull(){
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
		}
		if(NextFreePosition()){
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

	// Update is called once per frame
	void Update () {
		//moving formation
		if (movingRight) {
			transform.position += Vector3.right * formationSpeed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * formationSpeed * Time.deltaTime;
		}
		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);
		if (leftEdgeOfFormation <= xmin) {
			movingRight = true;
		}else if(rightEdgeOfFormation >= xmax){
			movingRight = false;
		}

		if (AllMembersDead ()) {
			Debug.Log ("Empty formation");
			SpawnUntilFull();
		}
	}

	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform){
			if (childPositionGameObject.childCount == 0) {
				return childPositionGameObject;
			}
		}
		return null;
	}
		
	bool AllMembersDead(){
		foreach (Transform childPositionGameObject in transform){
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		return true;
	}
}
