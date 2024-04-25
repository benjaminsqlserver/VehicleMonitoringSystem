using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using VehicleMonitoringSystem.Server.Data;

namespace VehicleMonitoringSystem.Server.Controllers
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;
        private readonly ConDataService service;

        public ExportConDataController(ConDataContext context, ConDataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ConData/gpsdata/csv")]
        [HttpGet("/export/ConData/gpsdata/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGpsDataToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetGpsData(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/gpsdata/excel")]
        [HttpGet("/export/ConData/gpsdata/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportGpsDataToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetGpsData(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/speedmeasurements/csv")]
        [HttpGet("/export/ConData/speedmeasurements/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSpeedMeasurementsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSpeedMeasurements(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/speedmeasurements/excel")]
        [HttpGet("/export/ConData/speedmeasurements/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSpeedMeasurementsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSpeedMeasurements(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/vehicles/csv")]
        [HttpGet("/export/ConData/vehicles/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVehiclesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVehicles(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/vehicles/excel")]
        [HttpGet("/export/ConData/vehicles/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVehiclesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVehicles(), Request.Query, false), fileName);
        }
    }
}
