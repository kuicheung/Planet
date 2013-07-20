using UnityEngine;
using System.Collections;

public class Weapons : MonoBehaviour {
	
	public Texture2D image;
	public Texture2D selectedImage;
	public float firingSpeed;
	public float radius;
	public float damage;
	public string weaponName;
	public bool grenede;
	public GameObject ammo;
	public AudioClip sound;
	
	
	private Weapon weapon;
	
	// Use this for initialization
	void Start () {
		weapon = new Weapon(image,selectedImage,damage,radius,firingSpeed,grenede,ammo,weaponName,sound);
	}
	
	void OnTriggerEnter(Collider theCollider){
		if(theCollider.name=="Player"){
			GameObject.Find("Player").SendMessage("AddWeapon",weapon);
			Destroy(gameObject);
		}
	}
}
