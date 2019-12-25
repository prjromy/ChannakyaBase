CREATE TABLE [fin].[ChargeDetail] (
    [ChgID]       INT            IDENTITY (1, 1) NOT NULL,
    [ChgName]     NVARCHAR (100) NOT NULL,
    [FID]         INT            NOT NULL,
    [Modules]     TINYINT        NOT NULL,
    [Product]     VARCHAR (MAX)  NULL,
    [ModEveID]    INT            NULL,
    [Triggertype] TINYINT        NOT NULL,
    [ChrType]     TINYINT        NOT NULL,
    [Ratio]       REAL           NULL,
    [CAmount]     MONEY          NULL,
    [ChrTempID]   INT            NULL,
    [Chngble]     BIT            NOT NULL,
    [ChrDirect]   BIT            NOT NULL,
    [Status]      BIT            NOT NULL,
    CONSTRAINT [PK_ChargeDetail] PRIMARY KEY CLUSTERED ([ChgID] ASC),
    CONSTRAINT [FK_ChargeDetail_FinAcc] FOREIGN KEY ([ModEveID]) REFERENCES [Mast].[Events] ([EventId]),
    CONSTRAINT [FK_ChargeDetail_Modules] FOREIGN KEY ([Modules]) REFERENCES [Mast].[Modules] ([Modid])
);

