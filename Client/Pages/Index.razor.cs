using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using VehicleMonitoringSystem.Client.ApplicationConstants;
using VehicleMonitoringSystem.Server.Models.ConData;


namespace VehicleMonitoringSystem.Client.Pages
{
    //This comment here is made so that I can check in ApplicationBaseline branch.
    public partial class Index
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        // Dependency Injection for ConDataService
        [Inject]
        protected ConDataService ConData { get; set; }

         // Declaration of variables
        protected int normalSpeedValue;
        protected int slowSpeedValue;
        protected int fastSpeedValue;
        protected int normalSpeedCount;
        protected int slowSpeedCount;
        protected int fastSpeedCount;

        protected List<SpeedMeasurement> trafficOffenders;

        [Inject]
        protected SecurityService Security { get; set; }

         // Method to retrieve vehicle IDs exceeding a given speed limit in the last one month
        protected async Task<List<long?>> GetVehicleIdsExceedingUpperLimit(int upperLimit)
        {
            // Calculate the start and end date for the last one month month
            var startDate = DateTime.Today.AddMonths(-1).Date;
            var endDate = DateTime.Today.Date;

            // Retrieve speed measurements for the last one month
            var speedMeasurementsResult = await ConData.GetSpeedMeasurements(
                filter: $"DateAndTimeInserted ge {startDate:O} and DateAndTimeInserted le {endDate:O}"
            );

            // Filter speed measurements that exceeded the upper limit
            var filteredMeasurements = speedMeasurementsResult.Value.Where(
                measurement => measurement.SpeedInKmPerHour > upperLimit
            );

            // Retrieve unique vehicle IDs from the filtered measurements
            var vehicleIdsThatExceeded = filteredMeasurements.Select(measurement => measurement.VehicleID).Distinct();

            // Convert to a list and return
            return vehicleIdsThatExceeded.ToList();
        }

        // Method to retrieve list of vehicles exceeding a given speed limit in the last one month
        //This method is going to be bound to a datagrid
        protected async Task<List<SpeedMeasurement>> GetListOfVehiclesExceedingUpperLimit(int upperLimit)
        {
            // Calculate the start and end date for the last one month month
            var startDate = DateTime.Today.AddMonths(-1).Date;
            var endDate = DateTime.Today.Date;

            // Retrieve speed measurements for the last one month
            //expand vehicle
            //odata equivalent of fetching a vehucle (full details) record along with each speed measurement
            var speedMeasurementsResult = await ConData.GetSpeedMeasurements(
                filter: $"DateAndTimeInserted ge {startDate:O} and DateAndTimeInserted le {endDate:O}",expand: "Vehicle"
            );

            // Filter speed measurements that exceeded the upper limit
            var filteredMeasurements = speedMeasurementsResult.Value.Where(
                measurement => measurement.SpeedInKmPerHour > upperLimit
            );

         

            // Convert to a list and return
            return filteredMeasurements.ToList();
        }


        // Method to retrieve vehicle IDs not exceeding a given speed limit in the last one month
        protected async Task<List<long?>> VehiclesNotExceedingUpperLimitInTheLastMonth(int upperLimit)
        {
            // Calculate the start and end date for the last one month
            var startDate = DateTime.Today.AddMonths(-1).Date;
            var endDate = DateTime.Today.Date;

            // Retrieve speed measurements for the last one month
            var speedMeasurementsResult = await ConData.GetSpeedMeasurements(
                filter: $"DateAndTimeInserted ge {startDate:O} and DateAndTimeInserted le {endDate:O}"
            );

            // Filter speed measurements that did not exceed the upper limit
            var filteredMeasurements = speedMeasurementsResult.Value.Where(
                measurement => measurement.SpeedInKmPerHour <= upperLimit
            );

            // Retrieve unique vehicle IDs from the filtered measurements
            var vehicleIdsThatDidNotExceed = filteredMeasurements.Select(measurement => measurement.VehicleID).Distinct();

            // Remove vehicles that exceeded the upper limit
            var vehicleIdsThatExceeded = await GetVehicleIdsExceedingUpperLimit(upperLimit);
            vehicleIdsThatDidNotExceed = vehicleIdsThatDidNotExceed.Except(vehicleIdsThatExceeded);

            // Convert to a list and return
            return vehicleIdsThatDidNotExceed.ToList();
        }

         // Method to get the number of vehicles not exceeding a given upper limit
        public async Task<int> GetNumberOfVehiclesNotExceedingUpperLimit(int upperLimit)
        {
            var vehicleIds = await VehiclesNotExceedingUpperLimitInTheLastMonth(upperLimit);
            return vehicleIds.Count();
        }

         // Method to get the number of vehicles exceeding a given upper limit
        public async Task<int> GetNumberOfVehiclesExceedingUpperLimit(int upperLimit)
        {
            var vehicleIds = await GetVehicleIdsExceedingUpperLimit(upperLimit);
            return vehicleIds.Count();
        }

          // Lifecycle method: Runs when the component is initialized
        protected override async Task OnInitializedAsync()
        {
            // Constants for speed classifications
            var SlowSpeedConstant = SpeedClassificationConstants.Slow;
            var NormalSpeedConstant = SpeedClassificationConstants.Normal;

            // Retrieving slow speed value from the database
            var slowSpeedValueResult = await ConData.GetSpeedClassifications(filter: $"Description eq '{SlowSpeedConstant}'");
            var slowSpeedValueList = slowSpeedValueResult.Value.AsODataEnumerable();

            // Retrieving normal speed value from the database
            var normalSpeedValueResult = await ConData.GetSpeedClassifications(filter: $"Description eq '{NormalSpeedConstant}'");
            var normalSpeedValueList = normalSpeedValueResult.Value.AsODataEnumerable();

            // Setting speed values based on database results
            slowSpeedValue = slowSpeedValueList.ElementAtOrDefault(0)?.UpperLimitInKmPerHour ?? 0;
            normalSpeedValue = normalSpeedValueList.ElementAtOrDefault(0)?.UpperLimitInKmPerHour ?? 0;
            fastSpeedValue = normalSpeedValue + 1;

            // Retrieving counts of vehicles based on speed limits
            slowSpeedCount = await GetNumberOfVehiclesNotExceedingUpperLimit(slowSpeedValue);
            normalSpeedCount = await GetNumberOfVehiclesNotExceedingUpperLimit(normalSpeedValue);
            fastSpeedCount = await GetNumberOfVehiclesExceedingUpperLimit(normalSpeedValue);

            //retrieving the list of traffic offenders
            trafficOffenders=await GetListOfVehiclesExceedingUpperLimit(normalSpeedValue);
        }

    }
}