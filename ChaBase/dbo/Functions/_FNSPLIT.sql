CREATE FUNCTION [dbo].[_FNSPLIT]
(
	@STEXT VARCHAR(8000),
	@SDELIM VARCHAR(20) = ' '
)
RETURNS @RETARRAY TABLE (IDX SMALLINT PRIMARY KEY,VALUE VARCHAR(8000))
---MARKED ENCRYPTION 
As
Begin
	Declare @Idx Smallint,@Value Varchar(8000),@BContinue Bit,@iStrike SmallInt,@iDelimLength TinyInt

	If @sDelim = 'Space'
	Begin
		Set @sDelim = ' '
	End

	Set @idx = 0
	Set @sText = LTrim(RTrim(@sText))
	Set @iDelimlength = DATALENGTH(@sDelim)
	Set @bcontinue = 1

	If Not ((@iDelimlength = 0) or (@sDelim = 'Empty'))
	Begin
		While @bcontinue = 1
		Begin

			--If you can find the delimiter in the text, retrieve the first element and
			--insert it with its index into the return table.

			If CHARINDEX(@sDelim,@sText) > 0
			Begin
				Set @value = SUBSTRING(@sText,1, CHARINDEX(@sDelim,@sText)-1)
				Begin
					Insert @retArray (Idx,Value) Values (@Idx,@Value)
				End
				--Trim the element and its delimiter from the front of the string.
				--Increment the index and loop.
				Set @iStrike = DATALENGTH(@value) + @iDelimlength
				Set @idx = @idx + 1
				Set @sText = LTrim(Right(@sText,DATALENGTH(@sText) - @iStrike))
			End
			Else
			Begin
				--If you can’t find the delimiter in the text, @sText is the last value in
				--@retArray.
				Set @Value = @sText
				Begin
					Insert @retArray (Idx,Value) Values (@Idx,@Value)
				End
				--Exit the WHILE loop.
				Set @BContinue = 0
			End
		End
	End
	Else
	Begin
		While @bcontinue=1
		Begin
			--If the delimiter is an empty string, check for remaining text
			--instead of a delimiter. Insert the first character into the
			--retArray table. Trim the character from the front of the string.
			--Increment the index and loop.
			If DATALENGTH(@sText) > 1
			Begin
				Set @Value = SUBSTRING(@sText,1,1)
				Begin
					Insert @retArray (Idx,Value) Values (@Idx,@Value)
				End
				Set @Idx = @Idx + 1
				Set @sText = SUBSTRING(@sText,2,DATALENGTH(@sText)-1)
			End
			Else
			Begin
				--One character remains.
				--Insert the character, and exit the WHILE loop.
				Insert @retArray (Idx,Value) Values (@Idx,@sText)
				Set @BContinue = 0 
			End
		End
	End
	Return
End