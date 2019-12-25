CREATE TABLE [fin].[NCollateralDetail] (
    [NCId]       INT           NOT NULL,
    [Collateral] NVARCHAR (50) NOT NULL,
    [NrbNo]      NVARCHAR (20) NOT NULL,
    [pNCId]      INT           NULL,
    [nodeType]   BIT           NOT NULL,
    [IsActive]   BIT           NOT NULL,
    [Alias]      NVARCHAR (50) NOT NULL,
    [hId]        NVARCHAR (50) NULL,
    [Pid]        INT           NULL,
    CONSTRAINT [PK_NCollateralDetail] PRIMARY KEY CLUSTERED ([NCId] ASC)
);

