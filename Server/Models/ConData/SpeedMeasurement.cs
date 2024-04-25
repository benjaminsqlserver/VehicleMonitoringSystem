using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleMonitoringSystem.Server.Models.ConData
{
    [Table("SpeedMeasurements", Schema = "dbo")]
    public partial class SpeedMeasurement
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
        public long SpeedID { get; set; }

        [ConcurrencyCheck]
        public long? VehicleID { get; set; }

        public Vehicle Vehicle { get; set; }

        [ConcurrencyCheck]
        public decimal? SpeedInKmPerHour { get; set; }

        [ConcurrencyCheck]
        public DateTime? DateAndTimeInserted { get; set; }

    }
}