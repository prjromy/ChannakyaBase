CREATE TABLE [dbo].[HolidaysInYear] (
    [HolidayId]   INT            IDENTITY (1, 1) NOT NULL,
    [YearId]      INT            NOT NULL,
    [MonthId]     INT            NOT NULL,
    [Holiday]     INT            NOT NULL,
    [Description] NVARCHAR (500) NULL,
    CONSTRAINT [PK_HolidaysInYear] PRIMARY KEY CLUSTERED ([HolidayId] ASC)
);

