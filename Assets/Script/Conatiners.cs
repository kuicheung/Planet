using UnityEngine;
using System.Collections;

public class Conatiners : MonoBehaviour {

	public float health=1;
	public GameObject shatter;
	
	void ChangeLife(int amountToChange){
		health += amountToChange;
		if(health<=0){
			Instantiate(shatter,transform.position,transform.rotation);
			GameObject item;
			if((item=ItemsArray.getRandomItem())!=null)
				Instantiate(item,transform.position,transform.rotation);
			Destroy(gameObject);
		}
	}
}
