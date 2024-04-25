using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using VehicleMonitoringSystem.Server.Data;

namespace VehicleMonitoringSystem.Server
{
    public partial class ConDataService
    {
        ConDataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportGpsDataToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/gpsdata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/gpsdata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportGpsDataToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/gpsdata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/gpsdata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGpsDataRead(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> items);

        public async Task<IQueryable<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum>> GetGpsData(Query query = null)
        {
            var items = Context.GpsData.AsQueryable();

            items = items.Include(i => i.Vehicle);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnGpsDataRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnGpsDatumGet(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);
        partial void OnGetGpsDatumByGpsdataId(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> items);


        public async Task<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> GetGpsDatumByGpsdataId(long gpsdataid)
        {
            var items = Context.GpsData
                              .AsNoTracking()
                              .Where(i => i.GPSDataID == gpsdataid);

            items = items.Include(i => i.Vehicle);
 
            OnGetGpsDatumByGpsdataId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnGpsDatumGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnGpsDatumCreated(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);
        partial void OnAfterGpsDatumCreated(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> CreateGpsDatum(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum gpsdatum)
        {
            OnGpsDatumCreated(gpsdatum);

            var existingItem = Context.GpsData
                              .Where(i => i.GPSDataID == gpsdatum.GPSDataID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.GpsData.Add(gpsdatum);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(gpsdatum).State = EntityState.Detached;
                throw;
            }

            OnAfterGpsDatumCreated(gpsdatum);

            return gpsdatum;
        }

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> CancelGpsDatumChanges(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnGpsDatumUpdated(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);
        partial void OnAfterGpsDatumUpdated(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> UpdateGpsDatum(long gpsdataid, VehicleMonitoringSystem.Server.Models.ConData.GpsDatum gpsdatum)
        {
            OnGpsDatumUpdated(gpsdatum);

            var itemToUpdate = Context.GpsData
                              .Where(i => i.GPSDataID == gpsdatum.GPSDataID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(gpsdatum);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterGpsDatumUpdated(gpsdatum);

            return gpsdatum;
        }

        partial void OnGpsDatumDeleted(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);
        partial void OnAfterGpsDatumDeleted(VehicleMonitoringSystem.Server.Models.ConData.GpsDatum item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.GpsDatum> DeleteGpsDatum(long gpsdataid)
        {
            var itemToDelete = Context.GpsData
                              .Where(i => i.GPSDataID == gpsdataid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnGpsDatumDeleted(itemToDelete);


            Context.GpsData.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterGpsDatumDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSpeedMeasurementsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/speedmeasurements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/speedmeasurements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSpeedMeasurementsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/speedmeasurements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/speedmeasurements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSpeedMeasurementsRead(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> items);

        public async Task<IQueryable<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement>> GetSpeedMeasurements(Query query = null)
        {
            var items = Context.SpeedMeasurements.AsQueryable();

            items = items.Include(i => i.Vehicle);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSpeedMeasurementsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSpeedMeasurementGet(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);
        partial void OnGetSpeedMeasurementBySpeedId(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> items);


        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> GetSpeedMeasurementBySpeedId(long speedid)
        {
            var items = Context.SpeedMeasurements
                              .AsNoTracking()
                              .Where(i => i.SpeedID == speedid);

            items = items.Include(i => i.Vehicle);
 
            OnGetSpeedMeasurementBySpeedId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSpeedMeasurementGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSpeedMeasurementCreated(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);
        partial void OnAfterSpeedMeasurementCreated(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> CreateSpeedMeasurement(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement speedmeasurement)
        {
            OnSpeedMeasurementCreated(speedmeasurement);

            var existingItem = Context.SpeedMeasurements
                              .Where(i => i.SpeedID == speedmeasurement.SpeedID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.SpeedMeasurements.Add(speedmeasurement);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(speedmeasurement).State = EntityState.Detached;
                throw;
            }

            OnAfterSpeedMeasurementCreated(speedmeasurement);

            return speedmeasurement;
        }

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> CancelSpeedMeasurementChanges(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSpeedMeasurementUpdated(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);
        partial void OnAfterSpeedMeasurementUpdated(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> UpdateSpeedMeasurement(long speedid, VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement speedmeasurement)
        {
            OnSpeedMeasurementUpdated(speedmeasurement);

            var itemToUpdate = Context.SpeedMeasurements
                              .Where(i => i.SpeedID == speedmeasurement.SpeedID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(speedmeasurement);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSpeedMeasurementUpdated(speedmeasurement);

            return speedmeasurement;
        }

        partial void OnSpeedMeasurementDeleted(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);
        partial void OnAfterSpeedMeasurementDeleted(VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.SpeedMeasurement> DeleteSpeedMeasurement(long speedid)
        {
            var itemToDelete = Context.SpeedMeasurements
                              .Where(i => i.SpeedID == speedid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSpeedMeasurementDeleted(itemToDelete);


            Context.SpeedMeasurements.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSpeedMeasurementDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportVehiclesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/vehicles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/vehicles/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVehiclesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/vehicles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/vehicles/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVehiclesRead(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> items);

        public async Task<IQueryable<VehicleMonitoringSystem.Server.Models.ConData.Vehicle>> GetVehicles(Query query = null)
        {
            var items = Context.Vehicles.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnVehiclesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnVehicleGet(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);
        partial void OnGetVehicleByVehicleId(ref IQueryable<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> items);


        public async Task<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> GetVehicleByVehicleId(long vehicleid)
        {
            var items = Context.Vehicles
                              .AsNoTracking()
                              .Where(i => i.VehicleID == vehicleid);

 
            OnGetVehicleByVehicleId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnVehicleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVehicleCreated(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);
        partial void OnAfterVehicleCreated(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> CreateVehicle(VehicleMonitoringSystem.Server.Models.ConData.Vehicle vehicle)
        {
            OnVehicleCreated(vehicle);

            var existingItem = Context.Vehicles
                              .Where(i => i.VehicleID == vehicle.VehicleID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Vehicles.Add(vehicle);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(vehicle).State = EntityState.Detached;
                throw;
            }

            OnAfterVehicleCreated(vehicle);

            return vehicle;
        }

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> CancelVehicleChanges(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVehicleUpdated(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);
        partial void OnAfterVehicleUpdated(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> UpdateVehicle(long vehicleid, VehicleMonitoringSystem.Server.Models.ConData.Vehicle vehicle)
        {
            OnVehicleUpdated(vehicle);

            var itemToUpdate = Context.Vehicles
                              .Where(i => i.VehicleID == vehicle.VehicleID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(vehicle);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterVehicleUpdated(vehicle);

            return vehicle;
        }

        partial void OnVehicleDeleted(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);
        partial void OnAfterVehicleDeleted(VehicleMonitoringSystem.Server.Models.ConData.Vehicle item);

        public async Task<VehicleMonitoringSystem.Server.Models.ConData.Vehicle> DeleteVehicle(long vehicleid)
        {
            var itemToDelete = Context.Vehicles
                              .Where(i => i.VehicleID == vehicleid)
                              .Include(i => i.GpsData)
                              .Include(i => i.SpeedMeasurements)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnVehicleDeleted(itemToDelete);


            Context.Vehicles.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterVehicleDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}