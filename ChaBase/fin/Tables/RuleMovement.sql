﻿CREATE TABLE [fin].[RuleMovement] (
    [MID]   TINYINT      IDENTITY (1, 1) NOT NULL,
    [MRule] VARCHAR (15) NOT NULL,
    CONSTRAINT [PK_RuleMovement] PRIMARY KEY CLUSTERED ([MID] ASC)
);

