using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mtg.ViewModels;
using mtg_lib;
using mtg_lib.Library.Models;

namespace mtg.Controllers
{
    [Route("[Controller]")]
    public class MagicTheGatheringController : Controller
    {
        [Route("")]
        [Route("[Action]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/DetailedCard/{id}")]
        [Route("[Action]")]
        public IActionResult DetailedCard(long id)
        {
            MTGService service = new MTGService();    
            Card card = service.GetCardByID(id);
            CardViewModel cardViewModel = new CardViewModel(card);
            return View(cardViewModel);
        }

        [Route("ConfirmPurchase/{id}")]
        [Route("[Action]")]
        [Authorize]
        public IActionResult ConfirmPurchase(long id)
        {
            MTGService service = new MTGService();    
            service.PurchaseCard(User.Identity.Name, id);
            return View();
        }
    }
}