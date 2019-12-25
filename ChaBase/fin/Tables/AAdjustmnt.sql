CREATE TABLE [fin].[AAdjustmnt] (
    [AAdjustmentID] INT            IDENTITY (1, 1) NOT NULL,
    [Iaccno]        INT            NOT NULL,
    [Pid]           TINYINT        NOT NULL,
    [Amt]           MONEY          NOT NULL,
    [TNo]           INT            NULL,
    [UserId]        INT            NULL,
    [TDate]         SMALLDATETIME  NULL,
    [note]          NVARCHAR (500) NULL,
    [valued]        INT            NULL,
    CONSTRAINT [PK_AAdjustmnt] PRIMARY KEY CLUSTERED ([AAdjustmentID] ASC)
);

