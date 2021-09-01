using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

    #region Variables
    public Player shooter;
    #endregion


    #region UnityCallbacks
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }else if (collision.gameObject.CompareTag("BaseArea") || collision.gameObject.CompareTag("Player"))
        {
            if (shooter != null)
            {
                shooter.IncreaseScore();
            }
            Destroy(gameObject);
        }
    }
    #endregion

    #region CustomMethod
   

    #endregion
}
