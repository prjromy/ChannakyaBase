CREATE TABLE [fin].[APBookChked] (
    [ID]        INT           IDENTITY (1, 1) NOT NULL,
    [IAccNo]    INT           NULL,
    [ChkedOn]   SMALLDATETIME NULL,
    [ChkedTill] SMALLDATETIME NULL,
    [ChkedBy]   INT           NULL,
    CONSTRAINT [PK_APBookChked] PRIMARY KEY CLUSTERED ([ID] ASC)
);

