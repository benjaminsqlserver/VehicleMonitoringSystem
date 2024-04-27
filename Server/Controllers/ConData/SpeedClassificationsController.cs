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
    [Route("odata/ConData/SpeedClassifications")]
    public partial class SpeedClassificationsController : ODataController
    {
        private VehicleMonitoringSystem.Server.Data.ConDataContext context;

        public SpeedClassificationsController(VehicleMonitoringSystem.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification> GetSpeedClassifications()
        {
            var items = this.context.SpeedClassifications.AsQueryable<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>();
            this.OnSpeedClassificationsRead(ref items);

            return items;
        }

        partial void OnSpeedClassificationsRead(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification> items);

        partial void OnSpeedClassificationGet(ref SingleResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/SpeedClassifications(SpeedClassificationID={SpeedClassificationID})")]
        public SingleResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification> GetSpeedClassification(short key)
        {
            var items = this.context.SpeedClassifications.Where(i => i.SpeedClassificationID == key);
            var result = SingleResult.Create(items);

            OnSpeedClassificationGet(ref result);

            return result;
        }
        partial void OnSpeedClassificationDeleted(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification item);
        partial void OnAfterSpeedClassificationDeleted(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification item);

        [HttpDelete("/odata/ConData/SpeedClassifications(SpeedClassificationID={SpeedClassificationID})")]
        public IActionResult DeleteSpeedClassification(short key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.SpeedClassifications
                    .Where(i => i.SpeedClassificationID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSpeedClassificationDeleted(item);
                this.context.SpeedClassifications.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSpeedClassificationDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSpeedClassificationUpdated(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification item);
        partial void OnAfterSpeedClassificationUpdated(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification item);

        [HttpPut("/odata/ConData/SpeedClassifications(SpeedClassificationID={SpeedClassificationID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSpeedClassification(short key, [FromBody]VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SpeedClassifications
                    .Where(i => i.SpeedClassificationID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSpeedClassificationUpdated(item);
                this.context.SpeedClassifications.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SpeedClassifications.Where(i => i.SpeedClassificationID == key);
                
                this.OnAfterSpeedClassificationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/SpeedClassifications(SpeedClassificationID={SpeedClassificationID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSpeedClassification(short key, [FromBody]Delta<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SpeedClassifications
                    .Where(i => i.SpeedClassificationID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSpeedClassificationUpdated(item);
                this.context.SpeedClassifications.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SpeedClassifications.Where(i => i.SpeedClassificationID == key);
                
                this.OnAfterSpeedClassificationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSpeedClassificationCreated(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification item);
        partial void OnAfterSpeedClassificationCreated(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification item)
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

                this.OnSpeedClassificationCreated(item);
                this.context.SpeedClassifications.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SpeedClassifications.Where(i => i.SpeedClassificationID == item.SpeedClassificationID);

                

                this.OnAfterSpeedClassificationCreated(item);

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
