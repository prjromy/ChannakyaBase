CREATE TABLE [LG].[UserClaim] (
    [UserClaimId] INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NOT NULL,
    [ClaimType]   NVARCHAR (MAX) NULL,
    [ClaimValue]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.UserClaim] PRIMARY KEY CLUSTERED ([UserClaimId] ASC)
);

