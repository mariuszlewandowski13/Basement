#region Usings

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;
using Steamworks;

#endregion

/// <summary>
/// This script automatically connects to Photon (using the settings file),
/// tries to join a random room and creates one if none was found (which is ok).
/// </summary>
public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    /// 
    #region Private Properties
    private bool AutoConnect = true;

    private byte Version = 1;

    private bool loaded = false;

    /// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
    private bool ConnectInUpdate = true;

    #endregion


    #region Methods
    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected && SaveLoadManager.isLoaded)
        {
            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
        }


        if (!loaded && ApplicationLoadScript.isLoaded)
        {
            GetComponent<SaveLoadManager>().LoadGame();
            loaded = true;
        }
    }


    // below, we implement some callbacks of PUN
    // you can find PUN's callbacks in the class PunBehaviour or in enum PhotonNetworkingMessage


    public virtual void OnConnectedToMaster()
    {
        Debug.Log("TESTTASRTAST");
        GetComponent<PhotonView>().RPC("UserJoinedNotification", PhotonTargets.Others);
        Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);
        SendAnalyticsRegionConnection();
        if (ApplicationStaticData.roomToConnectName != "")
        {
            Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinOrCreateRoom( " + ApplicationStaticData.roomToConnectName + ");");
            PhotonNetwork.JoinOrCreateRoom(ApplicationStaticData.roomToConnectName, new RoomOptions() { MaxPlayers = 0, IsVisible = true }, new TypedLobby() { });
        }
        else {
            Debug.Log("RoomName is empty. OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom( );");
            PhotonNetwork.JoinRandomRoom();
        }

    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList().");

        //Debug.Log(PhotonNetwork.lobby.Name);
        //
        // PhotonNetwork.JoinRandomRoom();

}
    

public virtual void OnReceivedRoomListUpdate()
    {
        Debug.Log("joining room");
        PhotonNetwork.JoinOrCreateRoom(ApplicationStaticData.roomToConnectName, new RoomOptions() { MaxPlayers = 0, IsVisible = true }, new TypedLobby() { });


    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }


    public void OnJoinedRoom()
    {
        
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
        if (ApplicationStaticData.roomToConnectName != ApplicationStaticData.worldRoomName)
        {
            SendRoomEnteredToDatabase();
        }
        SendAnalyticsRoomEntered();
    }

    private void SendAnalyticsRoomEntered()
    {

        Analytics.CustomEvent("RoomEntered", new Dictionary<string, object>
            {
            { "RoomID", ApplicationStaticData.roomToConnectName },
          });
    }

    private void SendRoomEnteredToDatabase()
    {
        if (GetComponent<DatabaseHandler>() != null)
        {
            GetComponent<DatabaseHandler>().UpdateUserRoomEntered(ApplicationStaticData.userID, ApplicationStaticData.roomToConnectName);
        }

    }

    private void SendAnalyticsRegionConnection()
    {
        Analytics.CustomEvent("RegionConnection", new Dictionary<string, object>
            {
            { "Region", PhotonNetwork.networkingPeer.CloudRegion },
          });
    }

    public void PrepareForReconnect()
    {
        ConnectInUpdate = true;
        loaded = false;
    }

    #endregion
}
