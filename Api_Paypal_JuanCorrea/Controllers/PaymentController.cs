using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Net.Http.Headers;
using System.Text;
using Api_Paypal_JuanCorrea.Models.Paypal_Order;
using Api_Paypal_JuanCorrea.Models.Paypal_Transaction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Specialized;
using Api_Paypal_JuanCorrea.Models;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Api_Paypal_JuanCorrea.Controllers
{
    [ApiController]
    [Route("api/Payment")]
    public class PaymentController : Controller
    {
        [HttpPost]
        [Route("Confirm")]
        public async Task<ActionResult> ConfirmarCompra([FromQuery]string token)
        {
            bool status = false;
            string IdTransaccion = null;
            using (var client = new HttpClient())
            {

                var userName = "AePKtIMdkax086onN9ZiRc5QIbQ4SEHRh8XPL7xcoBfQ-2OGfD317KZMbRvKC6kLBMrgVDh_FYU5Xd9_";
                var passwd = "EN6uWCEUgZ2kj-9htpidWS5R6GvhO360Cc9oe30hQYRxJDuOw2RTrsu95YpMokwI1andf6KVb_7BBn4L";

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);


                status = response.IsSuccessStatusCode;

                if (status)
                {
                    var jsonRespuesta = response.Content.ReadAsStringAsync().Result;

                    PaypalTransaction objeto = JsonConvert.DeserializeObject<PaypalTransaction>(jsonRespuesta);

                    IdTransaccion = objeto.purchase_units[0].payments.captures[0].id;
                }

            }
            var result = new {
                status,
                IdTransaccion
            };
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> Paypal([FromBody]Purchase purchase)
        {
            bool status = false;
            string result = null;

            using (var client = new HttpClient())
            {

                var userName = "AePKtIMdkax086onN9ZiRc5QIbQ4SEHRh8XPL7xcoBfQ-2OGfD317KZMbRvKC6kLBMrgVDh_FYU5Xd9_";
                var passwd = "EN6uWCEUgZ2kj-9htpidWS5R6GvhO360Cc9oe30hQYRxJDuOw2RTrsu95YpMokwI1andf6KVb_7BBn4L";

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));


                var orden = new PaypalOrder()
                {
                    intent = "CAPTURE",
                    purchase_units = new List<Models.Paypal_Order.PurchaseUnit>() {
                        new Models.Paypal_Order.PurchaseUnit() {
                            reference_id = "default",
                            amount = new Models.Paypal_Order.Amount() {
                                currency_code = "USD",
                                value = purchase.precio
                            }
                        }
                    },
                    payment_source = new PaymentSource()
                    {
                        paypal = new Paypal()
                        {
                            experience_context = new ExperienceContext()
                            {
                                payment_method_preference = "IMMEDIATE_PAYMENT_REQUIRED",
                                brand_name = "Mi Tienda",
                                landing_page = "NO_PREFERENCE",
                                locale = "en-US",
                                user_action = "PAY_NOW", //Accion para que paypal muestre el monto de pago
                                return_url = "http://127.0.0.1:5500/Projects/javascript-basic-projects/22-products/final/transaccion.html",// cuando se aprovo la solicitud del cobro
                                cancel_url = "http://127.0.0.1:5500/Projects/javascript-basic-projects/22-products/final/transaccion.html"// cuando cancela la operacion
                            }
                        }
                    }
                };


                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);
                status = response.IsSuccessStatusCode;
                if (status)
                {
                    var respuesta = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    result = respuesta.ToString();
                }
                
            }
            return Ok(result); 
        }

    }

}