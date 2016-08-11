-- Creating table 'Assessments'
CREATE TABLE [dbo].[Assessments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Order] int  NOT NULL,
    [ContentType] varchar(100)  NOT NULL,
    [Question] varchar(max)  NOT NULL,
    [StatementBefore] varchar(max)  NOT NULL,
    [StatementAfter] varchar(max)  NOT NULL,
    [AssessmentItemId] int  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO
-- Creating foreign key on [AssessmentItemId] in table 'Assessments'
ALTER TABLE [dbo].[Assessments]
ADD CONSTRAINT [FK_AssessmentItemAssessment]
    FOREIGN KEY ([AssessmentItemId])
    REFERENCES [dbo].[AssessmentItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'Assessments'
ALTER TABLE [dbo].[Assessments]
ADD CONSTRAINT [PK_Assessments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_AssessmentItemAssessment'
CREATE INDEX [IX_FK_AssessmentItemAssessment]
ON [dbo].[Assessments]
    ([AssessmentItemId]);