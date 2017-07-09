using Microsoft.AspNetCore.Mvc;
using SocialTalents.SyncWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialTalents.SyncWeb.Controllers
{
    public class SyncController : Controller
    {
        // GET: Sync
        [HttpGet]
        [Route("api/Sync/{id?}")]
        public ActionResult Index(string id)
        {
            var model = SyncCollection.Instance.Get(id, -1);
            if (model.Total == -1)
            {
                return Json(noResult);
            }
            else
            {
                return Json(model);
            }
        }

        static SyncModel noResult = new SyncModel("no result", -1);

        [HttpGet]
        [Route("api/Sync/Register/{id?}")]
        public ActionResult Register(string id, int total, string agentId)
        {
            SyncModel model = SyncCollection.Instance.Get(id, total);
            return Content(model.SyncAgent(agentId).ToString());
        }

        [HttpGet]
        [Route("api/Sync/Wait/{id?}")]
        [Route("api/Sync/Progress/{id?}")]
        public ActionResult Progress(string id, int total)
        {
            SyncModel model = SyncCollection.Instance.Get(id, total);
            return Content(model.ExitOnChange().ToString());
        }
    }
}
