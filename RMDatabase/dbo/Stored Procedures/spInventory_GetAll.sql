CREATE PROCEDURE [dbo].[spInventory_GetAll]
AS
begin

	SELECT [Id], [ProductId], [Quantity], [PurchasePrice], [PurchaseDate]
	from dbo.Inventory;
end
