
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace VehicleMonitoringSystem.Client
{
    public partial class ConDataService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public ConDataService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/ConData/");
        }


        public async System.Threading.Tasks.Task ExportGpsDataToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/gpsdata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/gpsdata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportGpsDataToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/gpsdata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/gpsdata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetGpsData(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>> GetGpsData(Query query)
        {
            return await GetGpsData(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>> GetGpsData(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"GpsData");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGpsData(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>>(response);
        }

        partial void OnCreateGpsDatum(HttpRequestMessage requestMessage);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> CreateGpsDatum(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum gpsDatum = default(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum))
        {
            var uri = new Uri(baseUri, $"GpsData");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(gpsDatum), Encoding.UTF8, "application/json");

            OnCreateGpsDatum(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>(response);
        }

        partial void OnDeleteGpsDatum(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteGpsDatum(long gpsdataId = default(long))
        {
            var uri = new Uri(baseUri, $"GpsData({gpsdataId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteGpsDatum(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetGpsDatumByGpsdataId(HttpRequestMessage requestMessage);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> GetGpsDatumByGpsdataId(string expand = default(string), long gpsdataId = default(long))
        {
            var uri = new Uri(baseUri, $"GpsData({gpsdataId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetGpsDatumByGpsdataId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>(response);
        }

        partial void OnUpdateGpsDatum(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateGpsDatum(long gpsdataId = default(long), VehicleMonitoringSystem.Server.Models.ConData.GpsDatum gpsDatum = default(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum))
        {
            var uri = new Uri(baseUri, $"GpsData({gpsdataId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", gpsDatum.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(gpsDatum), Encoding.UTF8, "application/json");

            OnUpdateGpsDatum(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSpeedMeasurementsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/speedmeasurements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/speedmeasurements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSpeedMeasurementsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/speedmeasurements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/speedmeasurements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSpeedMeasurements(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>> GetSpeedMeasurements(Query query)
        {
            return await GetSpeedMeasurements(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>> GetSpeedMeasurements(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"SpeedMeasurements");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSpeedMeasurements(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>>(response);
        }

        partial void OnCreateSpeedMeasurement(HttpRequestMessage requestMessage);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> CreateSpeedMeasurement(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement speedMeasurement = default(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement))
        {
            var uri = new Uri(baseUri, $"SpeedMeasurements");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(speedMeasurement), Encoding.UTF8, "application/json");

            OnCreateSpeedMeasurement(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>(response);
        }

        partial void OnDeleteSpeedMeasurement(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSpeedMeasurement(long speedId = default(long))
        {
            var uri = new Uri(baseUri, $"SpeedMeasurements({speedId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSpeedMeasurement(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSpeedMeasurementBySpeedId(HttpRequestMessage requestMessage);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> GetSpeedMeasurementBySpeedId(string expand = default(string), long speedId = default(long))
        {
            var uri = new Uri(baseUri, $"SpeedMeasurements({speedId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSpeedMeasurementBySpeedId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>(response);
        }

        partial void OnUpdateSpeedMeasurement(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSpeedMeasurement(long speedId = default(long), VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement speedMeasurement = default(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement))
        {
            var uri = new Uri(baseUri, $"SpeedMeasurements({speedId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", speedMeasurement.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(speedMeasurement), Encoding.UTF8, "application/json");

            OnUpdateSpeedMeasurement(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportVehiclesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/vehicles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/vehicles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportVehiclesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/vehicles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/vehicles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetVehicles(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>> GetVehicles(Query query)
        {
            return await GetVehicles(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>> GetVehicles(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Vehicles");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetVehicles(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>>(response);
        }

        partial void OnCreateVehicle(HttpRequestMessage requestMessage);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> CreateVehicle(VehicleMonitoringSystem.Server.Models.ConData.Vehicle vehicle = default(VehicleMonitoringSystem.Server.Models.ConData.Vehicle))
        {
            var uri = new Uri(baseUri, $"Vehicles");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(vehicle), Encoding.UTF8, "application/json");

            OnCreateVehicle(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>(response);
        }

        partial void OnDeleteVehicle(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteVehicle(long vehicleId = default(long))
        {
            var uri = new Uri(baseUri, $"Vehicles({vehicleId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteVehicle(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetVehicleByVehicleId(HttpRequestMessage requestMessage);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> GetVehicleByVehicleId(string expand = default(string), long vehicleId = default(long))
        {
            var uri = new Uri(baseUri, $"Vehicles({vehicleId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetVehicleByVehicleId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>(response);
        }

        partial void OnUpdateVehicle(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateVehicle(long vehicleId = default(long), VehicleMonitoringSystem.Server.Models.ConData.Vehicle vehicle = default(VehicleMonitoringSystem.Server.Models.ConData.Vehicle))
        {
            var uri = new Uri(baseUri, $"Vehicles({vehicleId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", vehicle.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(vehicle), Encoding.UTF8, "application/json");

            OnUpdateVehicle(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportSpeedClassificationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/speedclassifications/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/speedclassifications/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportSpeedClassificationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/speedclassifications/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/speedclassifications/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetSpeedClassifications(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>> GetSpeedClassifications(Query query)
        {
            return await GetSpeedClassifications(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>> GetSpeedClassifications(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"SpeedClassifications");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSpeedClassifications(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>>(response);
        }

        partial void OnCreateSpeedClassification(HttpRequestMessage requestMessage);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification> CreateSpeedClassification(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification speedClassification = default(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification))
        {
            var uri = new Uri(baseUri, $"SpeedClassifications");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(speedClassification), Encoding.UTF8, "application/json");

            OnCreateSpeedClassification(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>(response);
        }

        partial void OnDeleteSpeedClassification(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteSpeedClassification(short speedClassificationId = default(short))
        {
            var uri = new Uri(baseUri, $"SpeedClassifications({speedClassificationId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteSpeedClassification(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetSpeedClassificationBySpeedClassificationId(HttpRequestMessage requestMessage);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification> GetSpeedClassificationBySpeedClassificationId(string expand = default(string), short speedClassificationId = default(short))
        {
            var uri = new Uri(baseUri, $"SpeedClassifications({speedClassificationId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetSpeedClassificationBySpeedClassificationId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification>(response);
        }

        partial void OnUpdateSpeedClassification(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateSpeedClassification(short speedClassificationId = default(short), VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification speedClassification = default(VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification))
        {
            var uri = new Uri(baseUri, $"SpeedClassifications({speedClassificationId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", speedClassification.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(speedClassification), Encoding.UTF8, "application/json");

            OnUpdateSpeedClassification(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}