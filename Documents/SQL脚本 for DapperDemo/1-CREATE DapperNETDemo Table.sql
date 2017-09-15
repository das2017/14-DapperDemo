USE [DapperDemoDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DapperNETDemo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL CONSTRAINT [DF_DapperNETDemo_ParentID]  DEFAULT ((0)),
	[Name] [varchar](100) NOT NULL CONSTRAINT [DF_DapperNETDemo_Name]  DEFAULT (''),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_DapperNETDemo_ModifiedDate]  DEFAULT (getdate()),
	[Type] [int] NOT NULL CONSTRAINT [DF_DapperNETDemo_Type]  DEFAULT ((-1))
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


