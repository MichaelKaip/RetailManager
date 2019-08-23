CREATE PROCEDURE [dbo].[spUserLookup]
	@Id NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, FirstName, LastName, EmailAdress, CreatedDate
	FROM [dbo].[User]
	WHERE Id = @Id;
end