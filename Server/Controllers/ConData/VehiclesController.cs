using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VehicleMonitoringSystem.Server.Controllers.ConData
{
    [Route("odata/ConData/Vehicles")]
    public partial class VehiclesController : ODataController
    {
        private VehicleMonitoringSystem.Server.Data.ConDataContext context;

        public VehiclesController(VehicleMonitoringSystem.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> GetVehicles()
        {
            var items = this.context.Vehicles.AsQueryable<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>();
            this.OnVehiclesRead(ref items);

            return items;
        }

        partial void OnVehiclesRead(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> items);

        partial void OnVehicleGet(ref SingleResult<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/Vehicles(VehicleID={VehicleID})")]
        public SingleResult<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> GetVehicle(long key)
        {
            var items = this.context.Vehicles.Where(i => i.VehicleID == key);
            var result = SingleResult.Create(items);

            OnVehicleGet(ref result);

            return result;
        }
        partial void OnVehicleDeleted(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);
        partial void OnAfterVehicleDeleted(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);

        [HttpDelete("/odata/ConData/Vehicles(VehicleID={VehicleID})")]
        public IActionResult DeleteVehicle(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Vehicles
                    .Where(i => i.VehicleID == key)
                    .Include(i => i.GpsData)
                    .Include(i => i.SpeedMeasurements)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnVehicleDeleted(item);
                this.context.Vehicles.Remove(item);
                this.context.SaveChanges();
                this.OnAfterVehicleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnVehicleUpdated(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);
        partial void OnAfterVehicleUpdated(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);

        [HttpPut("/odata/ConData/Vehicles(VehicleID={VehicleID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutVehicle(long key, [FromBody]VehicleMonitoringSystem.Server.Models.ConData.Vehicle item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Vehicles
                    .Where(i => i.VehicleID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnVehicleUpdated(item);
                this.context.Vehicles.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Vehicles.Where(i => i.VehicleID == key);
                
                this.OnAfterVehicleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/Vehicles(VehicleID={VehicleID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchVehicle(long key, [FromBody]Delta<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Vehicles
                    .Where(i => i.VehicleID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnVehicleUpdated(item);
                this.context.Vehicles.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Vehicles.Where(i => i.VehicleID == key);
                
                this.OnAfterVehicleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnVehicleCreated(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);
        partial void OnAfterVehicleCreated(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] VehicleMonitoringSystem.Server.Models.ConData.Vehicle item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnVehicleCreated(item);
                this.context.Vehicles.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Vehicles.Where(i => i.VehicleID == item.VehicleID);

                

                this.OnAfterVehicleCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
