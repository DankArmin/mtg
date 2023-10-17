using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using mtg.ViewModels;
using mtg_lib.Library.Models;
using mtg_lib;
using System.Linq;
using System.Security.Claims;
using Auth0.AspNetCore.Authentication;

namespace mtg.Controllers
{
    public class AccountController : Controller
    {
        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in 
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Route("Account/Profile")]
        [Route("[Action]")]
        [Authorize]
        public IActionResult Profile()
        {
            return View(new UserProfileViewModel()
            {
                Name = User.Identity.Name,
                EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
            });
        }

        [Route("Account/Orders")]
        [Route("[Action]")]
        [Authorize]
        public IActionResult Orders()
        {
                MTGService service = new MTGService();

                string name = User.Identity.Name;
                List<Order> orders = service.GetOrders(name);
                List<UserOrdersViewModel> userOrderViewModels = new List<UserOrdersViewModel>();

                foreach (Order order in orders) {
                        UserOrdersViewModel userOrderViewModel = new UserOrdersViewModel();
                        userOrderViewModel.order = order;
                        userOrderViewModel.card = service.GetCardByID(order.CardId);
                        userOrderViewModels.Add(userOrderViewModel);
                } 
                return View(userOrderViewModels);
        }

        /// <summary>
        /// This is just a helper action to enable you to easily see all claims related to a user. It helps when debugging your
        /// application to see the in claims populated from the Auth0 ID Token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
