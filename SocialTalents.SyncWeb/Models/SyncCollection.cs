using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace SocialTalents.SyncWeb.Models
{
    public class SyncCollection
    {
        public SyncCollection()
        {
            DeleteQueue = new List<SyncModel>();
            Syncs = new ConcurrentDictionary<string, SyncModel>();
            Task cleanup = new Task(() => Cleanup());
            cleanup.Start();
        }

        // We do not want to use dependency injection in such small project, singleton is fine
        public static SyncCollection Instance { get; } = new SyncCollection();

        // Queue to delete sync models 
        List<SyncModel> DeleteQueue { get; set; }
        // Dictinary to quickly find models by id
        ConcurrentDictionary<string, SyncModel> Syncs { get; set; }

        /// <summary>
        /// Get or create a model, use total for new models
        /// </summary>
        /// <param name="syncId"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public SyncModel Get(string syncId, int total)
        {
            var r1 = Syncs.GetOrAdd(syncId, (a) => { var r = new SyncModel(a, total); DeleteQueue.Add(r); return r; });
            return r1;
        }

        /// <summary>
        /// Periodic cleanup process based on SyncModel.DeleteAt field
        /// </summary>
        public static void Cleanup()
        {
            while (true)
            {
                Task.Delay(5000).Wait();
                DateTime now = DateTime.Now;
                SyncModel[] toDelete = Instance.DeleteQueue.Where(s => s.DeleteAt < now).ToArray();
                foreach (var e in toDelete)
                {
                    Instance.DeleteQueue.Remove(e);
                    SyncModel outValue;
                    Instance.Syncs.TryRemove(e.SyncId, out outValue);
                }
            }
        }

    }
}
