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
    [Route("odata/ConData/GpsData")]
    public partial class GpsDataController : ODataController
    {
        private VehicleMonitoringSystem.Server.Data.ConDataContext context;

        public GpsDataController(VehicleMonitoringSystem.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> GetGpsData()
        {
            var items = this.context.GpsData.AsQueryable<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>();
            this.OnGpsDataRead(ref items);

            return items;
        }

        partial void OnGpsDataRead(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> items);

        partial void OnGpsDatumGet(ref SingleResult<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/GpsData(GPSDataID={GPSDataID})")]
        public SingleResult<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> GetGpsDatum(long key)
        {
            var items = this.context.GpsData.Where(i => i.GPSDataID == key);
            var result = SingleResult.Create(items);

            OnGpsDatumGet(ref result);

            return result;
        }
        partial void OnGpsDatumDeleted(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);
        partial void OnAfterGpsDatumDeleted(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);

        [HttpDelete("/odata/ConData/GpsData(GPSDataID={GPSDataID})")]
        public IActionResult DeleteGpsDatum(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.GpsData
                    .Where(i => i.GPSDataID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnGpsDatumDeleted(item);
                this.context.GpsData.Remove(item);
                this.context.SaveChanges();
                this.OnAfterGpsDatumDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnGpsDatumUpdated(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);
        partial void OnAfterGpsDatumUpdated(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);

        [HttpPut("/odata/ConData/GpsData(GPSDataID={GPSDataID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutGpsDatum(long key, [FromBody]VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.GpsData
                    .Where(i => i.GPSDataID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnGpsDatumUpdated(item);
                this.context.GpsData.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.GpsData.Where(i => i.GPSDataID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Vehicle");
                this.OnAfterGpsDatumUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/GpsData(GPSDataID={GPSDataID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchGpsDatum(long key, [FromBody]Delta<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.GpsData
                    .Where(i => i.GPSDataID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnGpsDatumUpdated(item);
                this.context.GpsData.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.GpsData.Where(i => i.GPSDataID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Vehicle");
                this.OnAfterGpsDatumUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnGpsDatumCreated(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);
        partial void OnAfterGpsDatumCreated(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item)
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

                this.OnGpsDatumCreated(item);
                this.context.GpsData.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.GpsData.Where(i => i.GPSDataID == item.GPSDataID);

                Request.QueryString = Request.QueryString.Add("$expand", "Vehicle");

                this.OnAfterGpsDatumCreated(item);

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
