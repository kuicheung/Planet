using UnityEngine;
using System.Collections;

public class dyingExplosion : MonoBehaviour {

	public GameObject explosion;
	
	public void Explode(){
		Instantiate(explosion,transform.position,transform.rotation);
	}
}
