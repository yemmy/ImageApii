using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ImageApi.Controllers
{
    //public class UploadController : ApiController
    //{

    [RoutePrefix("api/Upload")]
    public class UploadController : ApiController
    {
        [HttpPost]
        [Route("savefootage/{ip}/{quality}")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> SaveFootage(string ip,string quality)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {
                var httpRequest = HttpContext.Current.Request;
                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));

                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            var message = string.Format("Please Upload a file upto 1 mb.");
                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            await Task.Run(() =>
                            {
                                //get sender IP
                                var filePath = "C:/Monit/INTELICAMFOOTAGES/" + ip.Replace(".","P")+ext;//HttpContext..Server.MapPath("~/Userimage/" + postedFile.FileName);

                                
                                if(File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                                postedFile.SaveAs(filePath);
                            });
                        }

                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format(ex.Message);
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpGet]
        [Route("logStatus/{ip}/{status}")]
        [AllowAnonymous]
        public async Task<bool> LogStatus(string ip,string status)
        {
            await Task.Delay(2000);
            return true;
        }
        
        [HttpGet]
        [Route("getconfig/{ip}")]
        [AllowAnonymous]
        public async Task<string> GetConfig(string ip)
        {
            await Task.Delay(2000);
            return "Result";
        }

    }
}