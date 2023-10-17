using Microsoft.AspNetCore.Mvc;
using mtg_lib;
using mtg_lib.Library.Models;

namespace mtg.Controllers
{
    [Route("[Controller]")]
    public class CartController : Controller
    {

        private readonly MTGService service;

        public CartController()
        {
            service = new MTGService();
        }

        [Route("")]
        public IActionResult Cart()
        {
            var cartSession = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartSession))
            {
                ViewBag.Cart = cartSession;

                return View();
            }

            Dictionary<long, int> cart = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<long, int>>(cartSession);

            ICollection<long> cardsId = cart.Keys;

            List<Card> cards = new List<Card>();

            foreach (long id in cardsId)
            {
                cards.Add(service.GetCardByID(id));
            }

            ViewBag.Cart = cartSession;

            return View(cards);
        }

        [HttpPost]
        public IActionResult orderFromCart(){
            var cartSession = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartSession)){
                return RedirectToAction("Cart", "Cart");
            }

            Dictionary<long, int> cart = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<long, int>>(cartSession);

            ICollection<long> cardsId = cart.Keys;

            foreach (long id in cardsId){
                for (int i = 0; i < cart[id]; i++){
                    service.PurchaseCard(User.Identity.Name, id);
                }
            }

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Orders", "Account");
        }

    }
}

    