CREATE TABLE [fin].[AODIntReceivable] (
    [Tdate]      DATETIME       NOT NULL,
    [Iaccno]     INT            NOT NULL,
    [DRAmt]      MONEY          NOT NULL,
    [CRAmt]      MONEY          NOT NULL,
    [vno]        INT            NULL,
    [TType]      INT            NOT NULL,
    [PostedBy]   INT            NOT NULL,
    [VerifiedBy] INT            NULL,
    [Remarks]    NVARCHAR (100) NULL,
    [TNo]        NUMERIC (18)   NOT NULL
);

