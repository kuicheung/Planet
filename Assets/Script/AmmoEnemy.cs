using UnityEngine;
using System.Collections;

public class AmmoEnemy : MonoBehaviour {
	
	public float bulletSpeed=100f;
	public int damage=5;
	public GameObject explosion;
	public GameObject trail;
	public bool hasTrail = false;
	public bool tracking = false;
	public GameObject player;
	public bool moving = false;
	public Vector3 bulletPosition;
	public Vector3 playerPOsition;
	public Vector3 bulletRotation;
	public Vector3 bulletForward;
	
	void Start () {
		if(!tracking)
			rigidbody.AddRelativeForce(new Vector3(0f,0f,bulletSpeed));
		player = GameObject.Find("Player");
	}
	
	void Update () {
		if(hasTrail)
			Instantiate(trail,transform.position,transform.rotation);
		if(tracking){
			transform.LookAt(player.transform.position+Vector3.up*1.5f);
			transform.Translate(Vector3.forward*bulletSpeed*Time.deltaTime);			
		}
		RaycastHit hitInfo;
		if(Physics.Raycast(transform.position,transform.forward*2,out hitInfo,2f)){
			if(hitInfo.collider.tag!="AmmoIgnore"){
				if(hitInfo.collider.name=="Player"){
					Instantiate(explosion,hitInfo.point,hitInfo.collider.transform.rotation);
					Destroy(gameObject);
					hitInfo.collider.SendMessage("ChangeLife",-damage);
				}
				else{
					Instantiate(explosion,hitInfo.point,hitInfo.collider.transform.rotation);
					Destroy(gameObject);
				}
			}
		}
	}
	
	void OnCollisionEnter(Collision theCollision){
		if(theCollision.collider.tag!="AmmoIgnore"){
			if(theCollision.collider.name=="Player"){
				Instantiate(explosion,theCollision.contacts[0].point,transform.rotation);
				Destroy(gameObject);
				theCollision.collider.SendMessage("ChangeLife",-damage);
			}
			else{
				Instantiate(explosion,theCollision.contacts[0].point,transform.rotation);
				Destroy(gameObject);
			}
		}
		
	}
	
	IEnumerator MoveForward(){
		moving=true;
		
		yield return new WaitForSeconds(0.01f);	
		moving=false;
	}
}
