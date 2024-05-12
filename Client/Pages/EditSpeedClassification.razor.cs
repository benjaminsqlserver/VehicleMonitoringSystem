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
    public partial class EditSpeedClassification
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
        public short SpeedClassificationID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            speedClassification = await ConDataService.GetSpeedClassificationBySpeedClassificationId(speedClassificationId:SpeedClassificationID);
        }
        protected bool errorVisible;
        protected VehicleMonitoringSystem.Server.Models.ConData.SpeedClassification speedClassification;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await ConDataService.UpdateSpeedClassification(speedClassificationId:SpeedClassificationID, speedClassification);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(speedClassification);
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


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            speedClassification = await ConDataService.GetSpeedClassificationBySpeedClassificationId(speedClassificationId:SpeedClassificationID);
        }
    }
}