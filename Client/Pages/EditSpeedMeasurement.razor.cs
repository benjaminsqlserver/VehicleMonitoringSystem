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
    public partial class EditSpeedMeasurement
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

        [Parameter]
        public long SpeedID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            speedMeasurement = await ConDataService.GetSpeedMeasurementBySpeedId(speedId:SpeedID);
        }
        protected bool errorVisible;
        protected VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement speedMeasurement;

        protected IEnumerable<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> vehiclesForVehicleID;


        protected int vehiclesForVehicleIDCount;
        protected VehicleMonitoringSystem.Server.Models.ConData.Vehicle vehiclesForVehicleIDValue;
        protected async Task vehiclesForVehicleIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await ConDataService.GetVehicles(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Make, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                vehiclesForVehicleID = result.Value.AsODataEnumerable();
                vehiclesForVehicleIDCount = result.Count;

                if (!object.Equals(speedMeasurement.VehicleID, null))
                {
                    var valueResult = await ConDataService.GetVehicles(filter: $"VehicleID eq {speedMeasurement.VehicleID}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        vehiclesForVehicleIDValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Vehicle" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.UpdateSpeedMeasurement(speedId:SpeedID, speedMeasurement);
                DialogService.Close(speedMeasurement);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}