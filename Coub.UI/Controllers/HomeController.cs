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
            Application app = new Application();
            LogOnStatus status = app.LogOn("TRAINING", "Administrator", "P2ssw0rd");
            Console.WriteLine(status);
            if (status != LogOnStatus.LoggedOn)
            {
                throw new UnauthorizedAccessException();
            }
            Session["AdamApp"] = app;
            ViewBag.Base64Image = GetByte64Image();
            return View();
        }

        public FileContentResult GetFile()
        {
            Record rec = new Record((Application)Session["AdamApp"]);
            Guid recId;
            Guid.TryParse("7819340e-b66c-4225-a604-a54500a909b2", out recId);
            rec.Load(recId);

            IReadOnlyImage prev = rec.Files.Master.GetPreview();
            return new FileContentResult(prev.GetBytes(), "image/jpeg");
        }

        private string GetByte64Image()
        {
            Record rec = new Record((Application)Session["AdamApp"]);
            Guid recId;
            Guid.TryParse("7819340e-b66c-4225-a604-a54500a909b2", out recId);
            rec.Load(recId);

            IReadOnlyImage prev = rec.Files.Master.GetPreview();
            byte[] imagData = prev.GetBytes();
            string imageDataString = Convert.ToBase64String(imagData);
            return imageDataString;
        }



        
    }
}
