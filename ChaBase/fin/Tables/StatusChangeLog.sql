CREATE TABLE [fin].[StatusChangeLog] (
    [LogStatusID] INT           IDENTITY (1, 1) NOT NULL,
    [IAccNo]      BIGINT        NOT NULL,
    [TDate]       SMALLDATETIME NOT NULL,
    [UserID]      SMALLINT      NOT NULL,
    [AccState]    SMALLINT      NOT NULL,
    [ChangeOn]    DATETIME      CONSTRAINT [DF_StatusChangeLog_ChangeOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_StatusChangeLog] PRIMARY KEY CLUSTERED ([LogStatusID] ASC)
);

