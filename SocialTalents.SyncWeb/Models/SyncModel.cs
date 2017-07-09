using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SocialTalents.SyncWeb.Models
{
    public class SyncModel
    {
        public SyncModel(string syncId, int total)
        {
            AgentsTiming = new ConcurrentDictionary<string, int?>();
            Created = DateTime.Now;
            DeleteAt = Created.AddMinutes(60);
            SyncId = syncId;
            Total = total;
            Timeout = 3 * 60;
        }

        /// <summary>
        /// Id of this sync
        /// </summary>
        public string SyncId { get; set; }
        /// <summary>
        /// Total number of agents we expect
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// Time, when we can delete this model, defaults to +60 minutes
        /// </summary>
        public DateTime DeleteAt { get; set; }
        /// <summary>
        /// Time when this model was created
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Time when first agent reported
        /// </summary>
        public DateTime? Started { get; set; }
        /// <summary>
        /// Timeout for agents, seconds. Defaults to 3 minutes
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Delay fo each agent from first one
        /// </summary>
        public ConcurrentDictionary<string, int?> AgentsTiming { get; private set; }

        /// <summary>
        /// Holds execution till required Total number of agents reports
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>Number of agents discovered</returns>
        public int SyncAgent(string agentId)
        {
            bool isThisFirstAgentEntry = AddAgent(agentId);
            while (Started == null && AgentsTiming.Count < Total && DateTime.Now.Subtract(Created).TotalSeconds < Timeout)
            {
                Task.Delay(50).Wait();
            }
            StartAgent(agentId);

            return AgentsTiming.Count;
        }

        /// <summary>
        /// Returns whenever number of agents discovered changes
        /// </summary>
        /// <returns></returns>
        public int ExitOnChange()
        {
            int startCount = AgentsTiming.Count;
            int newCount = startCount;
            while (Started == null && DateTime.Now.Subtract(Created).TotalSeconds < Timeout)
            {
                Task.Delay(250).Wait();
                newCount = AgentsTiming.Count;
                if (newCount > startCount)
                {
                    return newCount;
                }
            }
            return newCount;
        }

        private bool AddAgent(string agentId)
        {
            return AgentsTiming.TryAdd(agentId, null);
        }

        /// <summary>
        /// Record information when we allowed agent to start
        /// </summary>
        /// <param name="agentId"></param>
        private void StartAgent(string agentId)
        {
            if (Started == null)
            {
                Started = DateTime.Now;
            }
            AgentsTiming.TryUpdate(agentId, (int)DateTime.Now.Subtract(Started.Value).TotalMilliseconds, null);
        }
    }
}
