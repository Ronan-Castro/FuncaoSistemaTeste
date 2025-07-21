CREATE PROCEDURE FI_SP_ConsultaBeneficiario
    @IDCLIENTE INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        Nome,
        CPF,
        IDCLIENTE
    FROM 
        BENEFICIARIOS
    WHERE 
        IDCLIENTE = @IDCLIENTE;
END;