using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleMonitoringSystem.Server.Models.ConData
{
    [Table("GPSData", Schema = "dbo")]
    public partial class GpsDatum
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
        public long GPSDataID { get; set; }

        [ConcurrencyCheck]
        public long? VehicleID { get; set; }

        public Vehicle Vehicle { get; set; }

        [ConcurrencyCheck]
        public decimal? Latitude { get; set; }

        [ConcurrencyCheck]
        public decimal? Longitude { get; set; }

        [ConcurrencyCheck]
        public DateTime? DateAndTimeInserted { get; set; }

    }
}