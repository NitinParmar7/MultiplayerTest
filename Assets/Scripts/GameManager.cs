using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    #region Variables
    public static GameManager Instance;

    public static List<Player> playerList = new List<Player>();

    private GameObject centerSphere;
    #endregion

    #region UnityCallbacks
    private void Awake()
    {
        Instance = this;
    }

    
    #endregion


    #region CustomMethods
    static public void SetUp(Player player, Color color, string name, int slot)
    {
        player.SetUp(color, name, slot);
        playerList.Add(player);
    }

    public SphereCollider GetCenterSphereCollider()
    {
        if(centerSphere == null)
        {
            centerSphere = GameObject.FindGameObjectWithTag("CenterSphere");
        }

        return centerSphere.GetComponent<SphereCollider>();
    }

    public int GetMaxScore()
    {
        if(playerList.Count == 1)
        {
            return 0;
        }else if(playerList.Count == 2)
        {
            return (Mathf.Max(playerList[0].currentScore, playerList[1].currentScore));
        }else if(playerList.Count == 3)
        {
            return (Mathf.Max(playerList[0].currentScore, playerList[1].currentScore, playerList[2].currentScore));
        }else if(playerList.Count == 4)
        {
            return (Mathf.Max(playerList[0].currentScore, playerList[1].currentScore, playerList[2].currentScore, playerList[3].currentScore));
        }
        else
        {
            return -1;
        }
    }

    public void ChangeSphereColor(Color targetColor)
    {
        StartCoroutine(ChangeSphereColorRoutine(targetColor));
    }

    #endregion

    #region Coroutines
    public IEnumerator ChangeSphereColorRoutine(Color targetColor)
    {
        float t = 0;
        MeshRenderer renderer = GameObject.FindGameObjectWithTag("CenterSphere").GetComponent<MeshRenderer>() ;
        Color startColor = renderer.material.color;
        while (t < 0)
        {
            renderer.material.color = Color.Lerp(startColor, targetColor, t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

}
