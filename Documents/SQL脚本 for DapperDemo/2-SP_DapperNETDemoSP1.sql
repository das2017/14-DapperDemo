USE [DapperDemoDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DapperNETDemoSP1]
        @Name VARCHAR(10),                                            
        @SuccessCode INT OUTPUT,
        @ResultMessage VARCHAR(255) OUTPUT
AS

BEGIN

    SELECT @Name AS NameResult	
    SET @SuccessCode = 0
    SET @ResultMessage = 'ÕÍ≥…÷¥––'
    RETURN 42
END

GO


