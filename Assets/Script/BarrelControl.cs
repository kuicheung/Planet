using UnityEngine;
using System.Collections;

public class BarrelControl : MonoBehaviour {

	public float health=1;
	public GameObject shatter;
	public float explsionRadius=10f;
	
	void ChangeLife(int amountToChange){
		health += amountToChange;
		if(health<=0){
			Collider[] explosionHits = Physics.OverlapSphere(transform.position+Vector3.up,explsionRadius);
		 	foreach(Collider hit in explosionHits){
				//Debug.Log(hit.collider.name);
				if(hit.collider.tag=="Enemy")
					hit.collider.SendMessage("ChangeLife",-10);
			}
			Instantiate(shatter,transform.position,transform.rotation);			
			Destroy(gameObject);
			
		}
	}
}
