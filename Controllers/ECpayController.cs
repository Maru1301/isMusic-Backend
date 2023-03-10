using api.iSMusic.Models;
using api.iSMusic.Models.EFModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;
using static System.Net.WebRequestMethods;

namespace api.iSMusic.Controllers
{
    public class ECpayController : Controller
    {
        [HttpPost("EcPay")]
        public string Ecpay(SendToNewebPayIn Cart)
        {
            Cart.productNames=Cart.productName.Split(',');

            int memberId = int.Parse(HttpContext.User.Claims.First(claim => claim.Type == "MemberId").Value);

            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

            var order = new Dictionary<string, object>
            {
                //特店交易編號
                { "MerchantTradeNo",  orderId},

                //交易金額
                { "TotalAmount",  Cart.Total},

                //交易描述
                { "TradeDesc",  "測試"},

                //特店交易時間 yyyy/MM/dd HH:mm:ss
                { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},

                //完成後發通知
                { "ReturnURL",  "http://localhost:8080"},

                //付款完成後導頁
                { "OrderResultURL", "http://localhost:8080" },

                //特店編號， 2000132 測試綠界編號
                { "MerchantID",  "3002599"},

                //交易類型 固定填入 aio
                { "PaymentType",  "aio"},

                //選擇預設付款方式 固定填入 ALL
                { "ChoosePayment",  "Credit"},

                //CheckMacValue 加密類型 固定填入 1 (SHA256)
                { "EncryptType",  "1"},
            };

            order["ItemName"] = string.Join("#", Cart.productNames);


            //檢查碼
            order["CheckMacValue"] = GetCheckMacValue(order);

            StringBuilder s = new StringBuilder();
            s.AppendFormat("<form id='payForm' action='{0}' method='post'>", "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5");
            foreach (KeyValuePair<string, object> item in order)
            {
                s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", item.Key, item.Value);
            }

            s.Append("</form>");

            return s.ToString();
        }
        private string GetCheckMacValue(Dictionary<string, object> order)
        {
            var param = order.Keys.OrderBy(x => x).Select(key => key + "=" + order[key]).ToList();

            var checkValue = string.Join("&", param);

            //測試用的 HashKey
            var hashKey = "spPjZn66i0OhqJsQ";

            //測試用的 HashIV
            var HashIV = "hT5OJckN45isQTTs";

            checkValue = $"HashKey={hashKey}" + "&" + checkValue + $"&HashIV={HashIV}";

            checkValue = HttpUtility.UrlEncode(checkValue).ToLower();

            checkValue = EncryptSHA256(checkValue);

            return checkValue.ToUpper();
        }

        public string EncryptSHA256(string source)
        {
            string result = string.Empty;

            using (System.Security.Cryptography.SHA256 algorithm = System.Security.Cryptography.SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(source));

                if (hash != null)
                {
                    result = BitConverter.ToString(hash)?.Replace("-", string.Empty)?.ToUpper();
                }

            }
            return result;
        }
    }
    
}
