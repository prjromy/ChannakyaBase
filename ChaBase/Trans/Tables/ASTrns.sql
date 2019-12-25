CREATE TABLE [Trans].[ASTrns] (
    [tno]       NUMERIC (18)    NOT NULL,
    [IAccno]    INT             NOT NULL,
    [tdate]     SMALLDATETIME   NOT NULL,
    [notes]     NVARCHAR (1000) NOT NULL,
    [slpno]     INT             NULL,
    [dramt]     MONEY           NOT NULL,
    [cramt]     MONEY           NOT NULL,
    [ttype]     TINYINT         NOT NULL,
    [postedby]  INT             NOT NULL,
    [brnhno]    INT             NOT NULL,
    [VNO]       NUMERIC (18)    NULL,
    [IsSlp]     BIT             NULL,
    [IsDeleted] BIT             NULL,
    [PostedOn]  DATETIME        CONSTRAINT [DF_ASTrns_PostedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ASTrns] PRIMARY KEY CLUSTERED ([tno] ASC)
);

