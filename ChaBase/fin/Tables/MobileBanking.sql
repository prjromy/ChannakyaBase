CREATE TABLE [fin].[MobileBanking] (
    [Tno]                BIGINT        NOT NULL,
    [TDate]              DATE          NOT NULL,
    [IAccNO]             INT           NOT NULL,
    [TType]              INT           NOT NULL,
    [MobileNo]           VARCHAR (10)  NOT NULL,
    [Amount]             MONEY         NOT NULL,
    [PostedOn]           DATETIME      CONSTRAINT [DF_MobileBanking_PostedOn] DEFAULT (getdate()) NOT NULL,
    [Remarks]            VARCHAR (500) NULL,
    [SuccessOrFailureOn] DATETIME      NULL,
    [Vno]                INT           NULL,
    CONSTRAINT [PK_MobileBanking] PRIMARY KEY CLUSTERED ([Tno] ASC)
);

