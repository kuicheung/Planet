using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemsArray : MonoBehaviour {

	private static List<GameObject> items;
	public static float rand;
	public GameObject health;
	public GameObject uzi;
	public GameObject grenedeLauncher;
	public GameObject iceGrenedeLauncher;
	public GameObject iceGun;
	
	void Start () {
		items = new List<GameObject>();
		items.Add(health);
		items.Add(uzi);
		items.Add(grenedeLauncher);
		items.Add(iceGun);
		items.Add(iceGrenedeLauncher);
	}
	
	public static GameObject getRandomItem(){
		float lgPrime = 1299827f;
		rand = Random.value;
		int index = Mathf.RoundToInt(rand*lgPrime)%1000;
		if(index>300)
			return null;
		else if(index >50)
			index = 0;
		else if(index > 25)
			index = 1;
		else if(index > 10)
			index = 1;
		else if(index > 5)
			index = 1;
		else
			index = 2;
		return items[index];
	}
}
