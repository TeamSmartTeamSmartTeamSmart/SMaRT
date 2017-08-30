use SMaRT;

--Deletes
delete from CheckExecution;
delete from CheckAssignment;
delete from "Check";
delete from GroupMembership;
delete from CheckableEntity;


--Check
insert into "check" values(1, 1, 'Get operating system', 'param ([string]$out)
 write-output $out', 'Returns the type, name and version of the current running operating system', '2012-06-18 10:34:09', 1, 1);
insert into "check" values(2, 1, 'Find folder', 'param ([string]$out)
 write-output $out', 'Search a specific folder', '2013-06-12 9:39:01', 0, 0);
insert into "check" values(2, 2, 'Search object', 'param ([string]$out)
 write-output $out', 'Search a specific folder (zip or normal)', '2013-08-16 15:01:59', 0, 0);
insert into "check" values(2, 3, 'Search object', 'param ([string]$out)
 write-output $out', 'Looks for an specific object in the file tree', '2016-09-01 12:20:00', 1, 1);
insert into "check" values(3, 1, 'Free storage', 'param ([string]$out)
 write-output $out', null, '2015-01-30 10:34:09', 0, 0);
insert into "check" values(3, 2, 'Free storage', 'param ([string]$out)
 write-output $out', 'Returns the amount of (available) free storage', '2015-01-30 10:36:35', 1, 1);
insert into "check" values(4, 1, 'CPU Temperature', 'param ([string]$out)
 write-output $out', 'Returns the temperature of the CPU', '2016-12-15 06:01:00', 1, 1);
insert into "check" values(5, 1, 'Ping HTTP-Server', 'param ([string]$out)
 write-output $out', 'Try to reach the HTTP-Server and returns the ping time', '1998-06-24 08:59:59', 1, 1);
insert into "check" values(6, 1, 'Count of Oracle Users', 'param ([string]$out)
 write-output $out', 'Returns count of current logged in users on the oracle db', '2000-02-18 22:47:08', 1, 1);

--CheckableEntity
insert into CheckableEntity values(1, 1, 'PC01', null, '2012-06-18 10:34:09', 1, 1);
insert into CheckableEntity values(2, 1, 'PC02', 'Room 1.05', '2013-04-18 10:34:09', 0, 0);
insert into CheckableEntity values(2, 2, 'PC03', 'Room 25.06', '2015-06-15 10:34:09', 1, 1);
insert into CheckableEntity values(3, 1, 'PC04', 'Room 1.01', '2012-09-19 10:34:09', 1, 1);
insert into CheckableEntity values(4, 1, 'PC99', 'Foyer 3', '2011-01-18 9:34:09', 1, 1);
insert into CheckableEntity values(5, 1, 'Server01', null, '1980-12-24 22:01:00', 0, 0);
insert into CheckableEntity values(5, 2, 'IBM x3950 M2 7233', 'Serverroom S3', '1980-12-29 10:01:00', 0, 0);
insert into CheckableEntity values(5, 3, 'IBM x3950 M2 7233', 'taken out of service', '2016-12-18 07:29:22', 0, 1);
insert into CheckableEntity values(6, 1, 'All PCs', 'Group for all PCs', '2016-12-19 08:08:22', 1, 1);

--GroupMembership
insert into GroupMembership values(6, 1, 1, 1, 1, 1);
insert into GroupMembership values(6, 1, 2, 2, 1, 1);
insert into GroupMembership values(6, 1, 3, 1, 1, 1);
insert into GroupMembership values(6, 1, 4, 1, 1, 1);
insert into GroupMembership values(6, 1, 5, 1, 0, 0);
insert into GroupMembership values(6, 1, 5, 2, 0, 0);
insert into GroupMembership values(6, 1, 5, 3, 0, 1);

--CheckAssignment
insert into CheckAssignment values(4, 1, 6, 1, 1, 100, '2016-12-19 08:15:00', '<arguments><argument name="IP">192.168.0.123</argument><argument name="Port">8080</argument></arguments>', 1, 1);
insert into CheckAssignment values(1, 1, 6, 1, 1, 1000, '2016-12-19 08:15:00', '<arguments><argument name="out">test</argument></arguments>', 1, 1);
insert into CheckAssignment values(3, 1, 5, 1, 1, 100, '2016-12-19 08:15:00', '<arguments><argument name="out">test</argument></arguments>', 0, 0);
insert into CheckAssignment values(3, 1, 5, 2, 2, 100, '2016-12-19 08:15:00', '<arguments><argument name="out">test</argument></arguments>', 0, 0);
insert into CheckAssignment values(3, 1, 5, 3, 3, 100, '2016-12-19 08:15:00', '<arguments><argument name="out">test</argument></arguments>', 1, 1);
insert into CheckAssignment values(6, 1, 5, 3, 1, 50, '2016-12-19 08:15:00', '<arguments><argument name="out">test</argument></arguments>', 1, 1);
insert into CheckAssignment values(2, 3, 1, 1, 1, 600, '2016-12-19 08:15:00', '<arguments><argument name="out">test</argument></arguments>', 0, 1);

--CheckExecution
--insert into CheckExecution values(4, 1, 6, 1, 1, 6, 1, '2016-12-19 08:15:00', '2016-12-19 08:15:00', '2016-12-19 08:15:00', 1, 'ok', 'no error');
insert into CheckExecution values(4, 1, 6, 1, 1, 6, 1, '2016-12-19 08:15:00', '2016-12-19 08:15:00', '2016-12-19 08:15:00', 1, 'ok', 'no error');
insert into CheckExecution values(4, 1, 6, 1, 1, 6, 1, '2016-12-19 08:16:00', '2016-12-19 08:16:00', '2016-12-19 08:16:00', 1, 'ok', 'no error');
insert into CheckExecution values(4, 1, 6, 1, 1, 6, 1, '2016-12-19 08:17:00', '2016-12-19 08:17:00', '2016-12-19 08:17:00', 1, 'ok', 'no error');
insert into CheckExecution values(4, 1, 6, 1, 1, 6, 1, '2016-12-19 08:18:00', '2016-12-19 08:18:00', '2016-12-19 08:18:00', 1, 'ok', 'no error');
insert into CheckExecution values(4, 1, 6, 1, 1, 6, 1, '2016-12-19 08:19:00', '2016-12-19 08:19:00', '2016-12-19 08:19:00', 1, 'ok', 'no error');

select * from CheckAssignment;