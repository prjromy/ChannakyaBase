CREATE TABLE [acc].[UserVSVoucher] (
    [ID]      INT IDENTITY (1, 1) NOT NULL,
    [UserId]  INT NOT NULL,
    [VTypeId] INT NOT NULL,
    [BatchId] INT NOT NULL,
    CONSTRAINT [PK_UserVSVoucher] PRIMARY KEY CLUSTERED ([ID] ASC)
);

