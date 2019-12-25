CREATE FUNCTION [fin].[FGetReportLoanDisburseDetails] ( @FDATE SMALLDATETIME , @TDATE SMALLDATETIME , @BRHID INT , @ISVERFIED BIT ) 
RETURNS @temptbl TABLE ( SdId int , PID int , PNAME varchar(100) , IAccno int , ACCNO varchar(50) , ANAME varchar(500) , NOTES varchar(1000) , VDATE datetime , PriDr decimal , OthrDr decimal , TTYPE int , BRNHNO int , POSTEDBY int , Isverfied bit ) AS 
BEGIN
   IF @isverfied = 1 
   INSERT INTO
      @temptbl 
      select
         * 
      from
         (SELECT SchmDetails.SDID, ProductDetail.PID, ProductDetail.PName, ADetail.IAccno, ADetail.Accno, ADetail.Aname, AMTrns.notes, ALoanTra.vdate, 

		ALoanTra.PriDr, ALoanTra.OthrDr, ALoanTra.ttype, AMTrns.brnhno,AMTrns.postedby, 1 as Isverfied

	FROM fin.ProductDetail 

	INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID 

	INNER JOIN fin.ALoanTra 

	INNER JOIN fin.ADetail ON ALoanTra.iaccno = ADetail.IAccno ON ProductDetail.PID = ADetail.PID 

	INNER JOIN trans.AMTrns ON ALoanTra.iaccno = trans.AMTrns.IAccno AND ALoanTra.tno = AMTrns.tno

	WHERE (ALoanTra.vdate between @fdate and @tdate) and AMTrns.brnhno=@brhid and (pridr <> 0 or othrdr <> 0)

	) x
	IF @isverfied = 0 
         INSERT INTO
            @temptbl 
            SELECT
               * 
            FROM
               (
                  SELECT
                     SchmDetails.SDID,
                     ProductDetail.PID,
                     ProductDetail.PName,
                     ADetail.iaccno,
                     ADetail.accno,
                     ADetail.Aname,
                     AsTrns.remarks,
                     AsTrns.PostedOn,
                     PriDr = 
                     case
                        when
                           astrns.IsOtherLoan = 0 
                        then
                           Amount 
                        else
                           0 
                     end
	, OthrDr = case
                        when
                           astrns.IsOtherLoan = 1 
                        then
                           Amount 
                        else
                           0 
                     end
	, Mode as ttype , ADetail.BrchID , astrns.PostedBy , 0 AS Isverfied 
                  FROM
                     fin.FGetLoanDisburseDetails() as AsTrns 
                     INNER JOIN
                        fin.ADetail 
                        ON AsTrns.IAccno = ADetail.IAccno 
                     INNER JOIN
                        fin.ProductDetail 
                        ON ADetail.PID = ProductDetail.PID 
                     INNER JOIN
                        fin.SchmDetails 
                        ON ProductDetail.SDID = SchmDetails.SDID 
                  WHERE
                     (
                        fin.SchmDetails.SType = 1
                     )
                     AND 
                     (
                        AsTrns.PostedOn BETWEEN @fdate AND @tdate 
                     )
                     AND ADetail.BrchID = @brhid 
                     and astrns.VNo is null 
               )
               x 
            WHERE
               x.pridr > 0 
               OR x.othrdr > 0 IF @isverfied = 2 
               INSERT INTO
                  @temptbl 
                  select
                     * 
                  from
                     (
                        SELECT
                           SchmDetails.SDID,
                           ProductDetail.PID,
                           ProductDetail.PName,
                           ADetail.iaccno,
                           ADetail.accno,
                           ADetail.Aname,
                           AsTrns.remarks,
                           AsTrns.PostedOn,
                           PriDr = 
                           case
                              when
                                 astrns.IsOtherLoan = 0 
                              then
                                 Amount 
                              else
                                 0 
                           end
	, OthrDr = 
                           case
                              when
                                 astrns.IsOtherLoan = 1 
                              then
                                 Amount 
                              else
                                 0 
                           end
	, Mode as ttype , ADetail.BrchID , astrns.PostedBy , 0 AS Isverfied 
                        FROM
                           fin.FGetLoanDisburseDetails() as AsTrns 
                           INNER JOIN
                              fin.ADetail 
                              ON AsTrns.IAccno = ADetail.IAccno 
                           INNER JOIN
                              fin.ProductDetail 
                              ON ADetail.PID = ProductDetail.PID 
                           INNER JOIN
                              fin.SchmDetails 
                              ON ProductDetail.SDID = SchmDetails.SDID 
                        WHERE
                           (
                              fin.SchmDetails.SType = 1
                           )
                           AND 
                           (
                              AsTrns.PostedOn BETWEEN @fdate AND @tdate 
                           )
                           AND ADetail.BrchID = @brhid 
                           and astrns.VNo is not null 
                     )
                     x 
                  WHERE
                     x.pridr > 0 
                     OR x.othrdr > 0 
                  union
                  SELECT
                     * 
                  FROM
                     (
                        SELECT
                           SchmDetails.SDID,
                           ProductDetail.PID,
                           ProductDetail.PName,
                           ADetail.iaccno,
                           ADetail.accno,
                           ADetail.Aname,
                           AsTrns.remarks,
                           AsTrns.PostedOn,
                           PriDr = 
                           case
                              when
                                 astrns.IsOtherLoan = 0 
                              then
                                 Amount 
                              else
                                 0 
                           end
	, OthrDr = 
                           case
                              when
                                 astrns.IsOtherLoan = 1 
                              then
                                 Amount 
                              else
                                 0 
                           end
	, Mode as ttype , ADetail.BrchID , astrns.PostedBy , 0 AS Isverfied 
                        FROM
                           fin.FGetLoanDisburseDetails() as AsTrns 
                           INNER JOIN
                              fin.ADetail 
                              ON AsTrns.IAccno = ADetail.IAccno 
                           INNER JOIN
                              fin.ProductDetail 
                              ON ADetail.PID = ProductDetail.PID 
                           INNER JOIN
                              fin.SchmDetails 
                              ON ProductDetail.SDID = SchmDetails.SDID 
                        WHERE
                           (
                              fin.SchmDetails.SType = 1
                           )
                           AND 
                           (
                              AsTrns.PostedOn BETWEEN @fdate AND @tdate 
                           )
                           AND ADetail.BrchID = @brhid 
                           and astrns.VNo is null 
                     )
                     x 
                  WHERE
                     x.pridr > 0 
							OR x.othrdr > 0 RETURN 
END