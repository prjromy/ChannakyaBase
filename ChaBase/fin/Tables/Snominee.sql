CREATE TABLE [fin].[Snominee] (
    [SnomineeId]     INT            IDENTITY (1, 1) NOT NULL,
    [RegNo]          NUMERIC (18)   NOT NULL,
    [NomNam]         NVARCHAR (100) NOT NULL,
    [NomRel]         NVARCHAR (100) NOT NULL,
    [CCertID]        VARCHAR (20)   NULL,
    [CCertno]        VARCHAR (25)   NULL,
    [Share]          REAL           NULL,
    [ContactAddress] VARCHAR (250)  NULL,
    [ContactNo]      NVARCHAR (50)  NULL,
    CONSTRAINT [PK_Snominee] PRIMARY KEY CLUSTERED ([SnomineeId] ASC),
    CONSTRAINT [FK_Snominee_ShrReg] FOREIGN KEY ([RegNo]) REFERENCES [fin].[ShrReg] ([RegNo])
);

