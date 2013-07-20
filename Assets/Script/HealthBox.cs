using UnityEngine;
using System.Collections;

public class HealthBox : MonoBehaviour {
	
	public float healthAmount = 10f;
	
	void OnTriggerEnter(Collider theCollider){
		if(theCollider.name=="Player"){
			GameObject.Find("Player").SendMessage("ChangeLife",healthAmount);
			Destroy(gameObject);
		}
	}
}
