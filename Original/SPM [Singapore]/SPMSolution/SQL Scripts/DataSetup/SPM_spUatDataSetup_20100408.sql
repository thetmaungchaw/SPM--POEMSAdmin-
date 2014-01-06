USE [SPM]
GO
/****** Object:  StoredProcedure [dbo].[SPM_spUatDataSetup_20100408]    Script Date: 04/08/2010 12:11:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SPM_spUatDataSetup_20100408]
AS
/* SPM Phase II UAT Data setup scripts.
* Created by Li Qun. 08/04/2010
*/

-- load function url records
insert into FunctionURLNew  select 'AccessManagement','Access Management','WebPages/AccessControl/AccessManagement.aspx','Access Control'
insert into FunctionURLNew  select 'AEGpMatch','Cross Team Setup','WebPages/Setup/CrossTeamSetup.aspx','Setup'
insert into FunctionURLNew  select 'AssignHist','Assignment History','WebPages/AssignmentManagement/AssignmentHistory.aspx','Assignment Management'
insert into FunctionURLNew  select 'AssignInfo','Assigned Client Info','WebPages/AssignmentManagement/AssignedClientInfo.aspx','Assignment Management'
insert into FunctionURLNew  select 'CallRpt','Call Report','WebPages/ContactManagement/CallReport.aspx','Contact Management'
insert into FunctionURLNew  select 'ClientAnalysis','Client Analysis','WebPages/ContactManagement/ClientAnalysis.aspx','Contact Management'
insert into FunctionURLNew  select 'ClientAssignCross','Cross Team Assignment','WebPages/AssignmentManagement/ClientAssignLite.aspx?type=cross','Assignment Management'
insert into FunctionURLNew  select 'ClientAssignLite','Assignment','WebPages/AssignmentManagement/ClientAssignLite.aspx','Assignment Management'
insert into FunctionURLNew  select 'ContactAnalysis','Contact Analysis','WebPages/ContactManagement/ContactAnalysis.aspx','Contact Management'
insert into FunctionURLNew  select 'ContactEntry','Contact Entry','WebPages/ContactManagement/ContactEntry.aspx','Contact Management'
insert into FunctionURLNew  select 'ContactEntryAdmin','Contact Entry Admin','WebPages/ContactManagement/ContactEntry.aspx?type=admin','Contact Management'
insert into FunctionURLNew  select 'ContactHist','Contact History','WebPages/ContactManagement/ContactHistory.aspx','Contact Management'
insert into FunctionURLNew  select 'CoreClient','Core Client Setup','WebPages/Setup/CoreClient.aspx','Setup'
insert into FunctionURLNew  select 'DealerShortKey','Client Short Key Setup','WebPages/Setup/ClientShortKey.aspx','Setup'
insert into FunctionURLNew  select 'Management','Dealer Management','WebPages/Setup/DealerManagement.aspx','Setup'
insert into FunctionURLNew  select 'PreferList','Preference List Setup','WebPages/Setup/PreferList.aspx','Setup'

-- Set cutoff date of existing assignment to 7 days after the assignment date
update clientassign set modifieduser='migration', modifieddate=getdate(),
cutoffdate = dateadd(day, 7, assigndate)
where cutoffdate is null

-- Set cqdemo1 user access rights
insert into AccessRight  select 'cqdemo1','SP','AccessManagement','A'
insert into AccessRight  select 'cqdemo1','SP','AEGpMatch','A'
insert into AccessRight  select 'cqdemo1','SP','AssignHist','A'
insert into AccessRight  select 'cqdemo1','SP','AssignInfo','A'
insert into AccessRight  select 'cqdemo1','SP','CallRpt','A'
insert into AccessRight  select 'cqdemo1','SP','ClientAnalysis','A'
insert into AccessRight  select 'cqdemo1','SP','ClientAssignCross','A'
insert into AccessRight  select 'cqdemo1','SP','ClientAssignLite','A'
insert into AccessRight  select 'cqdemo1','SP','ContactAnalysis','A'
insert into AccessRight  select 'cqdemo1','SP','ContactEntry','A'
insert into AccessRight  select 'cqdemo1','SP','ContactEntryAdmin','A'
insert into AccessRight  select 'cqdemo1','SP','ContactHist','A'
insert into AccessRight  select 'cqdemo1','SP','CoreClient','A'
insert into AccessRight  select 'cqdemo1','SP','DealerShortKey','A'
insert into AccessRight  select 'cqdemo1','SP','Management','A'
insert into AccessRight  select 'cqdemo1','SP','PreferList','A'
