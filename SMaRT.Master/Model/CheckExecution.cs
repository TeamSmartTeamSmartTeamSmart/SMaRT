namespace SMaRT.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CheckExecution")]
    public partial class CheckExecution
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CheckID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CheckRevisionNR { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AssignedEntityID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AssignedEntityRevisionNR { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AssignmentRevisionNR { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExecutedEntityID { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ExecutedEntityRevisionNR { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "datetime2")]
        public DateTime InstructionTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EndTime { get; set; }

        public int ReturnCode { get; set; }

        public string Output { get; set; }

        public string Error { get; set; }

        public virtual CheckableEntity CheckableEntity { get; set; }

        public virtual CheckAssignment CheckAssignment { get; set; }
    }
}
