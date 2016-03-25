
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/25/2016 11:03:36
-- Generated from EDMX file: E:\mindsage2016\WebManagementPortal\WebManagementPortal\EF\MindSageDataModels.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MINDSAGE_COURSECATALOG_V0090];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CourseCatalogs'
CREATE TABLE [dbo].[CourseCatalogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupName] varchar(255)  NOT NULL,
    [Grade] varchar(50)  NOT NULL,
    [Advertisements] varchar(max)  NOT NULL,
    [SideName] varchar(255)  NOT NULL,
    [PriceUSD] float  NOT NULL,
    [Series] varchar(255)  NOT NULL,
    [Title] varchar(255)  NOT NULL,
    [FullDescription] varchar(max)  NOT NULL,
    [DescriptionImageUrl] varchar(max)  NOT NULL,
    [RecLog_CreatedDate] datetime  NOT NULL,
    [RecLog_DeletedDate] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CourseCatalogs'
ALTER TABLE [dbo].[CourseCatalogs]
ADD CONSTRAINT [PK_CourseCatalogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------