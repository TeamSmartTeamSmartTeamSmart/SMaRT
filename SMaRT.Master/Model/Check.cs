namespace SMaRT.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Check")]
    public partial class Check
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Check()
        {
            CheckAssignments = new HashSet<CheckAssignment>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CheckID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RevisionNR { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FromDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsNewest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckAssignment> CheckAssignments { get; set; }
    }
}
