using UnityEngine;
using Photon.Pun;

public class ShootGun : Gun
{
    [SerializeField] private Camera myCamera;
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        gunAnimator = GetComponentInChildren<Animator>();
    }

    [PunRPC]
    private void RPC_SHOOT(Vector3 hitpoint, Vector3 hitnormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitpoint, 0.1f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpact = Instantiate(bulletPrefab, hitpoint,
                Quaternion.LookRotation(hitnormal, Vector3.up) * bulletPrefab.transform.rotation);
            bulletImpact.transform.SetParent(colliders[0].transform);
        }
    }
    
    public override void Use()
    {
        Shoot();
    }

    private void Shoot()
    {
        Ray ray = myCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = myCamera.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            _photonView.RPC(nameof(RPC_SHOOT), RpcTarget.All, hit.point, hit.normal);
        }
    }
    
}
