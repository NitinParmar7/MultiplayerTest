using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class Player : NetworkBehaviour {

    #region Variables

    [SerializeField]
    ToggleEvents OnSharedToggle;

    [SerializeField]
    ToggleEvents OnRemoteToggle;

    [SerializeField]
    ToggleEvents OnLocalToggle;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform bulletSpawnPoint;

    [SerializeField]
    BoxCollider baseArea;

    [SerializeField]
    public float bulletSpeed;

    private GameObject mainCamera;
    private Color playerColor;
    private string playerName;
    private int playerSlot;
    private float chargePower = 10;

    [SyncVar(hook = "OnScoreChanged")]
    public int currentScore;

    private Vector3 startPos;
    private Vector3 endPos;

    #endregion

    #region UnityCallbacks

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        EnablePlayer();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;


        if (Input.GetMouseButtonUp(0))
        {
            CmdFireBullet(Camera.main.transform.forward);
        }
    }

    #endregion

    #region CustomMethods
    private void EnablePlayer()
    {
        if (isLocalPlayer)
            mainCamera.SetActive(false);

        OnSharedToggle.Invoke(true);

        if (isLocalPlayer)
        {
            OnLocalToggle.Invoke(true);
        }else
        {
            OnRemoteToggle.Invoke(true);
        }
    }

    private void DisablePlayer()
    {
        if (isLocalPlayer)
            mainCamera.SetActive(true);

        OnSharedToggle.Invoke(false);

        if (isLocalPlayer)
        {
            OnLocalToggle.Invoke(false);
        }
        else
        {
            OnRemoteToggle.Invoke(false);
        }
    }

    public void SetUp(Color color, string name, int playerSlot)
    {
        this.playerColor = color;
        GetComponent<MeshRenderer>().material.color = this.playerColor;
        this.name = name;
        this.playerSlot = playerSlot;
    }

    [Command]
    private void CmdFireBullet(Vector3 direction)
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<MeshRenderer>().material.color = playerColor;
        var bulletCollider = bullet.GetComponent<SphereCollider>();
        Physics.IgnoreCollision(bulletCollider, GetComponent<CapsuleCollider>(), true);
        Physics.IgnoreCollision(bulletCollider, baseArea, true);
        Physics.IgnoreCollision(bulletCollider, GameManager.Instance.GetCenterSphereCollider(), true);
        bullet.GetComponent<Bullet>().shooter = this;
        var bulletBody = bullet.GetComponent<Rigidbody>();
        if(bulletBody != null)
        {
            Debug.Log(direction);
            direction = Quaternion.AngleAxis(-Camera.main.transform.eulerAngles.x, Vector3.forward) * direction;
            bulletBody.AddForce(direction.normalized * 40, ForceMode.Impulse);
        }
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 10);
    }


    public void IncreaseScore()
    {
        if (!isServer)
            return;

        currentScore++;
    }

    public void OnScoreChanged(int value)
    {
        if(value >= GameManager.Instance.GetMaxScore() && GameManager.Instance.GetMaxScore() >= 0)
        {
            RpcChangeCenterSphereColor();
        }
    }

    [ClientRpc]
    void RpcChangeCenterSphereColor()
    {
        GameManager.Instance.ChangeSphereColor(playerColor);
    }


    #endregion
}


[System.Serializable]
public class ToggleEvents : UnityEvent<bool>
{

}
