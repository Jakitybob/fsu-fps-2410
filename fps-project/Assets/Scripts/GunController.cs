using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] float FireRate;
    [SerializeField] int MagSize;
    [SerializeField] int ReserveAmmoCap;
    [SerializeField] int damage;

    bool IsShoot;
    int CurrentAmmo;
    int AmmoReserved;
    [SerializeField] bool IsAuto;

    public Image muzzleFlash;
    public Sprite[] Flashes;

    public Vector3 localPos;
    public Vector3 AimPos; 
    [SerializeField] float aimSmooth;

    [Header("Mouse Settings")]
    [SerializeField]float mouseSens;
    Vector2 curr_Rot;
    public float gunSway;
    [Header("Recoil Settings")]
    public bool RandRecoil;
    public Vector2 Recoillimit;
    public Vector2 []recoilPatt;



    public void Start()
    {
        CurrentAmmo = MagSize;
        AmmoReserved = ReserveAmmoCap;
        IsShoot = true;
    }
    private void Update()
    {
        ADS();
        if (Input.GetKeyUp(KeyCode.B))
        {
            IsAuto = !IsAuto;

        }
        if (IsAuto)
        {
            ShootingAuto(); 
        }else
        {
            ShootingSingle(); 
        }
        if (Input.GetMouseButtonDown(0) && IsShoot && CurrentAmmo > 0)
        {
            IsShoot = false;
            CurrentAmmo--;
            StartCoroutine(Shooting());
        }
        // if "R" & ammo in Mag is less then MagSize and the ammo were holding is greater than 0 add ammoneed to the magazine and take it from reserve
        else if (Input.GetKeyDown(KeyCode.R) && CurrentAmmo < MagSize && AmmoReserved > 0)
        {
            int ammoneed = MagSize - CurrentAmmo;
            if (ammoneed >= AmmoReserved)
            {
                CurrentAmmo += AmmoReserved;
                AmmoReserved -= ammoneed;
            }
            else
            {

                CurrentAmmo = MagSize;
                AmmoReserved -= ammoneed;
            }

        }
    } 
    //when RMB is clicked  it sets the guns local position to the AimPos and transitions smoothly to wantedPos  
    void ADS()
    {
        Vector3 target = localPos;
        if (Input.GetMouseButton(1)) target = AimPos;
        Vector3 wantedPos = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmooth);

        transform.localPosition = wantedPos;

        
    }
    // moves weapon slightly on z axis either randomly based on limitations or with a set pattern emulating recoil 
    void MakeRecoil()
    {
        transform.localPosition -= Vector3.forward * 0.1f;
        if (RandRecoil)
        {
            float xRecoil = Random.Range(-Recoillimit.x, Recoillimit.x);
            float yRecoil = Random.Range(-Recoillimit.y, Recoillimit.y);

            Vector2 recoil = new Vector2(xRecoil, yRecoil);
            curr_Rot += recoil;
        }
        else
        {
            int CurrRec = MagSize + 1 - CurrentAmmo;
            CurrRec = Mathf.Clamp(CurrRec,0,recoilPatt.Length -1);
            curr_Rot += recoilPatt[CurrRec];
        }
    }
    // if LMB held down weapon is automatic 
    void ShootingAuto()
    {
        if(Input.GetMouseButton(0)&& IsShoot && CurrentAmmo > 0)
        {
            IsShoot = false;
            CurrentAmmo--;
            StartCoroutine(Shooting());
        }

    }
    //if taped single fire. Can be toggled with "B" key or hard set by changing firerate
    void ShootingSingle()
    {
        if (Input.GetMouseButton(0) && IsShoot && CurrentAmmo > 0)
        {
            IsShoot = false;
            CurrentAmmo --;
            StartCoroutine(Shooting());
        }
    }
    IEnumerator Shooting()
    {
        MakeRecoil();
        StartCoroutine(MuzzleFlashes());
        enemyRaycast();
        yield return new WaitForSeconds(FireRate);
        IsShoot =true;
        
    }
    //displays sprites from Flashes array  for set time
    IEnumerator MuzzleFlashes()
    { 
        muzzleFlash.sprite = Flashes[Random.Range(0, Flashes.Length)];
        muzzleFlash.color= Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.sprite = null;
        muzzleFlash.color = new Color(0, 0, 0, 0);

    }
    //shoot ray if ray hit and target derives from IDamge apply damage if Rigid Simulate impact
    void enemyRaycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.parent.position,transform.parent.forward, out hit, 1 << LayerMask.NameToLayer("Enemy")))
        {
           IDamage enemy = hit.transform.GetComponent<IDamage>();
            if(enemy!= null)
            {
                enemy.takeDamage(damage);
            }
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints =RigidbodyConstraints.None;
                rb.AddForce(transform.parent.transform.forward * 500);
            }
        }
    }
    public void AddAmmo(int amount)
    {
         AmmoReserved += amount;
        
        if (AmmoReserved > ReserveAmmoCap) 
        {
            AmmoReserved = ReserveAmmoCap;
    
        }
    }
}   
