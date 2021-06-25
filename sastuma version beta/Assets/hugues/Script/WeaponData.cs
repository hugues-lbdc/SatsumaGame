using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "My Game/Weapon Data")]

//Création de la classe Weapon comptenent un nom, un nombre de points de déga, et une portée
public class WeaponData : ScriptableObject
{
    //public string name = "Submachine Gun";
    public float damage;
    public float range;
    public float timeForShoot;
    public int balle;

    public GameObject graphics;
}
