using UnityEngine;
using UnityEngine.Networking;

public class CenterSphereSpawner : NetworkBehaviour {

    #region Variables
    [SerializeField]
    GameObject centerSpherePrefab;
    #endregion

    #region UnityCallbacks

    public override void OnStartServer()
    {
        var centerSphere = (GameObject)Instantiate(centerSpherePrefab, new Vector3(0, 1f, 0), Quaternion.identity);
        NetworkServer.Spawn(centerSphere);
    }

    #endregion

}
