using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Linq;

public class EnemyFollowControlScript : MonoBehaviour {
	
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
	public bool searchingPath=false;
	public bool moving = false;
	public GameObject bullets;
	public GameObject iceBlock;
	public GameObject followTarget;
	public GameObject player;
	public GameObject ragdoll;
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
	public float distanceToTarget=0f;
	public float chasingDistance=3f;
	public float maxChaseDistance=80f;
	public Path path;
	public float nextWaypointDistance = 3f;
	public Vector3 targetPosition;
	public Vector3 cameraLookAtPos;
	
    private int currentWaypoint = 0;
	
	private GameObject block;
	private Animator anim;
	private CharacterController control;
	private Seeker seeker;
	public float initMaxChaseDistance;
	
	void Start () {
		initPos = transform.position;
		initRotation = transform.rotation;
		initMaxChaseDistance = maxChaseDistance;
		Transform targetTransform = followTarget.transform;
		targetPosition = targetTransform.position-targetTransform.forward*followZOffset-targetTransform.right*followXOffset;
		anim = GetComponent<Animator>();
		control=GetComponent<CharacterController>();
		seeker = GetComponent<Seeker>();
		StartCoroutine("UpdatePath");
	}
	
	
	void FixedUpdate () {
		
		if(!freeze){			
			anim.enabled=true;
			
			if (path == null) {
	            //We have no path to move after yet
	            return;
	        }
			
	        Vector3 playerPos = player.transform.position;
			float distanceToPlayer = Vector3.Distance(transform.position,playerPos);
			anim.SetFloat("Speed", verticalInput);
			
			if(following&&followTarget!=null){
				float distToFollowTarget = Vector3.Distance(transform.position,followTarget.transform.position);
				float followDistance=20f;							
				distanceToTarget = Vector3.Distance(transform.position,targetPosition);
				targetPosition = followTarget.transform.position-followTarget.transform.forward*followZOffset-followTarget.transform.right*followXOffset;
				
				if(!updatingPath)
					StartCoroutine("UpdatePath");
				if (currentWaypoint >= path.vectorPath.Count) {
					if(!updatingPath)
	            		StartCoroutine("UpdatePath");
					currentWaypoint = path.vectorPath.Count-1;
	       	 	}
				Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
	        	dir *= speed * Time.deltaTime;	
				verticalInput = 0.2f;
				transform.LookAt(targetPosition+Vector3.down);
				dir = new Vector3(dir.x,-gravity,dir.z);
				control.Move(dir);
				attack=false;
				if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
	            	currentWaypoint++;
	        	}
				if(distanceToPlayer<=maxChaseDistance)
					following=false;
			}
			else{
				targetPosition=playerPos;
				if(distanceToPlayer>=chasingDistance&&distanceToPlayer<=maxChaseDistance){
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
			
			//Check if we are close enough to the next waypoint
	        //If we are, proceed to follow the next waypoint
	        
		}//!freeze
		else
			anim.enabled=false;
	}
	
	
	IEnumerator Fire () {						
		firing = true;
		yield return new WaitForSeconds(firingSpeed);
		Instantiate(bullets,transform.position+Vector3.up*2+transform.forward*1.5f,transform.rotation);
		yield return new WaitForSeconds(firingSpeed);
		firing = false;
	}
	
	void ChangeLife(int amountToChange){
		maxChaseDistance=500f;
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
		Instantiate(ragdoll,transform.position,transform.rotation);
		Destroy(gameObject);
	}
	
	void Reset() {
		transform.position=initPos+Vector3.up;
		transform.rotation=initRotation;
		maxChaseDistance=initMaxChaseDistance;
		verticalInput=0f;
		sidewayInput=0f;
		if(followTarget!=null)
			following = true;
		seeker.StartPath (transform.position,transform.position+Vector3.forward*5, OnPathComplete);
	}
	
	public void OnPathComplete (Path p) {
        //Debug.Log ("Path Error"+p.error);
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
		yield return new WaitForSeconds(1f);
		updatingPath=false;
	}
	
	
}
