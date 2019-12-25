CREATE TABLE [fin].[ALColl] (
    [Sno]           INT            IDENTITY (1, 1) NOT NULL,
    [IAccno]        INT            NOT NULL,
    [NCID]          INT            NOT NULL,
    [CValue]        MONEY          NOT NULL,
    [Weightage]     FLOAT (53)     NULL,
    [OName]         NVARCHAR (100) NULL,
    [OAdd]          NVARCHAR (75)  NULL,
    [contactNo]     NVARCHAR (50)  NULL,
    [citizenshipNo] NVARCHAR (30)  NULL,
    [RegNo]         NVARCHAR (30)  NOT NULL,
    [RegDate]       DATETIME       NULL,
    [Remarks]       NVARCHAR (150) NULL,
    [IsAc]          BIT            NOT NULL,
    [IsActive]      BIT            NOT NULL,
    [PostedBy]      INT            NOT NULL,
    [PostedOn]      DATETIME       NOT NULL,
    CONSTRAINT [PK_ALColl] PRIMARY KEY CLUSTERED ([Sno] ASC),
    CONSTRAINT [FK_ALColl_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno]),
    CONSTRAINT [FK_ALColl_NCollateralDetail] FOREIGN KEY ([NCID]) REFERENCES [fin].[NCollateralDetail] ([NCId])
);

