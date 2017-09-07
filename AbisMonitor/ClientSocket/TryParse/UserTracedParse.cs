using AbisMonitor.Domain;
using AbisMonitor.UI.ViewModels.Monitor;

namespace AbisMonitor.UI.ClientSocket.TryParse
{
    public class CatchUserParse:UI.ClientSocket.TryParse.ITryParse
    {
        public bool TryParse(byte[] buffers)
        {
            var userTrack = new UI.ClientSocket.TryParse.UserIdParse().TryParse(buffers);
            if (userTrack == null) return false;
            userTrack.State = UserTrackState.UserTraced;
            QueueManager.Instance.AddUserTrack(userTrack);
            return true;
        }
    }

    public class LoseUserParse : UI.ClientSocket.TryParse.ITryParse
    {
        public bool TryParse(byte[] buffers)
        {
            var userTrack = new UI.ClientSocket.TryParse.UserIdParse().TryParse(buffers);
            if (userTrack == null) return false;
            userTrack.State = UserTrackState.UserLose;
            QueueManager.Instance.AddUserTrack(userTrack);
            return true;
        }
    }

    public class StartUserTrack:UI.ClientSocket.TryParse.ITryParse
    {
        public bool TryParse(byte[] buffers)
        {
            var userTrack = new UI.ClientSocket.TryParse.UserIdParse().TryParse(buffers);
            if (userTrack == null) return false;
            userTrack.State = UserTrackState.Start;
            QueueManager.Instance.AddUserTrack(userTrack);
            return true;
        }
    }

    public class StopUserTrack : UI.ClientSocket.TryParse.ITryParse
    {
        public bool TryParse(byte[] buffers)
        {
            var userTrack = new UI.ClientSocket.TryParse.UserIdParse().TryParse(buffers);
            if (userTrack == null) return false;
            userTrack.State = UserTrackState.Stop;
            QueueManager.Instance.AddUserTrack(userTrack);
            return true;
        }
    }


}
