using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Orders;
using Adam.Core.Records;


namespace Coub.UI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public FileContentResult GetFile()
        {
            #region LogOn

            Application app = new Application();
            LogOnStatus status = app.LogOn("TRAINING", "Demidov", "123456");
            
            if (status != LogOnStatus.LoggedOn)
            {
                throw new UnauthorizedAccessException();
            }

            #endregion    
       
            Record rec = new Record(app);
            Guid recId;
            Guid.TryParse("7819340e-b66c-4225-a604-a54500a909b2", out recId);
            rec.Load(recId);

            IReadOnlyImage prev = rec.Files.Master.GetPreview();
            return new FileContentResult(prev.GetBytes(), "image/jpeg");
            
        }

    }
}
