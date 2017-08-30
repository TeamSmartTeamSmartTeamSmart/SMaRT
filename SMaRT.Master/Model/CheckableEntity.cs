namespace SMaRT.Master
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CheckableEntity")]
    public partial class CheckableEntity
    {
        public CheckableEntity()
        {
            CheckAssignments = new HashSet<CheckAssignment>();
            CheckExecutions = new HashSet<CheckExecution>();
            ChildOf = new HashSet<GroupMembership>();
            ParentOf = new HashSet<GroupMembership>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EntityID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RevisionNR { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime FromDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsNewest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckAssignment> CheckAssignments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CheckExecution> CheckExecutions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMembership> ChildOf { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMembership> ParentOf { get; set; }
    }
}
