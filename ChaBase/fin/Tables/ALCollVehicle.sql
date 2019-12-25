CREATE TABLE [fin].[ALCollVehicle] (
    [ColVechicleId] INT            IDENTITY (1, 1) NOT NULL,
    [Sno]           INT            NOT NULL,
    [LicNo]         NVARCHAR (15)  NULL,
    [LicRight]      NVARCHAR (30)  NULL,
    [IssueOff]      NVARCHAR (100) NULL,
    [ModNo]         NVARCHAR (30)  NULL,
    [ChassisNo]     NVARCHAR (40)  NULL,
    [EngNo]         NVARCHAR (40)  NULL,
    [Color]         NVARCHAR (20)  NULL,
    [CC]            NVARCHAR (20)  NULL,
    [MC]            NVARCHAR (40)  NULL,
    [MD]            NVARCHAR (40)  NULL,
    [type]          TINYINT        NOT NULL,
    [description]   VARCHAR (100)  NULL,
    [VehicleNo]     VARCHAR (30)   NULL,
    CONSTRAINT [PK_ALCollVehicle] PRIMARY KEY CLUSTERED ([ColVechicleId] ASC),
    CONSTRAINT [FK_ALCollVehicle_ALColl] FOREIGN KEY ([Sno]) REFERENCES [fin].[ALColl] ([Sno])
);

