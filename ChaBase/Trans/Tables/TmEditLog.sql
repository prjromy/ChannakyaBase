CREATE TABLE [Trans].[TmEditLog] (
    [EditID]     INT           IDENTITY (1, 1) NOT NULL,
    [Amount]     DECIMAL (18)  NULL,
    [Iaccno]     INT           NOT NULL,
    [Value]      NVARCHAR (50) NULL,
    [Tdate]      SMALLDATETIME NOT NULL,
    [Tno]        NUMERIC (18)  NULL,
    [BrnchID]    INT           NOT NULL,
    [PostedBy]   INT           NULL,
    [PostedOn]   DATETIME      NULL,
    [Status]     SMALLINT      NULL,
    [VerifiedOn] SMALLDATETIME NULL,
    [VerifiedBy] INT           NULL,
    [RejectedOn] SMALLDATETIME NULL,
    [RejectedBy] INT           NULL,
    CONSTRAINT [PK_TmEditLog] PRIMARY KEY CLUSTERED ([EditID] ASC)
);

