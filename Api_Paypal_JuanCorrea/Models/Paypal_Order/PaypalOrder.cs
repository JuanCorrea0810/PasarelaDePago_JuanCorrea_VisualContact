using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_Paypal_JuanCorrea.Models.Paypal_Order
{
    public class PaypalOrder
    {
        public string intent { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public PaymentSource payment_source { get; set; }

    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class ExperienceContext
    {
        public string payment_method_preference { get; set; }
        public string brand_name { get; set; }
        public string locale { get; set; }
        public string landing_page { get; set; }
        public string shipping_preference { get; set; }
        public string user_action { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }

    public class PaymentSource
    {
        public Paypal paypal { get; set; }
    }

    public class Paypal
    {
        public ExperienceContext experience_context { get; set; }
    }

    public class PurchaseUnit
    {
        public string reference_id { get; set; }
        public Amount amount { get; set; }
    }
}