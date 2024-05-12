using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace VehicleMonitoringSystem.Client.Pages
{
    public partial class Vehicles
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

        [Inject]
        public ConDataService ConDataService { get; set; }

        protected IEnumerable<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> vehicles;

        protected RadzenDataGrid<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> grid0;
        protected int count;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetVehicles(filter: $@"(contains(Make,""{search}"") or contains(Model,""{search}"") or contains(LicensePlate,""{search}"") or contains(VIN,""{search}"") or contains(Color,""{search}"") or contains(OwnerName,""{search}"") or contains(OwnerContactAddress,""{search}"") or contains(OwnerPhoneNumber,""{search}"") or contains(OwnerEmail,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                vehicles = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Vehicles" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            //await DialogService.OpenAsync<AddVehicle>("Add Vehicle", null);
            await DialogService.OpenAsync<AddVehicle>("Add New Vehicle", null, new DialogOptions() { Width = $"{650}px", Height = $"{400}px" });
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> args)
        {
            await DialogService.OpenAsync<EditVehicle>("Edit Vehicle", new Dictionary<string, object> { {"VehicleID", args.Data.VehicleID} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, VehicleMonitoringSystem.Server.Models.ConData.Vehicle vehicle)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await ConDataService.DeleteVehicle(vehicleId:vehicle.VehicleID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Vehicle"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConDataService.ExportVehiclesToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Vehicles");
            }

            if (args == null || args.Value == "xlsx")
            {
                await ConDataService.ExportVehiclesToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Vehicles");
            }
        }
    }
}