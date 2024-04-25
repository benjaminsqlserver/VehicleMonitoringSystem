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
    [Route("odata/ConData/SpeedMeasurements")]
    public partial class SpeedMeasurementsController : ODataController
    {
        private VehicleMonitoringSystem.Server.Data.ConDataContext context;

        public SpeedMeasurementsController(VehicleMonitoringSystem.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> GetSpeedMeasurements()
        {
            var items = this.context.SpeedMeasurements.AsQueryable<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>();
            this.OnSpeedMeasurementsRead(ref items);

            return items;
        }

        partial void OnSpeedMeasurementsRead(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> items);

        partial void OnSpeedMeasurementGet(ref SingleResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/SpeedMeasurements(SpeedID={SpeedID})")]
        public SingleResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> GetSpeedMeasurement(long key)
        {
            var items = this.context.SpeedMeasurements.Where(i => i.SpeedID == key);
            var result = SingleResult.Create(items);

            OnSpeedMeasurementGet(ref result);

            return result;
        }
        partial void OnSpeedMeasurementDeleted(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);
        partial void OnAfterSpeedMeasurementDeleted(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);

        [HttpDelete("/odata/ConData/SpeedMeasurements(SpeedID={SpeedID})")]
        public IActionResult DeleteSpeedMeasurement(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.SpeedMeasurements
                    .Where(i => i.SpeedID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSpeedMeasurementDeleted(item);
                this.context.SpeedMeasurements.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSpeedMeasurementDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSpeedMeasurementUpdated(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);
        partial void OnAfterSpeedMeasurementUpdated(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);

        [HttpPut("/odata/ConData/SpeedMeasurements(SpeedID={SpeedID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSpeedMeasurement(long key, [FromBody]VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SpeedMeasurements
                    .Where(i => i.SpeedID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSpeedMeasurementUpdated(item);
                this.context.SpeedMeasurements.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SpeedMeasurements.Where(i => i.SpeedID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Vehicle");
                this.OnAfterSpeedMeasurementUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/SpeedMeasurements(SpeedID={SpeedID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSpeedMeasurement(long key, [FromBody]Delta<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SpeedMeasurements
                    .Where(i => i.SpeedID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSpeedMeasurementUpdated(item);
                this.context.SpeedMeasurements.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SpeedMeasurements.Where(i => i.SpeedID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Vehicle");
                this.OnAfterSpeedMeasurementUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSpeedMeasurementCreated(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);
        partial void OnAfterSpeedMeasurementCreated(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item)
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

                this.OnSpeedMeasurementCreated(item);
                this.context.SpeedMeasurements.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SpeedMeasurements.Where(i => i.SpeedID == item.SpeedID);

                Request.QueryString = Request.QueryString.Add("$expand", "Vehicle");

                this.OnAfterSpeedMeasurementCreated(item);

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
