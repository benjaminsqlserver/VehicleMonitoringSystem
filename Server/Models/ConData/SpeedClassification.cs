using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VehicleMonitoringSystem.Server.Models.ConData
{
    [Table("SpeedClassifications", Schema = "dbo")]
    public partial class SpeedClassification
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
        public short SpeedClassificationID { get; set; }

        [ConcurrencyCheck]
        public int? LowerLimitInKmPerHour { get; set; }

        [ConcurrencyCheck]
        public int? UpperLimitInKmPerHour { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Description { get; set; }

    }
}