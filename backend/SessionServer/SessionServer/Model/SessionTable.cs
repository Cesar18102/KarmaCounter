using System.Linq;
using System.Collections.Generic;

using SessionServer.Dto;

namespace SessionServer.Model
{
    public class SessionTable
    {
        public Dictionary<string, List<Session>> Sessions = new Dictionary<string, List<Session>>();//private

        public bool IsSessionAlive(Session session) => Sessions.Values.Any(SL => SL.Contains(session));

        public Session Create(User user, string url)
        {
            if (!Sessions.ContainsKey(url))
                Sessions.Add(url, new List<Session>());

            Session session = new Session(user);
            Sessions[url].Add(session);
            return session;
        }

        public bool Terminate(Session session)
        {
            foreach (KeyValuePair<string, List<Session>> serverSessions in Sessions)
                if (serverSessions.Value.Contains(session))
                {
                    Sessions[serverSessions.Key].Remove(session);
                    return true;
                }

            return false;
        }
    }
}