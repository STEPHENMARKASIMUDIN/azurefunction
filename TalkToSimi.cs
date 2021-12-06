using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace AzureFunctionTest
{
    public static class TalkToSimi
    {
        [FunctionName("GetAPhoto")]
        public static async Task<IActionResult> GetAPhoto(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Post), Route = null)] HttpRequestMessage req,
            ILogger log)
        {

            var authKey = req.Headers.GetValues("Pexel-Key");
            var key = Credentials.Instance.APIKey;
            foreach (var item in authKey)
            {
                if (!item.Equals(key))
                {
                    return new BadRequestObjectResult("Unauthorized Request!");
                }
            }
            var pexelContent = await req.Content.ReadAsAsync<PexelDTO>();

            var pexelUrl = $"https://api.pexels.com/v1/photos/";
            
            var pexelResult = new PexelDTO.PexelResponseDTO();
            try
            {
         

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{pexelUrl}{pexelContent.id}");
                request.Method = "GET";
                request.Headers.Add("Authorization", key);
                request.ContentType = "application/json";
                
                
                WebResponse webresponse = request.GetResponse();

                using Stream response = webresponse.GetResponseStream();

                if (response != null)
                {
                    using StreamReader reader = new StreamReader(response);

                    string res = reader.ReadToEnd();
                    reader.Close();
                    webresponse.Close();

                    pexelResult = JsonConvert.DeserializeObject<PexelDTO.PexelResponseDTO>(res);

                }
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(ex.ToString());
            }

            return new OkObjectResult(pexelResult);





        }
    }
}
