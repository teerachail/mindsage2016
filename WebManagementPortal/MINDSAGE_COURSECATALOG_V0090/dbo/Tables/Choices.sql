-- Creating table 'Choices'
CREATE TABLE [dbo].[Choices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(255)  NOT NULL,
    [IsCorrect] bit  NOT NULL,
    [AssessmentId] int  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- Creating foreign key on [AssessmentId] in table 'Choices'
ALTER TABLE [dbo].[Choices]
ADD CONSTRAINT [FK_AssessmentChoice]
    FOREIGN KEY ([AssessmentId])
    REFERENCES [dbo].[Assessments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'Choices'
ALTER TABLE [dbo].[Choices]
ADD CONSTRAINT [PK_Choices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_AssessmentChoice'
CREATE INDEX [IX_FK_AssessmentChoice]
ON [dbo].[Choices]
    ([AssessmentId]);