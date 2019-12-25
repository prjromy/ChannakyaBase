CREATE TABLE [fin].[CashFlow] (
    [Brhid]      INT            NOT NULL,
    [FUserid]    INT            NULL,
    [UserID]     INT            NULL,
    [TType]      TINYINT        NOT NULL,
    [TDate]      SMALLDATETIME  NOT NULL,
    [Tdesc]      NVARCHAR (150) NULL,
    [TNO]        BIGINT         NOT NULL,
    [Currid]     SMALLINT       NOT NULL,
    [Dramt]      MONEY          NOT NULL,
    [Cramt]      MONEY          NOT NULL,
    [AcceptedOn] DATETIME       NULL,
    [Status]     TINYINT        NULL,
    [Vno]        NUMERIC (18)   NULL,
    CONSTRAINT [PK__CashFlow__C4553DD577ED2A99] PRIMARY KEY CLUSTERED ([TNO] ASC)
);

