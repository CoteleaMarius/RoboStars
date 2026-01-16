using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private float walkSpeed, sprintSpeed, jumpForce, mouseSensitivity, smoothTime;
    private float _verticalLookRotation;
    private bool _isGrounded;
    private Vector3 _smoothMove;
    private Vector3 _moveAmount;
    private Rigidbody _rigidbody;
    private PhotonView _photonView;

    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private Item[] items;
    private int _itemIndex;
    private int _previousItemIndex = -1;

    private float _maxHealth = 100f;
    private float _currentHealth;
    private PlayerManager _playerManager;
    
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _rigidbody = GetComponent<Rigidbody>();
        _currentHealth = _maxHealth;
        _playerManager = PhotonView.Find((int)_photonView.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            Destroy(playerCamera);
            Destroy(_rigidbody);
        }
        else
        {
            EquipItem(0);
        }
    }

    private void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        Look();
        Movement();
        Jump();
        SelectWeapon();
        UseItem();
    }

    private void UseItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            items[_itemIndex].Use();
        }
    }

    public void TakeDamage(float damage)
    {
        _photonView.RPC("RPC_Damage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_Damage(float damage)
    {
        if (!_photonView.IsMine) return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _playerManager.Die();
        }
    }
    
    private void SelectWeapon()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i+1).ToString()))
            {
                EquipItem(i);
                break;
            }
        }
    }

    private void EquipItem(int index)
    {
        if (index == _previousItemIndex) return;
        _itemIndex = index;
        items[_itemIndex].itemGameObject.SetActive(true);
        if (_previousItemIndex != -1)
        {
            items[_previousItemIndex].itemGameObject.SetActive(false);
        }

        _previousItemIndex = _itemIndex;

        if (_photonView.IsMine)
        {
            HashTable hash = new HashTable();
            hash.Add("index", _itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!_photonView.IsMine && targetPlayer == _photonView.Owner)
        {
            EquipItem((int)changedProps["index"]);
        }
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * (Input.GetAxisRaw("Mouse X") * mouseSensitivity));
        _verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -80f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void GroundState(bool isGrounded)
    {
        _isGrounded = isGrounded;
    }
    
    private void Movement()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        _moveAmount = Vector3.SmoothDamp(_moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? 
            sprintSpeed : walkSpeed), ref _smoothMove, smoothTime);
    }

    private void FixedUpdate()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        _rigidbody.MovePosition(_rigidbody.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
    }
    
}
