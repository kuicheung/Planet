using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using System.Linq;

public class FriendlyControlScript : MonoBehaviour {
	
	public float speed = 10f;
	public float turnSpeed = 1f;
	public float jumpSpeed = 50f;
	public float gravity = 10f;
	public float firingSpeed = 0.5f;
	public float iceCubeOffset = 1f;
	public float followXOffset = 4f;
	public float followZOffset = 4f;
	public bool freeze=false;
	public bool grounded = true;
	public bool firing = false;
	public bool attack = false;
	public bool updatingPath = false;
	public bool searchingPath = false;
	public bool moving = false;
	public bool switching = false;
	public bool playingSound=false;
	public bool started = false;
	public GameObject bullets;
	public GameObject iceBlock;
	public GameObject followTarget;
	public GameObject startEffects;
	public GameObject pistolModel;
	public GameObject uziModel;
	public GameObject mixGunModel;
	public GameObject grenadeLauncherModel;
	public GameObject iceGunModel;
	public Texture2D headShot;
	public GameObject player;
	public GameObject guiBar;
	public Camera mainCam;
	public int life = 10;
	public Vector3 initPos;
	public Quaternion initRotation;
	public int maxLife = 10;
	public float horizontalInput=0f;
	public float verticalInput=0f;
	public float sidewayInput=0f;
	public bool jumpInput=false;
	public bool fireInput=true;
	public bool chase=false;
	public bool following=true;
	public bool selected=false;
	public float distanceToTarget=0f;
	public float chasingDistance=3f;
	public float maxChaseDistance=80f;
	public Path path;
	public float nextWaypointDistance = 3f;
	public Vector3 targetPosition;
	public Vector3 cameraLookAtPos;
	public int weaponIndex = 0;
	
	private List<Weapon> weapons;
	private List<string> weaponsName;
	
    private int currentWaypoint = 0;
	
	private GameObject block;
	private Animator anim;
	private CharacterController control;
	private Seeker seeker;
	public float initMaxChaseDistance;
	private GameObject currentGun;
	
