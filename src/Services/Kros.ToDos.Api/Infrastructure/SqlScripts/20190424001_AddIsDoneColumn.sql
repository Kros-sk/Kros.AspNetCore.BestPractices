﻿SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[ToDos]
ADD [IsDone] [BIT];

GO

UPDATE [ToDos] SET [IsDone] = 0

GO