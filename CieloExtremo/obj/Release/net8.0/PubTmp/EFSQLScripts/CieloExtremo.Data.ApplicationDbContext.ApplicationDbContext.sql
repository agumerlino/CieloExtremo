IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240626172007_inicial'
)
BEGIN
    CREATE TABLE [Producto] (
        [Id] int NOT NULL IDENTITY,
        [nombre] nvarchar(max) NULL,
        [descripcion] nvarchar(max) NULL,
        [precio] int NOT NULL,
        [categoria] nvarchar(max) NOT NULL,
        [subcategoria] nvarchar(max) NULL,
        [foto] nvarchar(max) NULL,
        [destacar] bit NOT NULL,
        CONSTRAINT [PK_Producto] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240626172007_inicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240626172007_inicial', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813223003_fotos'
)
BEGIN
    EXEC sp_rename N'[Producto].[foto]', N'foto6', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813223003_fotos'
)
BEGIN
    ALTER TABLE [Producto] ADD [foto1] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813223003_fotos'
)
BEGIN
    ALTER TABLE [Producto] ADD [foto2] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813223003_fotos'
)
BEGIN
    ALTER TABLE [Producto] ADD [foto3] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813223003_fotos'
)
BEGIN
    ALTER TABLE [Producto] ADD [foto4] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813223003_fotos'
)
BEGIN
    ALTER TABLE [Producto] ADD [foto5] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240813223003_fotos'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240813223003_fotos', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823015242_moneda'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Producto]') AND [c].[name] = N'precio');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Producto] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Producto] ALTER COLUMN [precio] nvarchar(max) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240823015242_moneda'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240823015242_moneda', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240827173659_infovendedor'
)
BEGIN
    ALTER TABLE [Producto] ADD [mailVendedor] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240827173659_infovendedor'
)
BEGIN
    ALTER TABLE [Producto] ADD [telefonoVendedor] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240827173659_infovendedor'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240827173659_infovendedor', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240910140226_1'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Producto]') AND [c].[name] = N'telefonoVendedor');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Producto] DROP CONSTRAINT [' + @var1 + '];');
    EXEC(N'UPDATE [Producto] SET [telefonoVendedor] = N'''' WHERE [telefonoVendedor] IS NULL');
    ALTER TABLE [Producto] ALTER COLUMN [telefonoVendedor] nvarchar(max) NOT NULL;
    ALTER TABLE [Producto] ADD DEFAULT N'' FOR [telefonoVendedor];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240910140226_1'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Producto]') AND [c].[name] = N'nombre');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Producto] DROP CONSTRAINT [' + @var2 + '];');
    EXEC(N'UPDATE [Producto] SET [nombre] = N'''' WHERE [nombre] IS NULL');
    ALTER TABLE [Producto] ALTER COLUMN [nombre] nvarchar(max) NOT NULL;
    ALTER TABLE [Producto] ADD DEFAULT N'' FOR [nombre];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240910140226_1'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Producto]') AND [c].[name] = N'mailVendedor');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Producto] DROP CONSTRAINT [' + @var3 + '];');
    EXEC(N'UPDATE [Producto] SET [mailVendedor] = N'''' WHERE [mailVendedor] IS NULL');
    ALTER TABLE [Producto] ALTER COLUMN [mailVendedor] nvarchar(max) NOT NULL;
    ALTER TABLE [Producto] ADD DEFAULT N'' FOR [mailVendedor];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240910140226_1'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Producto]') AND [c].[name] = N'descripcion');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Producto] DROP CONSTRAINT [' + @var4 + '];');
    EXEC(N'UPDATE [Producto] SET [descripcion] = N'''' WHERE [descripcion] IS NULL');
    ALTER TABLE [Producto] ALTER COLUMN [descripcion] nvarchar(max) NOT NULL;
    ALTER TABLE [Producto] ADD DEFAULT N'' FOR [descripcion];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240910140226_1'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240910140226_1', N'8.0.4');
END;
GO

COMMIT;
GO

