using UnityEngine;
using System.Collections;
using Pathfinding;

public class DemonControlScript : MonoBehaviour {
	
	public float speed = 10f;
	public float turnSpeed = 1f;
	public float jumpSpeed = 50f;
	public float gravity = 10f;
	public float firingSpeed = 0.5f;
	public float iceCubeOffset = 1f;
	public float howlingRadius = 20f;
	public float howlingDmg = 10f;
	public int howlingHealth = 50;
	public int meleeDamage = -10;
	public bool freeze=false;
	public bool grounded = true;
	public bool firing = false;
	public bool attack = false;
	public bool updatingPath = false;
	public bool searchingPath = false;
	public bool started = false;
	public bool howling = false;
	public bool smacking = false;
	public GameObject splatter;
	public GameObject iceBlock;
	public GameObject player;
	public GameObject guiBox;
	public GameObject howlingFlames;
	public GameObject smackingFire;
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
	public float distanceToPlayer=0f;
	public float chasingDistance=3f;
	public float maxChaseDistance=80f;
	public Path path;
	public float nextWaypointDistance = 3f;
	public Vector3 targetPosition;
	public float timeToFire = 20f;
	public float startFireTime;
	
    private int currentWaypoint = 0;
	
	private GameObject block;
	private GameObject flame;
	private Animator anim;
	private CharacterController control;
	private Seeker seeker;
	public float initMaxChaseDistance;

	private AnimatorStateInfo currentBaseState;
	private AnimatorStateInfo layer2CurrentState;
	
	static int swapeState = Animator.StringToHash("Base Layer.attack");	
	
	void Start () {
		initPos = transform.position;
		initRotation = transform.rotation;
		initMaxChaseDistance = maxChaseDistance;
		targetPosition = player.transform.position;
		anim = GetComponent<Animator>();
		control=GetComponent<CharacterController>();
		seeker = GetComponent<Seeker>();
		seeker.StartPath (transform.position,transform.position+Vector3.forward*5, OnPathComplete);
		if (anim.layerCount >= 2)
			anim.SetLayerWeight(1, 1);
		startFireTime = Time.time;
	}
	
	
	void FixedUpdate () {
				
			anim.enabled=true;
			targetPosition = player.transform.position+Vector3.up;
			
			if (path == null) {
	            //We have no path to move after yet
	            return;
	        }
	        
	        
	        
	        //Direction to the next waypoint
	        	
			
			distanceToPlayer = Vector3.Distance(transform.position,targetPosition);
			anim.SetFloat("Speed", verticalInput);	
			currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation		
			if (anim.layerCount == 2)
				layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);
			
			if(howlingHealth<=0){
				if(!howling){
					howling=true;
					life-=1;
					anim.SetBool("Howl",true);
					anim.SetBool("Attack",false);
					StartCoroutine("Howl");
					howlingHealth = 50;
				}
				
			}
		
			if(!started)
				startFireTime=Time.time;
		
			if(!howling){	
				if(Time.time-startFireTime<timeToFire){							
					if(distanceToPlayer>=chasingDistance&&distanceToPlayer<=maxChaseDistance){
						if(!started){
							guiBox.SendMessage("StartBossFight");
							started = true;
						}
						
						
						if(!updatingPath)
							StartCoroutine("UpdatePath");
						if (currentWaypoint >= path.vectorPath.Count) {
							if(!updatingPath)
				            	StartCoroutine("UpdatePath");
							currentWaypoint = path.vectorPath.Count-1;
				       	 }
						Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
				        dir *= speed * Time.deltaTime;	
						verticalInput = 1f;
						transform.LookAt(targetPosition+Vector3.down);
						dir = new Vector3(dir.x,-gravity,dir.z);
						control.Move(dir);
						if(!firing)
							attack=false;
							
						if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
				            currentWaypoint++;
				        }
						
					}
					else if(distanceToPlayer<chasingDistance){
						verticalInput=0f;
						maxChaseDistance=100f;
						transform.LookAt(targetPosition+Vector3.down);
						attack=true;
					}
					else{
						verticalInput = 0f;
						attack=false;
					}
					
					//move character			
					
					
					if(fireInput&&!firing&&attack){
						StartCoroutine(Fire());
						anim.SetBool("Attack",true);
					}
					
					if(attack)			
						anim.SetBool("Attack",true);
					else 
						anim.SetBool("Attack",false);
				}
				else{
					if(started){
						verticalInput=0f;
						anim.SetBool("Shoot",true);
						anim.SetBool("Attack",false);
						transform.LookAt(new Vector3(player.transform.position.x,
										transform.position.y,player.transform.position.z));
						if(!smacking)
							StartCoroutine("Smack");
					}
				}
			}
	        
	}
	
	IEnumerator Fire () {						
		firing = true;
		yield return new WaitForSeconds(firingSpeed);
		Instantiate(splatter,player.transform.position+Vector3.up*2,transform.rotation);
		player.SendMessage("ChangeLife",meleeDamage);
		yield return new WaitForSeconds(firingSpeed);
		firing = false;
	}
	
	void ChangeLife(int amountToChange){
		maxChaseDistance=500f;
		life += amountToChange;
		howlingHealth += amountToChange;
		guiBox.SendMessage("SetBossLife",life/500f);
		if(life<=0)
			StartCoroutine("Dead");
	}
	
	void Freeze(){
		return;
	}
	
	void Dead() {
		gameObject.SendMessage("Explode");
		if(block!=null)
			Destroy(block);	
		if(flame!=null)
			Destroy(flame);
		guiBox.SendMessage("StartReactor");
		GameObject.Find("Reactor").SendMessage("StartReactor");
		Destroy(gameObject);
	}
	
	void Reset() {
		transform.position=initPos+Vector3.up;
		transform.rotation=initRotation;
		maxChaseDistance=initMaxChaseDistance;
		verticalInput=0f;
		sidewayInput=0f;
		life=maxLife;
		started=false;
		guiBox.SendMessage("SetBossLife",life/500f);
		if(flame!=null)
			Destroy(flame);
		anim.SetBool("Howl",false);
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
		yield return new WaitForSeconds(0.5f);
		updatingPath=false;
	}
	
	IEnumerator Howl() {		
		flame = (GameObject)Instantiate(howlingFlames,transform.position,transform.rotation);
		yield return new WaitForSeconds(2.7f);
		for(int i=0;i<5;i++){
			if(distanceToPlayer<howlingRadius)
				player.SendMessage("ChangeLife",-howlingDmg);
			yield return new WaitForSeconds(1f);	
		}
		howling=false;
		anim.SetBool("Howl",false);
		if(flame!=null)
			Destroy(flame);
	}
	
	IEnumerator Smack(){
		smacking=true;
		for(int ct=0;ct<5;ct++){
			for(int i=0;i<40;i++){
				Instantiate(smackingFire,transform.position+transform.forward.normalized*i*2,transform.rotation);
			}
			yield return new WaitForSeconds(1f);
		}
		smacking=false;
		anim.SetBool("Shoot",false);
		startFireTime=Time.time;
	}
	
}
