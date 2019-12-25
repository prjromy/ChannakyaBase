CREATE TABLE [acc].[FinSys2] (
    [F2id]    INT             IDENTITY (1, 1) NOT NULL,
    [F1id]    INT             NOT NULL,
    [F2Desc]  VARCHAR (50)    NOT NULL,
    [Pid]     INT             NULL,
    [F1Type]  INT             NOT NULL,
    [IsFixed] BIT             NOT NULL,
    [Count]   INT             NOT NULL,
    [Content] VARBINARY (MAX) NULL,
    CONSTRAINT [PK__FinSys2__9A3FCEF5C889656B] PRIMARY KEY CLUSTERED ([F2id] ASC),
    CONSTRAINT [FK_FinSys2_FinSys1] FOREIGN KEY ([F1id]) REFERENCES [acc].[FinSys1] ([F1id])
);

