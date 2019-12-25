CREATE TABLE [fin].[AMTransLoan] (
    [AmtransLoanId] DECIMAL (18)    IDENTITY (1, 1) NOT NULL,
    [tno]           NUMERIC (18)    NOT NULL,
    [Pid]           TINYINT         NOT NULL,
    [Amt]           NUMERIC (19, 2) NOT NULL,
    [Vno]           NUMERIC (18)    NULL,
    CONSTRAINT [PK_AMTransLoan] PRIMARY KEY CLUSTERED ([AmtransLoanId] ASC)
);

