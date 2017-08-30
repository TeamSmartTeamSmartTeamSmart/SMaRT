use master;
go

alter database SMaRT
set single_user with
rollback after 5;
go

IF  EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'SMaRT')
DROP DATABASE SMaRT;
go

create Database SMaRT;
go

use SMaRT;
go

create xml schema collection ArgumentsSchema
as
N'<?xml version="1.0" encoding="utf-16"?>
<xs:schema elementFormDefault="qualified"
    xmlns:mstns="http://novomatic.com/schemes/arguments.xsd"
    xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xs:element name="arguments">
    <xs:complexType>
      <xs:sequence>
        <xs:element name="argument" type="argument" minOccurs="0" maxOccurs="unbounded"/>
      </xs:sequence>
    </xs:complexType>
  </xs:element>

  <xs:complexType name="argument">
	<xs:simpleContent>
	  <xs:extension base="xs:string">
		<xs:attribute name="name" type="xs:string" use="required"/>
	  </xs:extension>
    </xs:simpleContent>
  </xs:complexType>
</xs:schema>';
go

-- tables
-- Table: Check
CREATE TABLE "Check" (
    CheckID int  NOT NULL,
    RevisionNR int  NOT NULL,
    Name nvarchar(64)  NOT NULL,
    Code nvarchar(max)  NOT NULL,
    Description nvarchar(max)  NULL,
    FromDate datetime2(7)  NOT NULL,
    IsActive bit  NOT NULL,
    IsNewest bit  NOT NULL,
    CONSTRAINT Check_pk PRIMARY KEY  (CheckID,RevisionNR)
);

-- Table: CheckAssignment
CREATE TABLE CheckAssignment (
    CheckID int  NOT NULL,
    CheckRevisionNR int  NOT NULL,
    EntityID int  NOT NULL,
    EntityRevisionNR int  NOT NULL, 
    RevisionNR int  NOT NULL,
    Interval int  NOT NULL,
    FromDate datetime2(7)  NOT NULL,
    Parameters xml (ArgumentsSchema)  NULL,
    IsActive bit  NOT NULL,
    IsNewest bit  NOT NULL,
    CONSTRAINT CheckAssignment_pk PRIMARY KEY  (CheckID,CheckRevisionNR,EntityID,EntityRevisionNR,RevisionNR)
);

-- Table: CheckExecution
CREATE TABLE CheckExecution (
    CheckID int  NOT NULL,
    CheckRevisionNR int  NOT NULL,
    AssignedEntityID int  NOT NULL,
    AssignedEntityRevisionNR int  NOT NULL,
    AssignmentRevisionNR int  NOT NULL,
    ExecutedEntityID int  NOT NULL,
    ExecutedEntityRevisionNR int  NOT NULL,
    InstructionTime datetime2(7)  NOT NULL,
    StartTime datetime2(7)  NULL,
    EndTime datetime2(7)  NULL,
    ReturnCode int  NOT NULL,
    Output nvarchar(max)  NULL,
    Error nvarchar(max)  NULL,
    CONSTRAINT CheckExecution_pk PRIMARY KEY  (CheckID,CheckRevisionNR,AssignedEntityID,AssignedEntityRevisionNR,AssignmentRevisionNR,ExecutedEntityID,ExecutedEntityRevisionNR,InstructionTime)
);

-- Table: CheckableEntity
CREATE TABLE CheckableEntity (
    EntityID int  NOT NULL,
    RevisionNR int  NOT NULL,
    Name nvarchar(64)  NOT NULL,
    Description nvarchar(max)  NULL,
    FromDate datetime2(7)  NOT NULL,
    IsActive bit  NOT NULL,
    IsNewest bit  NOT NULL,
    CONSTRAINT CheckableEntity_pk PRIMARY KEY  (EntityID,RevisionNR)
);

-- Table: GroupMembership
CREATE TABLE GroupMembership (
    ParentID int  NOT NULL,
    ParentRevisionNR int  NOT NULL,
    ChildID int  NOT NULL,
    ChildRevisionNR int  NOT NULL,
    IsActive bit  NOT NULL,
    IsNewest bit  NOT NULL,
    CONSTRAINT GroupMembership_pk PRIMARY KEY  (ParentID,ParentRevisionNR,ChildID,ChildRevisionNR)
);
go

-- foreign keys
-- Reference: CheckAssignment_Check (table: CheckAssignment)
ALTER TABLE CheckAssignment ADD CONSTRAINT CheckAssignment_Check
    FOREIGN KEY (CheckID,CheckRevisionNR)
    REFERENCES "Check" (CheckID,RevisionNR);

-- Reference: CheckExecution_CheckExecutionResult (table: CheckExecution)
ALTER TABLE CheckExecution ADD CONSTRAINT CheckExecution_CheckExecutionResult
    FOREIGN KEY (CheckID,CheckRevisionNR,AssignedEntityID,AssignedEntityRevisionNR,AssignmentRevisionNR)
    REFERENCES CheckAssignment (CheckID,CheckRevisionNR,EntityID,EntityRevisionNR,RevisionNR);

-- Reference: CheckExecution_CheckableEntity (table: CheckExecution)
ALTER TABLE CheckExecution ADD CONSTRAINT CheckExecution_CheckableEntity
    FOREIGN KEY (ExecutedEntityID,ExecutedEntityRevisionNR)
    REFERENCES CheckableEntity (EntityID,RevisionNR);

-- Reference: CheckableAssignment_CheckableEntity (table: CheckAssignment)
ALTER TABLE CheckAssignment ADD CONSTRAINT CheckableAssignment_CheckableEntity
    FOREIGN KEY (EntityID,EntityRevisionNR)
    REFERENCES CheckableEntity (EntityID,RevisionNR);

-- Reference: GroupMembership_CheckableEntity (table: GroupMembership)
ALTER TABLE GroupMembership ADD CONSTRAINT GroupMembership_CheckableEntity
    FOREIGN KEY (ChildID,ChildRevisionNR)
    REFERENCES CheckableEntity (EntityID,RevisionNR);

-- Reference: GroupMembership_Group (table: GroupMembership)
ALTER TABLE GroupMembership ADD CONSTRAINT GroupMembership_Group
    FOREIGN KEY (ParentID,ParentRevisionNR)
    REFERENCES CheckableEntity (EntityID,RevisionNR);
go

alter database SMaRT set multi_user
go