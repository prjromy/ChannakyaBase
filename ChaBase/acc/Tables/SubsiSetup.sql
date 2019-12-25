CREATE TABLE [acc].[SubsiSetup] (
    [SSId]      INT          IDENTITY (1, 1) NOT NULL,
    [STBId]     INT          NOT NULL,
    [Title]     VARCHAR (50) NOT NULL,
    [Prefix]    VARCHAR (50) NOT NULL,
    [Length]    INT          NULL,
    [CurrentNo] INT          NULL,
    CONSTRAINT [PK_SubsiSetup] PRIMARY KEY CLUSTERED ([SSId] ASC)
);

