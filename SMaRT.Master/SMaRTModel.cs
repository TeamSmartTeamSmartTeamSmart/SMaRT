namespace SMaRT.Master
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SMaRTModel : DbContext
    {
        public SMaRTModel()
            : base("name=SMaRTEntities")
        {
        }

        public virtual DbSet<Check> Checks { get; set; }
        public virtual DbSet<CheckableEntity> CheckableEntities { get; set; }
        public virtual DbSet<CheckAssignment> CheckAssignments { get; set; }
        public virtual DbSet<CheckExecution> CheckExecutions { get; set; }
        public virtual DbSet<GroupMembership> GroupMemberships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Check>()
                .HasMany(e => e.CheckAssignments)
                .WithRequired(e => e.Check)
                .HasForeignKey(e => new { e.CheckID, e.CheckRevisionNR })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CheckableEntity>()
                .HasMany(e => e.CheckAssignments)
                .WithRequired(e => e.CheckableEntity)
                .HasForeignKey(e => new { e.EntityID, e.EntityRevisionNR })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CheckableEntity>()
                .HasMany(e => e.CheckExecutions)
                .WithRequired(e => e.CheckableEntity)
                .HasForeignKey(e => new { e.ExecutedEntityID, e.ExecutedEntityRevisionNR })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CheckableEntity>()
                .HasMany(e => e.ChildOf)
                .WithRequired(e => e.Child)
                .HasForeignKey(e => new { e.ChildID, e.ChildRevisionNR })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CheckableEntity>()
                .HasMany(e => e.ParentOf)
                .WithRequired(e => e.Parent)
                .HasForeignKey(e => new { e.ParentID, e.ParentRevisionNR })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CheckAssignment>()
                .HasMany(e => e.CheckExecutions)
                .WithRequired(e => e.CheckAssignment)
                .HasForeignKey(e => new { e.CheckID, e.CheckRevisionNR, e.AssignedEntityID, e.AssignedEntityRevisionNR, e.AssignmentRevisionNR })
                .WillCascadeOnDelete(false);
        }
    }
}
