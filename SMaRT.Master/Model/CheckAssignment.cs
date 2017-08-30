namespace SMaRT.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CheckAssignment")]
    public partial class CheckAssignment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CheckAssignment()
        {
            CheckExecutions = new HashSet<CheckExecution>();
        }

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
        public int EntityID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EntityRevisionNR { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RevisionNR { get; set; }

        public int Interval { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "xml")]
        public string Parameters { get; set; }

        public bool IsActive { get; set; }

        public bool IsNewest { get; set; }

        public virtual Check Check { get; set; }

        public virtual CheckableEntity CheckableEntity { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckExecution> CheckExecutions { get; set; }
    }
}
