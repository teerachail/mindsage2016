-- Creating table 'Licenses'
CREATE TABLE [dbo].[Licenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CourseName] varchar(255)  NOT NULL,
    [Grade] varchar(50)  NOT NULL,
    [StudentsCapacity] int  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL,
    [ContractId] int  NOT NULL,
    [CourseCatalogId] int  NOT NULL
);
GO
-- Creating foreign key on [ContractId] in table 'Licenses'
ALTER TABLE [dbo].[Licenses]
ADD CONSTRAINT [FK_ContractLicense]
    FOREIGN KEY ([ContractId])
    REFERENCES [dbo].[Contracts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating foreign key on [CourseCatalogId] in table 'Licenses'
ALTER TABLE [dbo].[Licenses]
ADD CONSTRAINT [FK_CourseCatalogLicense]
    FOREIGN KEY ([CourseCatalogId])
    REFERENCES [dbo].[CourseCatalogs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'Licenses'
ALTER TABLE [dbo].[Licenses]
ADD CONSTRAINT [PK_Licenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_ContractLicense'
CREATE INDEX [IX_FK_ContractLicense]
ON [dbo].[Licenses]
    ([ContractId]);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_CourseCatalogLicense'
CREATE INDEX [IX_FK_CourseCatalogLicense]
ON [dbo].[Licenses]
    ([CourseCatalogId]);