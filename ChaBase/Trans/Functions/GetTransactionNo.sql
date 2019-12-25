	create function [Trans].[GetTransactionNo]()
			returns int
			as
			
			 begin
			 declare @tno int
				 exec [Mast].[GetTransno] @tno out
				 return @tno
			 end