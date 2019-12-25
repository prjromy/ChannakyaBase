CREATE TABLE [fin].[ShrReg] (
    [CId]              NUMERIC (10)  NOT NULL,
    [RegNo]            NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Rdate]            SMALLDATETIME NULL,
    [PostedBy]         INT           NOT NULL,
    [ApprovedBy]       INT           NOT NULL,
    [RegistrationCode] VARCHAR (50)  CONSTRAINT [DF_ShrReg_RegistrationCode] DEFAULT ('Reg-00001') NOT NULL,
    [PostedOn]         DATETIME      CONSTRAINT [DF_ShrReg_PostedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_ShrReg] PRIMARY KEY CLUSTERED ([RegNo] ASC)
);

