using Photon.Pun;

public class MatchMaking : MonoBehaviourPunCallbacks
{
    public void JoinRandom()
    {
        // Connect to a random Room
        // Do the remining in the OnJoinRoom Callback.
        // Also handle the fail.
    }
    public void Room()
    {

    }

    public override void OnJoinedRoom()
    {
        
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {

    }
}
