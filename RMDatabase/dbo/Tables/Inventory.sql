CREATE TABLE [dbo].[Inventory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductId] INT NOT NULL, 
    [Quantity] NCHAR(10) NOT NULL DEFAULT 1, 
    [PurchasePrice] MONEY NOT NULL, 
    [PurchaseDate] DATETIME2 NULL DEFAULT getutcdate(), 
    CONSTRAINT [FK_Inventory_ToProduct] FOREIGN KEY ([ProductID]) REFERENCES [Product]([ID])
)
