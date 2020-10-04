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

        [HttpGet("GetVehicleData")]
        public async Task<ActionResult<RefuelStatsAPIResponse>> GetVehicleData(int vehicleId)
        {
            if (!IsAuthorized(vehicleId, out string errorMessage))
            {
                return new RefuelStatsError()
                {
                    Message = errorMessage
                };
            }

            Vehicle vehicle = _vehiclesManager.GetVehicleById(vehicleId);

            return new RefuelStatsCorrectResponse()
            {
                Message = "ok",
                AverageCombustion = _vehiclesManager.GetVehicleAverageCombustion(vehicle),
                PriceFor100Km = _vehiclesManager.GetPriceForNKilometers(vehicle, 100),
                TotalCosts = await _vehiclesManager.GetVehicleTotalCosts(vehicle)
            };
        }

        [HttpGet("GetVehicleFuelStats")]
        public async Task<ActionResult<RefuelStatsAPIResponse>> GetVehicleFuelStats(int vehicleId, FuelType fuelType)
        {
            if (!IsAuthorized(vehicleId, out string errorMessage))
            {
                return new RefuelStatsError()
                {
                    Message = errorMessage
                };
            }

            throw new NotImplementedException();
        }

        private bool IsAuthorized(int vehicleId, out string errorMessage)
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type.Contains("nameidentifier"));
            if (claim == null)
            {
                errorMessage = "access denied";
                return false;
            }

            int userId = Int32.Parse(claim.Value);
            User user = _usersManager.GetUserById(userId).Result;
            if (user == null)
            {
                errorMessage = "access denied";
                return false;
            }

            Vehicle vehicle = _vehiclesManager.GetVehicleById(vehicleId);
            if (!_vehiclesManager.IsUserOwnsVehicle(userId, vehicleId))
            {
                errorMessage = "not an owner";
                return false;
            }

            errorMessage = "";
            return true;
        }
    }
}
