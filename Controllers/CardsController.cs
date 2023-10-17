using mtg_lib.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mtg_lib;

namespace mtg.Controllers
{
    [Route("")]
    [Route("[Controller]")]
    public class CardsController : Controller
    {
        private readonly MTGService service;
        private const int PageSize = 25;

        public CardsController()
        {
            service = new MTGService();
        }

        [Route("")]
        [Route("[Action]")]
        public IActionResult Cards(int page = 1, string searchTerm = null, int? minManaCost = null, int? maxManaCost = null, int? minPower = null, int? maxPower = null, string sortCriteria = null)
{
    IEnumerable<Card> allCards = service.GetAllCards();

    if (!string.IsNullOrEmpty(searchTerm))
    {
        allCards = allCards.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    if (minManaCost.HasValue)
{
    allCards = allCards.Where(c => int.TryParse(c.ConvertedManaCost, out int manaCost) && manaCost >= minManaCost.Value);
}

if (maxManaCost.HasValue)
{
    allCards = allCards.Where(c => int.TryParse(c.ConvertedManaCost, out int manaCost) && manaCost <= maxManaCost.Value);
}

if (minPower.HasValue)
{
    allCards = allCards.Where(c => int.TryParse(c.Power, out int power) && power >= minPower.Value);
}

if (maxPower.HasValue)
{
    allCards = allCards.Where(c => int.TryParse(c.Power, out int power) && power <= maxPower.Value);
}


    if (!string.IsNullOrEmpty(sortCriteria))
            {
                switch (sortCriteria)
                {
                    case "default":
                        allCards = allCards.OrderBy(c => c.Id);
                        break;
                    case "nameAsc":
                        allCards = allCards.OrderBy(c => c.Name);
                        break;
                    case "nameDesc":
                        allCards = allCards.OrderByDescending(c => c.Name);
                        break;
                    case "manaCostAsc":
                        allCards = allCards.OrderBy(c => c.ConvertedManaCost);
                        break;
                    case "manaCostDesc":
                        allCards = allCards.OrderByDescending(c => c.ConvertedManaCost);
                        break;
                    case "powerAsc":
                        allCards = allCards.OrderBy(c => c.Power);
                        break;
                    case "powerDesc":
                        allCards = allCards.OrderByDescending(c => c.Power);
                        break;
                }
            }

    int totalCount = allCards.Count();
    int totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

    allCards = allCards.Skip((page - 1) * PageSize).Take(PageSize);

    ViewData["SearchTerm"] = searchTerm;
    ViewData["MinManaCost"] = minManaCost;
    ViewData["MaxManaCost"] = maxManaCost;
    ViewData["MinPower"] = minPower;
    ViewData["MaxPower"] = maxPower;
    ViewData["SortCriteria"] = sortCriteria;
    ViewData["Page"] = page;
    ViewData["TotalPages"] = totalPages;

    return View(allCards);
}

    [HttpPost]
    public IActionResult AddToCart(long cardId){

        if (!HttpContext.Session.TryGetValue("Cart", out byte[] cartBytes) || cartBytes.Length == 0)
        {
            Dictionary<long, int> cart = new Dictionary<long, int>();

            cart.Add(cardId, 1);

            string cartString = Newtonsoft.Json.JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartString);
        } else {
            string cartString = HttpContext.Session.GetString("Cart");
            Dictionary<long, int> cart = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<long, int>>(cartString);

            if (cart.ContainsKey(cardId))
            {
                cart[cardId]++;
            } else {
                cart.Add(cardId, 1);
            }

            cartString = Newtonsoft.Json.JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("Cart", cartString);
        }

        return RedirectToAction("Cart", "Cart");
    }
    }
}
