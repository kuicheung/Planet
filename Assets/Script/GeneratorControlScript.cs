using UnityEngine;
using System.Collections;

public class GeneratorControlScript : MonoBehaviour {

	public float health=1;
	public GameObject shatter;
	
	void ChangeLife(int amountToChange){
		health += amountToChange;
		if(health<=0){
			Instantiate(shatter,transform.position,transform.rotation);
			GameObject[] shieldWalls = GameObject.FindGameObjectsWithTag("ShieldWall");
			foreach(GameObject wall in shieldWalls)
				wall.SetActive(false);
			Destroy(gameObject);
		}
	}
}
