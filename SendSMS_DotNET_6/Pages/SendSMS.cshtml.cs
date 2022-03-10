using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SendSMS_DotNET_6.Models;
using System.Net.Http.Headers;

namespace SendSMS_DotNET_6.Pages
{
    public class SendSMSModel : PageModel
    {
        private readonly ILogger<SendSMSModel> _logger;

        public SendSMSModel(ILogger<SendSMSModel> logger)
        {
            _logger = logger;
        }
        //https://alpha.net.bd/SMS/API/
        public BalanceResponse BalanceResponse { get; set; }=new BalanceResponse();
        public void OnGet()
        {
            using (var client = new HttpClient())
            {
                string API_Url = "https://api.sms.net.bd/user/balance/";
                string API_Key = "";
            
                client.BaseAddress = new Uri(API_Url);// //
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync("?api_key=" + API_Key).Result;
                using (HttpContent content = response.Content)
                {
                    var bkresult = content.ReadAsStringAsync().Result;
                    dynamic stuff = JsonConvert.DeserializeObject(bkresult);
                    if (stuff.error == "0")
                    {
                        BalanceResponse.error = stuff.error;
                        BalanceResponse.msg = stuff.msg;
                        string data = stuff["data"].ToString();
                        BalanceResponse.data = JsonConvert.DeserializeObject<Balance>(data);
                        //BalanceResponse.data.validity = stuff.data["validity"];
                        //Console.WriteLine(stuff.msg);
                    }
                    else
                    {
                        BalanceResponse.msg = stuff.msg;
                        //BalanceResponse.data = stuff.data;
                        //Console.WriteLine("Sms Not Send, " + stuff.msg);
                    }

                }
            }
        }

        public SMSRequest SMSRequest { get; set; }
        public async Task<IActionResult> OnPost([FromForm] SMSRequest request)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string API_Url = "https://api.sms.net.bd/sendsms";
                    string API_Key = "";

                    client.BaseAddress = new Uri(API_Url);// //
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync("?api_key=" + API_Key + "&msg=" + request.Text_Massage + "&to=" + request.To_Phone_Number).Result;
                    using (HttpContent content = response.Content)
                    {
                        var bkresult = content.ReadAsStringAsync().Result;
                        dynamic stuff = JsonConvert.DeserializeObject(bkresult);
                        if (stuff.error == "0")
                        {
                            return Redirect("/SendSMS");
                            //Console.WriteLine(stuff.msg);
                        }
                        else
                        {
                            return Redirect("/");
                            //Console.WriteLine("Sms Not Send, " + stuff.msg);
                        }

                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}