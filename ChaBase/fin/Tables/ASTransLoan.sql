CREATE TABLE [fin].[ASTransLoan] (
    [AstransLoanId] DECIMAL (18) IDENTITY (1, 1) NOT NULL,
    [tno]           NUMERIC (18) NOT NULL,
    [PId]           TINYINT      NOT NULL,
    [Amt]           MONEY        NOT NULL,
    [vNO]           NUMERIC (18) NULL,
    CONSTRAINT [PK_ASTransLoan] PRIMARY KEY CLUSTERED ([AstransLoanId] ASC)
);

