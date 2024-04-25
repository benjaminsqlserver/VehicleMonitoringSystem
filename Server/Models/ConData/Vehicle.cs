using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleMonitoringSystem.Server.Models.ConData
{
    [Table("Vehicle", Schema = "dbo")]
    public partial class Vehicle
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VehicleID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Make { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Model { get; set; }

        [ConcurrencyCheck]
        public int? Year { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string LicensePlate { get; set; }

        [ConcurrencyCheck]
        public string VIN { get; set; }

        [ConcurrencyCheck]
        public string Color { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string OwnerName { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string OwnerContactAddress { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string OwnerEmail { get; set; }

        public ICollection<GpsDatum> GpsData { get; set; }

        public ICollection<SpeedMeasurement> SpeedMeasurements { get; set; }

    }
}