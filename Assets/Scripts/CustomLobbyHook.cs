using UnityEngine;
using UnityEngine.Networking;

public class CustomLobbyHook : Prototype.NetworkLobby.LobbyHook
{
#region Unitycallbacks
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);

        if (lobbyPlayer == null)
            return;

        Prototype.NetworkLobby.LobbyPlayer lp = lobbyPlayer.GetComponent<Prototype.NetworkLobby.LobbyPlayer>();

        if(lp != null)
        {
            Player player = gamePlayer.GetComponent<Player>();
            if(player != null)
            {
                GameManager.SetUp(player, lp.playerColor, lp.playerName, lp.slot);
            }
        }
    }
#endregion
}
