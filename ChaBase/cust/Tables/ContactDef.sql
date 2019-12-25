CREATE TABLE [cust].[ContactDef] (
    [CNotype] TINYINT       IDENTITY (1, 1) NOT NULL,
    [CNodesc] NVARCHAR (25) NOT NULL,
    [CNoabb]  CHAR (3)      NOT NULL,
    CONSTRAINT [PK_CustContDetail] PRIMARY KEY CLUSTERED ([CNotype] ASC)
);

