CREATE TABLE [fin].[ALoanTra] (
    [iaccno]   INT             NOT NULL,
    [tno]      NUMERIC (18)    NOT NULL,
    [vdate]    SMALLDATETIME   NOT NULL,
    [PriDr]    NUMERIC (38, 2) NULL,
    [OthrDr]   NUMERIC (38, 2) NULL,
    [PriCr]    NUMERIC (38, 2) NULL,
    [othrCr]   NUMERIC (38, 2) NULL,
    [RbtInt]   NUMERIC (38, 2) NULL,
    [RbtPen]   NUMERIC (38, 2) NULL,
    [intAPd]   NUMERIC (38, 2) NULL,
    [intRPd]   NUMERIC (38, 2) NULL,
    [IonIAPd]  NUMERIC (38, 2) NULL,
    [IonIRPd]  NUMERIC (38, 2) NULL,
    [IonCAPd]  NUMERIC (38, 2) NULL,
    [IonCRPd]  NUMERIC (38, 2) NULL,
    [PonPrAPd] NUMERIC (38, 2) NULL,
    [PonPrRPd] NUMERIC (38, 2) NULL,
    [PonIAPd]  NUMERIC (38, 2) NULL,
    [PonIRPd]  NUMERIC (38, 2) NULL,
    [ttype]    TINYINT         NOT NULL
);

