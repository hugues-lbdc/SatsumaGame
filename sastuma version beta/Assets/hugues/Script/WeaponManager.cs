using UnityEngine;
using Photon.Pun;

public class WeaponManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    public WeaponData primaryWeapon;

    private WeaponData currentWeapon;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public WeaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    void EquipWeapon(WeaponData _weapon)
    {
        currentWeapon = _weapon;

        GameObject pistolet = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
        pistolet.transform.SetParent(weaponHolder);
        
        currentGraphics = pistolet.GetComponent<WeaponGraphics>();

        if (currentGraphics == null)
            Debug.LogError("pas de script weapon graphics sur l'arme" + pistolet.name);
        
        //change
        if(photonView.IsMine)
        {
            SetLayerRecursively(pistolet, LayerMask.NameToLayer(weaponLayerName));
        }
    }
    
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}