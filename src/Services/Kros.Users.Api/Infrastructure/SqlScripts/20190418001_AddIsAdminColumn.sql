﻿SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Users]
ADD [IsAdmin] [bit] NOT NULL;