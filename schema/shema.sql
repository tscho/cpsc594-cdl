USE [master]
GO
/****** Object:  Database [CPSC594]    Script Date: 03/07/2011 13:28:59 ******/
CREATE DATABASE [CPSC594] ON  PRIMARY 
( NAME = N'CPSC594', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\CPSC594.mdf' , SIZE = 18432KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CPSC594_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\CPSC594_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CPSC594] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CPSC594].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CPSC594] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [CPSC594] SET ANSI_NULLS OFF
GO
ALTER DATABASE [CPSC594] SET ANSI_PADDING OFF
GO
ALTER DATABASE [CPSC594] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [CPSC594] SET ARITHABORT OFF
GO
ALTER DATABASE [CPSC594] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [CPSC594] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [CPSC594] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [CPSC594] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [CPSC594] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [CPSC594] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [CPSC594] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [CPSC594] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [CPSC594] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [CPSC594] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [CPSC594] SET  DISABLE_BROKER
GO
ALTER DATABASE [CPSC594] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [CPSC594] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [CPSC594] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [CPSC594] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [CPSC594] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [CPSC594] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [CPSC594] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [CPSC594] SET  READ_WRITE
GO
ALTER DATABASE [CPSC594] SET RECOVERY SIMPLE
GO
ALTER DATABASE [CPSC594] SET  MULTI_USER
GO
ALTER DATABASE [CPSC594] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [CPSC594] SET DB_CHAINING OFF
GO
USE [CPSC594]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 03/07/2011 13:29:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectName] [varchar](100) NOT NULL,
	[ProjectID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Iteration]    Script Date: 03/07/2011 13:29:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Iteration](
	[IterationID] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
 CONSTRAINT [PK_Iteration] PRIMARY KEY CLUSTERED 
(
	[IterationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Component]    Script Date: 03/07/2011 13:29:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Component](
	[ComponentID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[ComponentName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Component] PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TestEffectiveness]    Script Date: 03/07/2011 13:29:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestEffectiveness](
	[ComponentID] [int] NOT NULL,
	[TestEffectivenessID] [int] IDENTITY(1,1) NOT NULL,
	[IterationID] [int] NOT NULL,
	[TestCases] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_TestEffectiveness] PRIMARY KEY CLUSTERED 
(
	[TestEffectivenessID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DefectRepairRate]    Script Date: 03/07/2011 13:29:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DefectRepairRate](
	[DefectRepairRateID] [int] IDENTITY(1,1) NOT NULL,
	[ComponentID] [int] NOT NULL,
	[IterationID] [int] NOT NULL,
	[NumberOfResolvedDefects] [int] NOT NULL,
	[NumberOfVerifiedDefects] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_DefectRepairRate] PRIMARY KEY CLUSTERED 
(
	[DefectRepairRateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DefectInjectionRate]    Script Date: 03/07/2011 13:29:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DefectInjectionRate](
	[ComponentID] [int] NOT NULL,
	[DefectInjectionRateID] [int] IDENTITY(1,1) NOT NULL,
	[IterationID] [int] NOT NULL,
	[NumberOfHighDefects] [int] NOT NULL,
	[NumberOfMediumDefects] [int] NOT NULL,
	[NumberOfLowDefects] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_DefectInjectionRate] PRIMARY KEY CLUSTERED 
(
	[DefectInjectionRateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coverage]    Script Date: 03/07/2011 13:29:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Coverage](
	[FileName] [varchar](100) NULL,
	[ComponentID] [int] NOT NULL,
	[LinesCovered] [int] NOT NULL,
	[LinesExecuted] [int] NOT NULL,
	[CoverageID] [int] IDENTITY(1,1) NOT NULL,
	[IterationID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Coverage] PRIMARY KEY CLUSTERED 
(
	[CoverageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_Component_Project]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[Component]  WITH CHECK ADD  CONSTRAINT [FK_Component_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
GO
ALTER TABLE [dbo].[Component] CHECK CONSTRAINT [FK_Component_Project]
GO
/****** Object:  ForeignKey [FK_TestEffectiveness_ComponentID]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[TestEffectiveness]  WITH CHECK ADD  CONSTRAINT [FK_TestEffectiveness_ComponentID] FOREIGN KEY([ComponentID])
REFERENCES [dbo].[Component] ([ComponentID])
GO
ALTER TABLE [dbo].[TestEffectiveness] CHECK CONSTRAINT [FK_TestEffectiveness_ComponentID]
GO
/****** Object:  ForeignKey [FK_TestEffectiveness_Iteration]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[TestEffectiveness]  WITH CHECK ADD  CONSTRAINT [FK_TestEffectiveness_Iteration] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[TestEffectiveness] CHECK CONSTRAINT [FK_TestEffectiveness_Iteration]
GO
/****** Object:  ForeignKey [FK_DefectRepairRate_ComponentID]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[DefectRepairRate]  WITH CHECK ADD  CONSTRAINT [FK_DefectRepairRate_ComponentID] FOREIGN KEY([ComponentID])
REFERENCES [dbo].[Component] ([ComponentID])
GO
ALTER TABLE [dbo].[DefectRepairRate] CHECK CONSTRAINT [FK_DefectRepairRate_ComponentID]
GO
/****** Object:  ForeignKey [FK_DefectRepairRate_IterationID]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[DefectRepairRate]  WITH CHECK ADD  CONSTRAINT [FK_DefectRepairRate_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[DefectRepairRate] CHECK CONSTRAINT [FK_DefectRepairRate_IterationID]
GO
/****** Object:  ForeignKey [FK_DefectInjectionRate_ComponentID]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[DefectInjectionRate]  WITH CHECK ADD  CONSTRAINT [FK_DefectInjectionRate_ComponentID] FOREIGN KEY([ComponentID])
REFERENCES [dbo].[Component] ([ComponentID])
GO
ALTER TABLE [dbo].[DefectInjectionRate] CHECK CONSTRAINT [FK_DefectInjectionRate_ComponentID]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DefectInjectionRate and component relationship.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DefectInjectionRate', @level2type=N'CONSTRAINT',@level2name=N'FK_DefectInjectionRate_ComponentID'
GO
/****** Object:  ForeignKey [FK_DefectInjectionRate_IterationID]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[DefectInjectionRate]  WITH CHECK ADD  CONSTRAINT [FK_DefectInjectionRate_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[DefectInjectionRate] CHECK CONSTRAINT [FK_DefectInjectionRate_IterationID]
GO
/****** Object:  ForeignKey [FK_Coverage_ComponentID]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[Coverage]  WITH NOCHECK ADD  CONSTRAINT [FK_Coverage_ComponentID] FOREIGN KEY([ComponentID])
REFERENCES [dbo].[Component] ([ComponentID])
GO
ALTER TABLE [dbo].[Coverage] NOCHECK CONSTRAINT [FK_Coverage_ComponentID]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relationship between coverage and component.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Coverage', @level2type=N'CONSTRAINT',@level2name=N'FK_Coverage_ComponentID'
GO
/****** Object:  ForeignKey [FK_Coverage_IterationID]    Script Date: 03/07/2011 13:29:00 ******/
ALTER TABLE [dbo].[Coverage]  WITH CHECK ADD  CONSTRAINT [FK_Coverage_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[Coverage] CHECK CONSTRAINT [FK_Coverage_IterationID]
GO
