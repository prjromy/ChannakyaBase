CREATE TABLE [fin].[ALRegistration] (
    [RegId]      INT           IDENTITY (1, 1) NOT NULL,
    [PID]        INT           NOT NULL,
    [LAmt]       MONEY         NOT NULL,
    [Duration]   INT           NOT NULL,
    [ADuration]  INT           NULL,
    [Status]     TINYINT       CONSTRAINT [DF_ALRegistration_Status] DEFAULT ((0)) NOT NULL,
    [RegDate]    DATETIME      NOT NULL,
    [SAmt]       MONEY         NULL,
    [Remarks]    NVARCHAR (50) NULL,
    [iAccno]     INT           NULL,
    [PAYSID]     INT           NULL,
    [PostedBy]   INT           NOT NULL,
    [ApprovedBy] INT           NULL,
    [ApprovedOn] DATETIME      NULL,
    CONSTRAINT [PK_ALRegistration] PRIMARY KEY CLUSTERED ([RegId] ASC),
    CONSTRAINT [FK_ALRegistration_ADetail] FOREIGN KEY ([iAccno]) REFERENCES [fin].[ADetail] ([IAccno]),
    CONSTRAINT [FK_ALRegistration_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID])
);