	void Start () {
		freeze = true;
		initPos = transform.position;
		initRotation = transform.rotation;
		initMaxChaseDistance = maxChaseDistance;
		Transform targetTransform = followTarget.transform;
		targetPosition = transform.position+transform.forward*4;//targetTransform.position-targetTransform.forward*followZOffset-targetTransform.right*followXOffset;
		anim = GetComponent<Animator>();
		control=GetComponent<CharacterController>();
		seeker = GetComponent<Seeker>();
		seeker.StartPath (transform.position,targetPosition, OnPathComplete);
		weapons = new List<Weapon>();
		weaponsName = new List<string>();
		uziModel.SetActive(false);
		mixGunModel.SetActive(false);
		grenadeLauncherModel.SetActive(false);
		iceGunModel.SetActive(false);
		currentGun=pistolModel;
	}
	
	
	void FixedUpdate () {
		startEffects.transform.position = transform.position;
		Transform playerTransform = player.collider.transform;
		float distToPlayer = Vector3.Distance(transform.position,playerTransform.position);
		if(distToPlayer<=10){			
			startEffects.SetActive(false);
			if(!started){
				player.SendMessage("GetWeapons",gameObject);
				guiBar.SendMessage("AddPlayer",headShot);
				started = true;
			}
			if(following){
				targetPosition = playerTransform.position-playerTransform.forward*followZOffset-playerTransform.right*followXOffset;
				if(!updatingPath){
							StartCoroutine("UpdatePath");
				}
				
			}
		}
		if(!freeze){			
			anim.enabled=true;
			
			if (path == null) {
	            //We have no path to move after yet
	            return;
	        }
	        
	        if(Input.GetKeyDown(KeyCode.F))
				following = true;
	        
	        //Direction to the next waypoint
	        float distToFollowTarget = Vector3.Distance(transform.position,followTarget.transform.position);
			float followDistance;
			
			if(Input.GetButton("Fire1")){
				fireInput=true;
				followDistance=100f;
			}
			else{
				fireInput=false;
				followDistance=10f;
			}
			
			if(following){
				if(distToFollowTarget>=followDistance){
					targetPosition = followTarget.transform.position-followTarget.transform.forward*followZOffset-followTarget.transform.right*followXOffset;
					if(!updatingPath)
						StartCoroutine("UpdatePath");
				}				
			}
			else{
				if(distToFollowTarget>=100f){
					following = true;
				}
			}
			
				
			distanceToTarget = Vector3.Distance(transform.position,targetPosition);
			anim.SetFloat("Speed", verticalInput);	
			
			if(distanceToTarget>=2){
				if (currentWaypoint >= path.vectorPath.Count) {
					if(!updatingPath)
	            		StartCoroutine("UpdatePath");
					
					//stay at final waypoint until path is updated
					currentWaypoint = path.vectorPath.Count-1;
	       	 	}
				Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
	        	dir *= speed * Time.deltaTime;	
				verticalInput = 1f;
				transform.LookAt(new Vector3(targetPosition.x,transform.position.y,targetPosition.z));
				dir = new Vector3(dir.x,-gravity,dir.z);
				control.Move(dir);
				attack=false;
				if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            	currentWaypoint++;
	        	}
			}
			else {
				verticalInput=0f;
				attack=true;
				if(selected){
					StartCoroutine(UnSelect(1f));
					selected=false;
				}
			}		
			
			Weapon weapon = weapons[weaponIndex];
		
			
			
			//shooting input
			if(fireInput&&attack){
				anim.SetBool("Attack",true);
				if(!firing)
					StartCoroutine(Fire(weapon.getFiringSpeed(),weapon.getSound(),weapon.getAmmo()));
			}
			else 
				anim.SetBool("Attack",false);
	        
		}//!freeze
		else
			anim.enabled=false;
	}
	
	IEnumerator Fire (float firingSpeed, AudioClip sound,GameObject ammo) {	
		RaycastHit[] hits;
		hits = Physics.RaycastAll(mainCam.transform.position,mainCam.transform.forward).OrderBy(h=>h.distance).ToArray();
		int i = 0;
		while((i<hits.Length)&&(hits[i].collider.name=="Player"||hits[i].collider.tag=="Friendly"))
			i++;
		cameraLookAtPos = hits[i].point;
					
		transform.LookAt(new Vector3(cameraLookAtPos.x,transform.rotation.y,cameraLookAtPos.z));
		Vector3 startPos = currentGun.transform.position+transform.forward*2;//transform.position+transform.up*2+transform.forward*2;
		GameObject bullet = (GameObject) Instantiate(ammo,startPos,transform.rotation);
		bullet.SendMessage("Fire",cameraLookAtPos);
		if(!playingSound)
			StartCoroutine(GunSound (sound));
		firing = true;
		yield return new WaitForSeconds(firingSpeed);
		firing = false;		
	}
	
	void ChangeLife(int amountToChange){
		maxChaseDistance=200f;
		life += amountToChange;
		if(life<=0)
			StartCoroutine("Dead");
	}
	
	IEnumerator Freeze(){
		if(!freeze){
			block = (GameObject) Instantiate(iceBlock,transform.position+new Vector3(0,iceCubeOffset,0),transform.rotation);	
			freeze=true;
			yield return new WaitForSeconds(10f);
			Destroy(block);
			freeze=false;	
		}
	}
	
	void Dead() {
		gameObject.SendMessage("Explode");
		if(block!=null)
			Destroy(block);
		Destroy(gameObject);
	}
	
	void Reset() {
		transform.position=initPos+Vector3.up;
		transform.rotation=initRotation;
		maxChaseDistance=initMaxChaseDistance;
		verticalInput=0f;
		sidewayInput=0f;
		seeker.StartPath (transform.position,transform.position+Vector3.forward*5, OnPathComplete);
	}
	
	public void OnPathComplete (Path p) {
       // Debug.Log ("Path Error"+p.error);
        if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
			searchingPath=false;
        }
    }
	
	IEnumerator UpdatePath() {
		updatingPath=true;
		if(!searchingPath){
			searchingPath=true;
			seeker.StartPath (transform.position,targetPosition, OnPathComplete);
		}
		yield return new WaitForSeconds(0.15f);
		updatingPath=false;
		
	}
	
	public void MoveTo(Vector3 moveToPosition){
		following=false;
		targetPosition = new Vector3(moveToPosition.x,transform.position.y,moveToPosition.z);
		selected=true;
		//seeker.StartPath (transform.position,targetPosition, OnPathComplete);
	}
	IEnumerator UnSelect(float timer){
		yield return new WaitForSeconds(timer);
		startEffects.SetActive(false);
	}
	
	void Select(){
		startEffects.SetActive(true);		
	}
	
	void AddWeapon(Weapon weapon){
		if(!weaponsName.Contains(weapon.getName())){
			weapons.Add(weapon);
			weaponsName.Add(weapon.getName());
		}
	}
	
	IEnumerator SwitchWeapon (int wIndex) {	
		weaponIndex=wIndex;
		if(weaponIndex>=weapons.Count)
			weaponIndex=0;
		string wepName = weapons[weaponIndex].getName();
		if(wepName.Equals("PP7")||wepName.Equals("IceGun")){
			currentGun.SetActive(false);
			currentGun=pistolModel;
			currentGun.SetActive(true);
		}
		if(wepName.Equals("IceGun")){
			currentGun.SetActive(false);
			currentGun=iceGunModel;
			currentGun.SetActive(true);
		}
		else if(wepName.Equals("UZI")){
			currentGun.SetActive(false);
			currentGun=uziModel;
			currentGun.SetActive(true);
		}
		else if(wepName.Equals("IceGrenadeLauncher")){
			currentGun.SetActive(false);
			currentGun=mixGunModel;
			currentGun.SetActive(true);
		}
		else if(wepName.Equals("GrenadeLauncher")){
			currentGun.SetActive(false);
			currentGun=grenadeLauncherModel;
			currentGun.SetActive(true);
		}
		switching = true;
		yield return new WaitForSeconds(0.1f);
		switching = false;
	}
	
	IEnumerator Granade (float firingSpeed,AudioClip sound,GameObject ammo) {
			Vector3 startPos = transform.position+transform.up*2+transform.forward*2;
			GameObject grenede = (GameObject) Instantiate(ammo,transform.position+transform.up+transform.forward*2,transform.rotation);			
			grenede.transform.LookAt(cameraLookAtPos);
			firing = true;
			AudioSource.PlayClipAtPoint(sound,gameObject.transform.position);
			yield return new WaitForSeconds(firingSpeed);
			firing = false;
	}
	
	void AddWeapons(List<Weapon> playerWeapons){
		foreach(Weapon w in playerWeapons){
			weapons.Add(w);	
		}
		freeze=false;
	}
	
	
	IEnumerator GunSound(AudioClip sound) {
		playingSound = true;
		AudioSource.PlayClipAtPoint(sound,gameObject.transform.position);
		yield return new WaitForSeconds(.3f);
		playingSound = false;
	}
}
