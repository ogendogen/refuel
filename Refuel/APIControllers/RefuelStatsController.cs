using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Refuel.APIModels;

namespace Refuel.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefuelStatsController : ControllerBase
    {
        private readonly IUsersManager _usersManager;
        private readonly IVehiclesManager _vehiclesManager;

        public RefuelStatsController(IUsersManager usersManager, IVehiclesManager vehiclesManager)
        {
            _usersManager = usersManager;
            _vehiclesManager = vehiclesManager;
        }

        [HttpGet("GetVehicleData/{vehicleId}")]
        public async Task<ActionResult<RefuelStatsAPIResponse>> GetVehicleData(int vehicleId)
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type.Contains("nameidentifier"));
            if (claim == null)
            {
                return new RefuelStatsError()
                {
                    Message = "access denied"
                };
            }

            int userId = Int32.Parse(claim.Value);
            User user = await _usersManager.GetUserById(userId);
            if (user == null)
            {
                return new RefuelStatsError()
                {
                    Message = "access denied"
                };
            }

            Vehicle vehicle = _vehiclesManager.GetVehicleById(vehicleId);
            if (!_vehiclesManager.IsUserOwnsVehicle(userId, vehicleId))
            {
                return new RefuelStatsError()
                {
                    Message = "not an owner"
                };
            }

            return new RefuelStatsCorrectResponse()
            {
                Message = "ok",
                AverageCombustion = _vehiclesManager.GetVehicleAverageCombustion(vehicle),
                PriceFor100Km = _vehiclesManager.GetPriceForNKilometers(vehicle, 100)
            };
        }
    }
}
