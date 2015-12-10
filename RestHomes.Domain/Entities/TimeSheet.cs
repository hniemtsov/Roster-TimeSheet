namespace RestHomes.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TimeSheet")]
    public partial class TimeSheet
    {
        [Key]
        [Column(Order = 0, TypeName = "date")]
        public DateTime Day { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDw { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDp { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDs { get; set; }

        public double? Hours { get; set; }

        public bool? isSigned { get; set; }

        public virtual Position Position { get; set; }

        public virtual Shift Shift { get; set; }

        public virtual Worker Worker { get; set; }
    }
}
