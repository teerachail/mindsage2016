-- Creating table 'TeacherKeys'
CREATE TABLE [dbo].[TeacherKeys] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Grade] varchar(50)  NOT NULL,
    [Code] varchar(50)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL,
    [LicenseId] int  NOT NULL
);
GO
-- Creating foreign key on [LicenseId] in table 'TeacherKeys'
ALTER TABLE [dbo].[TeacherKeys]
ADD CONSTRAINT [FK_LicenseTeacherKey]
    FOREIGN KEY ([LicenseId])
    REFERENCES [dbo].[Licenses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'TeacherKeys'
ALTER TABLE [dbo].[TeacherKeys]
ADD CONSTRAINT [PK_TeacherKeys]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_LicenseTeacherKey'
CREATE INDEX [IX_FK_LicenseTeacherKey]
ON [dbo].[TeacherKeys]
    ([LicenseId]);