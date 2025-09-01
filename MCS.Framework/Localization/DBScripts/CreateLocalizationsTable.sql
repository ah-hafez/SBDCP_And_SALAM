SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING OFF
GO
CREATE TABLE [Resources] (
		[ID]              int NOT NULL IDENTITY(1, 1),
		[ResourceId]      nvarchar(1024) NOT NULL,
		[Value]           nvarchar(max) NULL,
		[Culture]         nvarchar(10) NULL,
		[ResourceSet]     nvarchar(512) NULL,
		[Type]            nvarchar(512) NULL,
		[BinFile]         varbinary(max) NULL,
		[TextFile]        nvarchar(max) NULL,
		[Filename]        nvarchar(128) NULL,
        [Comment]         nvarchar(512) NULL
)
ON [PRIMARY]
GO
ALTER TABLE [Resources]
	ADD
	CONSTRAINT [PK_Resources]
	PRIMARY KEY
	([ID])
	ON [PRIMARY]
GO
ALTER TABLE [Resources]
	ADD
	CONSTRAINT [DF_Resources_Filename]
	DEFAULT ('') FOR [Filename]
GO
ALTER TABLE [Resources]
	ADD
	CONSTRAINT [DF_Resources_Culture]
	DEFAULT ('') FOR [Culture]
GO
ALTER TABLE [Resources]
	ADD
	CONSTRAINT [DF_Resources_PageId]
	DEFAULT ('') FOR [ResourceSet]
GO
ALTER TABLE [Resources]
	ADD
	CONSTRAINT [DF_Resources_Text]
	DEFAULT ('') FOR [Value]
GO
ALTER TABLE [Resources]
	ADD
	CONSTRAINT [DF_Resources_Type]
	DEFAULT ('') FOR [Type]
GO


GO