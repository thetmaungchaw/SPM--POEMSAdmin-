USE [SPM]
GO
/****** Object:  StoredProcedure [dbo].[SPM_spProdDataSetup_20100408]    Script Date: 04/08/2010 11:53:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SPM_spProdDataSetup_20100408]
AS
/* SPM Phase II Production Data setup scripts.
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