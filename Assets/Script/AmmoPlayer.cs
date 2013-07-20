using UnityEngine;
using System.Collections;

public class AmmoPlayer : MonoBehaviour {
	
	public float bulletSpeed=100;
	public float upForce=100;
	public float granadeRadius = 10;
	public int damage;
	public GameObject explosion;
	public GameObject trail;
	public AudioClip grenadeExplode;
	public bool hasTrail=false;
	public bool isFreeze=false;
	public bool isGrenede=false;
	public Vector3 startPos;
	
	void Start(){
		startPos = transform.position;
		
	}
	
	// Use this for initialization
	void Fire(Vector3 targetPos) {
		transform.LookAt(targetPos);
		rigidbody.AddRelativeForce(new Vector3(0,upForce,bulletSpeed));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Vector3.Distance(transform.position,startPos)>150f){
			Destroy(gameObject);	
			return;
		}
		if(hasTrail)
			Instantiate(trail,transform.position,transform.rotation);
		
		RaycastHit hitInfo;
		
		if(isGrenede){		
			Collider[] granadeHits;			
			if(Physics.Raycast(transform.position,transform.forward,out hitInfo,5f)){
				if(hitInfo.collider.tag!="AmmoIgnore"){
					Destroy(gameObject);
					Instantiate(explosion,hitInfo.point,hitInfo.collider.transform.rotation);
					AudioSource.PlayClipAtPoint(grenadeExplode,gameObject.transform.position);				
					granadeHits = Physics.OverlapSphere(hitInfo.point,granadeRadius);
					foreach(Collider hit in granadeHits)
						if(hit.collider.tag=="Enemy"){
							hit.collider.SendMessage("ChangeLife",-damage);
							if(isFreeze)
								hit.collider.SendMessage("Freeze");
						}
				}
			}
		}
		else{
			if(Physics.Raycast(transform.position,transform.forward,out hitInfo,3f)){	
				if(hitInfo.collider.tag!="AmmoIgnore"){
					Destroy(gameObject);
					if(hitInfo.collider.name!="Player"&&hitInfo.collider.tag!="Friendly")
						Instantiate(explosion,hitInfo.point,hitInfo.collider.transform.rotation);
					if(hitInfo.collider.tag=="Container")
						hitInfo.collider.SendMessage("ChangeLife",-damage);
					else if(hitInfo.collider.tag=="Enemy"){
						hitInfo.collider.SendMessage("ChangeLife",-damage);
						if(isFreeze)
							hitInfo.collider.SendMessage("Freeze");
					}
				}
			}
		}
			
	}
	
	void OnCollisionEnter(Collision theCollision){
		Destroy(gameObject);
		if(isGrenede){
			Collider[] granadeHits;
			Instantiate(explosion,theCollision.contacts[0].point,theCollision.collider.transform.rotation);
			AudioSource.PlayClipAtPoint(grenadeExplode,gameObject.transform.position);			
			granadeHits = Physics.OverlapSphere(theCollision.contacts[0].point,granadeRadius);
			foreach(Collider hit in granadeHits)
				if(hit.collider.tag=="Enemy"){
					hit.collider.SendMessage("ChangeLife",-damage);
					if(isFreeze)
						hit.collider.SendMessage("Freeze");
				}
		}
		else{
			if(theCollision.collider.name!="Player"&&theCollision.collider.tag!="Friendly")
				Instantiate(explosion,theCollision.contacts[0].point,new Quaternion(0,0,0,0));
			if(theCollision.collider.tag=="Container")
				theCollision.collider.SendMessage("ChangeLife",-damage);
			else if(theCollision.collider.tag=="Enemy"){
				theCollision.collider.SendMessage("ChangeLife",-damage);
				if(isFreeze)
					theCollision.collider.SendMessage("Freeze");
			}
		}
	}
	
}
