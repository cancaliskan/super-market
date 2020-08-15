using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using Supermarket.Business.Contracts;
using Supermarket.Common.Helpers;
using Supermarket.Web.Models;

namespace Supermarket.Web.Controllers
{
    public class SalesInformationController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISalesInformationService _salesInformationService;
        private readonly IMapper _mapper;

        public SalesInformationController(IUserService userService, ISalesInformationService salesInformationService, IMapper mapper)
        {
            _userService = userService;
            _salesInformationService = salesInformationService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Orders()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var currentUserResponse = _userService.GetUserByEmail(userEmail);
            if (currentUserResponse.IsSucceed)
            {
                var response = _salesInformationService.Orders(currentUserResponse.Result);
                if (response.IsSucceed)
                {
                    var model = _mapper.Map<List<SalesInformationViewModel>>(response.Result);
                    return View(model);
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Detail(string id)
        {
            var response = _salesInformationService.Detail(id.ToGuid());
            if (response.IsSucceed)
            {
                var model = _mapper.Map<SalesInformationViewModel>(response.Result);
                return View(model);
            }

            return RedirectToAction("Detail");
        }
    }
}