CREATE TABLE [fin].[Aclosed] (
    [Iaccno]  INT           NOT NULL,
    [Vdate]   SMALLDATETIME NOT NULL,
    [postby]  INT           NULL,
    [Remarks] NVARCHAR (50) NULL,
    CONSTRAINT [PK__Aclosed__B781A13DBC4B39D7] PRIMARY KEY CLUSTERED ([Iaccno] ASC)
);

