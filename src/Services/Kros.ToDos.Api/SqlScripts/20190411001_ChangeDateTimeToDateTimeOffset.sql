SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[ToDos]
ADD [CreatedNew] [DATETIMEOFFSET];

GO

UPDATE [ToDos] SET [CreatedNew] = CONVERT(DATETIMEOFFSET, CONVERT(VARCHAR, [Created], 120) + RIGHT(CONVERT(VARCHAR, SYSDATETIMEOFFSET(), 120), 6), 120);
GO

ALTER TABLE [dbo].[ToDos] DROP COLUMN [Created];
GO

EXEC sys.sp_rename @objname = N'dbo.ToDos.CreatedNew',
    @newname = 'Created',
    @objtype = 'COLUMN'