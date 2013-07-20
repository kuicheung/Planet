using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon{

	private Texture2D image;
	private Texture2D selectedImage;
	private float damage;
	private float radius;
	private float firingSpeed;
	private bool grenede;
	private GameObject ammo;
	private string name;
	private AudioClip sound;
	
	public Weapon(Texture2D img,Texture2D selectedImg,float dmg, float r, float sp,bool theGrenede,GameObject theAmmo,string theName,AudioClip theSound){
		image = img;
		selectedImage=selectedImg;
		damage = dmg;
		radius = r;
		firingSpeed = sp;
		grenede = theGrenede;
		ammo = theAmmo;
		name = theName;
		sound = theSound;
	}
	
	public Texture2D getImage(){
		return image;	
	}
	
	public Texture2D getSelectedImage(){
		return selectedImage;	
	}
	
	public float getDamage(){
		return damage;	
	}
	
	public float getRadius(){
		return radius;	
	}
	
	public float getFiringSpeed(){
		return firingSpeed;	
	}
	
	public bool isGrenede(){
		return grenede;
	}
	
	public GameObject getAmmo(){
		return ammo;
	}
	
	public string getName(){
		return name;
	}
	
	public AudioClip getSound(){
		return sound;	
	}
}
