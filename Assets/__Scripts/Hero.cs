using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
	static public Hero S; // Singleton //
	
	[Header("Set in Inspector")]
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;
	public float gameRestartDelay = 2f;
	public GameObject projectilePrefab;
	public float projectileSpeed = 40;
	
	[Header("Set Dynamically")]
	[SerializeField]
	public float _shieldLevel = 1;
	private GameObject lastTriggerGo = null;
	
	void Awake(){
		if (S==null){
			S = this;
		}
		else{
			Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
		}
	}
	
    // Start is called before the first frame update
    void Start()
    {
        print("Print to Console!");
    }

    // Update is called once per frame
    void Update()
    {
	// Pull in information from the Input class
	float xAxis = Input.GetAxis("Horizontal"); // b
	float yAxis = Input.GetAxis("Vertical"); // b
	// Change transform.position based on the axes
	Vector3 pos = transform.position;
	pos.x += xAxis * speed * Time.deltaTime;
	pos.y += yAxis * speed * Time.deltaTime;
	transform.position = pos;
	// Rotate the ship to make it feel more dynamic // c
	transform.rotation = Quaternion.Euler(yAxis*pitchMult,xAxis*rollMult,0);
    if ( Input.GetKeyDown( KeyCode.Space ) ) { // a
	TempFire();
	}
    
    }
	
	void TempFire() { // b
	GameObject projGO = Instantiate<GameObject>( projectilePrefab );
	projGO.transform.position = transform.position;
	Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
	rigidB.velocity = Vector3.up * projectileSpeed;
	}
	
	void OnTriggerEnter(Collider other){
		print("Collision Detected!");
		Transform rootT = other.gameObject.transform.root;
		GameObject go = rootT.gameObject;
		//print("Triggered: "+go.name);
		// Make sure it's not the same triggering go as last time
		if (go == lastTriggerGo) { // c
		return;
		}
		lastTriggerGo = go; // d
		if (go.tag == "Enemy") { // If the shield was triggered by an enemy
		shieldLevel--; // Decrease the level of the shield by 1
		Destroy(go); // … and Destroy the enemy // e
		} else {
			print( "Triggered by non-Enemy: "+go.name); // f
			}
		}

		public float shieldLevel {
			get{
				return(_shieldLevel);
			}
			set{
				_shieldLevel = Mathf.Min(value,4);
				if (value < 0) {
				   //Destroy(this.gameObject);
				   Main.S.DelayedRestart(gameRestartDelay);
				}
			}
	}
			
		
}
