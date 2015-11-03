using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Orders;
using Adam.Core.Records;
using Adam.Core.Search;


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
            
            return View();
        }

        public ActionResult GetPreviews()
        {
            List<string> model = GetByte64Previews();
            return PartialView("_PreviewsPartial", model);
        }

        public ActionResult GetTestString()
        {
            return View("_TestView",(object)"asd");
        }

        private List<string> GetByte64Previews()
        {
            RecordCollection recordCollection = new RecordCollection((Application)Session["AdamApp"]);
            Adam.Core.Search.SearchExpression se = new Adam.Core.Search.SearchExpression("File.Version.Extension = jpg");
            
            recordCollection.Load(se);
            List<string> base64Previews = new List<string>();
            byte[] tempBytes;
            foreach (Record rec in recordCollection)
            {
                tempBytes = rec.Files.Master.GetPreview().GetBytes();
                base64Previews.Add(Convert.ToBase64String(tempBytes));
            }

            return base64Previews;
        }


        #region Temp

        public FileContentResult GetFile()
        {
            Record rec = new Record((Application) Session["AdamApp"]);
            Guid recId;
            Guid.TryParse("7819340e-b66c-4225-a604-a54500a909b2", out recId);
            rec.Load(recId);

            IReadOnlyImage prev = rec.Files.Master.GetPreview();
            return new FileContentResult(prev.GetBytes(), "image/jpeg");
        }

        private string GetByte64Image()
        {
            Record rec = new Record((Application) Session["AdamApp"]);
            Guid recId;
            Guid.TryParse("7819340e-b66c-4225-a604-a54500a909b2", out recId);
            rec.Load(recId);

            IReadOnlyImage prev = rec.Files.Master.GetPreview();
            byte[] imagData = prev.GetBytes();
            string imageDataString = Convert.ToBase64String(imagData);
            return imageDataString;
        }

        #endregion


    }
}
