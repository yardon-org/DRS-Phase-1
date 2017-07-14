using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using OAuthHMAC.Web.Filters;

namespace OAuthHMAC.Web.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
    
        [Route("api/Orders/Get")]
        public IHttpActionResult Get()
        {
            ClaimsPrincipal principal = Request.GetRequestContext().Principal as ClaimsPrincipal;

            var Name = ClaimsPrincipal.Current.Identity.Name;

            return Ok(Order.CreateOrders());
        }

        [Route("")]
        public IHttpActionResult Post(Order order)
        {
            return Ok(order);
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string County { get; set; }
        public Boolean DeliveredPackage { get; set; }

        public static List<Order> CreateOrders()
        {
            List<Order> OrderList = new List<Order>()
            {
                new Order {OrderId = 1, CustomerName = "Person One", County = "Kent", DeliveredPackage = true},
                new Order {OrderId = 2, CustomerName = "Person Two", County = "London", DeliveredPackage = false}
            };

            return OrderList;
        }
    }
}