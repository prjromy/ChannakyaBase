/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (1, N'Cash(Own branch withdraw/deposit with cheque or slip)')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (2, N'Cash(Different branch cash deposit/withdraw by slip or cheque)')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (3, N'Non-Account Related Cash')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (5, N'Account Related Non-Cash')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (6, N'Voucher Deposit')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (10, N'Auto payment for loan from deposit a/c')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (21, N'Amount send to already existing user but not accepted yet.')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (22, N'Amount  Accepted.')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (23, N'Amount Rejected By Receiver')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (24, N'Unverified RejectedCashflow')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (41, N'Disbursement With cash teller unverified.')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (42, N'Disbursement With Teller Verified')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (45, N'Disburse With Bank')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (50, N'Unverified Internal cheque deposit')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (51, N'Verified Internal Cheque Deposit')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (60, N'Loan After Auto Payment , Its is used to eliminate the TranLnk Table')
GO
INSERT [Trans].[TType] ([Id], [TtypeDesc]) VALUES (61, N'Unverified Rejected Cheque Good For Payment')
GO
