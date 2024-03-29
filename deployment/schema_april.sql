USE [master]
GO
/****** Object:  Database [MetricAnalyzer]    Script Date: 04/04/2011 21:39:32 ******/
CREATE DATABASE [MetricAnalyzer]
GO
ALTER DATABASE [MetricAnalyzer] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MetricAnalyzer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MetricAnalyzer] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [MetricAnalyzer] SET ANSI_NULLS OFF
GO
ALTER DATABASE [MetricAnalyzer] SET ANSI_PADDING OFF
GO
ALTER DATABASE [MetricAnalyzer] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [MetricAnalyzer] SET ARITHABORT OFF
GO
ALTER DATABASE [MetricAnalyzer] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [MetricAnalyzer] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [MetricAnalyzer] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [MetricAnalyzer] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [MetricAnalyzer] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [MetricAnalyzer] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [MetricAnalyzer] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [MetricAnalyzer] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [MetricAnalyzer] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [MetricAnalyzer] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [MetricAnalyzer] SET  DISABLE_BROKER
GO
ALTER DATABASE [MetricAnalyzer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [MetricAnalyzer] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [MetricAnalyzer] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [MetricAnalyzer] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [MetricAnalyzer] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [MetricAnalyzer] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [MetricAnalyzer] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [MetricAnalyzer] SET  READ_WRITE
GO
ALTER DATABASE [MetricAnalyzer] SET RECOVERY SIMPLE
GO
ALTER DATABASE [MetricAnalyzer] SET  MULTI_USER
GO
ALTER DATABASE [MetricAnalyzer] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [MetricAnalyzer] SET DB_CHAINING OFF
GO
USE [MetricAnalyzer]
GO
/****** Object:  Table [dbo].[Iteration]    Script Date: 04/04/2011 21:39:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Iteration](
	[IterationID] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[IterationLabel] [nchar](4) NOT NULL,
 CONSTRAINT [PK_Iteration] PRIMARY KEY CLUSTERED 
(
	[IterationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 04/04/2011 21:39:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProductName] [varchar](100) NOT NULL,
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VelocityTrend]    Script Date: 04/04/2011 21:39:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VelocityTrend](
	[ProductID] [int] NOT NULL,
	[IterationID] [int] NOT NULL,
	[EstimatedHours] [float] NOT NULL,
	[ActualHours] [float] NOT NULL,
	[VelocityTrendID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_VelocityTrend_1] PRIMARY KEY CLUSTERED 
(
	[VelocityTrendID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestEffectiveness]    Script Date: 04/04/2011 21:39:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestEffectiveness](
	[TestEffectivenessID] [int] IDENTITY(1,1) NOT NULL,
	[IterationID] [int] NOT NULL,
	[TestCases] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[ProductID] [int] NOT NULL,
 CONSTRAINT [PK_TestEffectiveness] PRIMARY KEY CLUSTERED 
(
	[TestEffectivenessID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rework]    Script Date: 04/04/2011 21:39:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rework](
	[ReworkID] [int] IDENTITY(1,1) NOT NULL,
	[ReworkHours] [float] NOT NULL,
	[ProductID] [int] NOT NULL,
	[IterationID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Rework] PRIMARY KEY CLUSTERED 
(
	[ReworkID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResourceUtilization]    Script Date: 04/04/2011 21:39:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceUtilization](
	[ProductID] [int] NOT NULL,
	[ResourceUtilizationID] [int] IDENTITY(1,1) NOT NULL,
	[PersonHours] [float] NOT NULL,
	[IterationID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[WorkActionID] [int] NOT NULL,
 CONSTRAINT [PK_ResourceUtilization] PRIMARY KEY CLUSTERED 
(
	[ResourceUtilizationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Component]    Script Date: 04/04/2011 21:39:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Component](
	[ComponentID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[ComponentName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Component] PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OutOfScopeWork]    Script Date: 04/04/2011 21:39:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutOfScopeWork](
	[ProductID] [int] NOT NULL,
	[OutOfScopeWorkID] [int] IDENTITY(1,1) NOT NULL,
	[PersonHours] [float] NOT NULL,
	[IterationID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_OutOfScopeWork] PRIMARY KEY CLUSTERED 
(
	[OutOfScopeWorkID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DefectRepairRate]    Script Date: 04/04/2011 21:39:33 ******/
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
/****** Object:  Table [dbo].[DefectInjectionRate]    Script Date: 04/04/2011 21:39:33 ******/
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
/****** Object:  Table [dbo].[Coverage]    Script Date: 04/04/2011 21:39:33 ******/
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
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Coverage] PRIMARY KEY CLUSTERED 
(
	[CoverageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_VelocityTrend_IterationID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[VelocityTrend]  WITH CHECK ADD  CONSTRAINT [FK_VelocityTrend_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[VelocityTrend] CHECK CONSTRAINT [FK_VelocityTrend_IterationID]
GO
/****** Object:  ForeignKey [FK_VelocityTrend_ProductID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[VelocityTrend]  WITH CHECK ADD  CONSTRAINT [FK_VelocityTrend_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[VelocityTrend] CHECK CONSTRAINT [FK_VelocityTrend_ProductID]
GO
/****** Object:  ForeignKey [FK_TestEffectiveness_Iteration]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[TestEffectiveness]  WITH CHECK ADD  CONSTRAINT [FK_TestEffectiveness_Iteration] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[TestEffectiveness] CHECK CONSTRAINT [FK_TestEffectiveness_Iteration]
GO
/****** Object:  ForeignKey [FK_TestEffectiveness_ProductID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[TestEffectiveness]  WITH CHECK ADD  CONSTRAINT [FK_TestEffectiveness_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[TestEffectiveness] CHECK CONSTRAINT [FK_TestEffectiveness_ProductID]
GO
/****** Object:  ForeignKey [FK_Rework_IterationID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[Rework]  WITH CHECK ADD  CONSTRAINT [FK_Rework_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[Rework] CHECK CONSTRAINT [FK_Rework_IterationID]
GO
/****** Object:  ForeignKey [FK_Rework_ProductID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[Rework]  WITH CHECK ADD  CONSTRAINT [FK_Rework_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Rework] CHECK CONSTRAINT [FK_Rework_ProductID]
GO
/****** Object:  ForeignKey [FK_ResourceUtilization_IterationID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[ResourceUtilization]  WITH CHECK ADD  CONSTRAINT [FK_ResourceUtilization_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[ResourceUtilization] CHECK CONSTRAINT [FK_ResourceUtilization_IterationID]
GO
/****** Object:  ForeignKey [FK_ResourceUtilization_ProductID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[ResourceUtilization]  WITH CHECK ADD  CONSTRAINT [FK_ResourceUtilization_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ResourceUtilization] CHECK CONSTRAINT [FK_ResourceUtilization_ProductID]
GO
/****** Object:  ForeignKey [FK_Component_Product]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[Component]  WITH CHECK ADD  CONSTRAINT [FK_Component_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Component] CHECK CONSTRAINT [FK_Component_Product]
GO
/****** Object:  ForeignKey [FK_OutOfScopeWork_IterationID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[OutOfScopeWork]  WITH CHECK ADD  CONSTRAINT [FK_OutOfScopeWork_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[OutOfScopeWork] CHECK CONSTRAINT [FK_OutOfScopeWork_IterationID]
GO
/****** Object:  ForeignKey [FK_OutOfScopeWork_ProductID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[OutOfScopeWork]  WITH CHECK ADD  CONSTRAINT [FK_OutOfScopeWork_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[OutOfScopeWork] CHECK CONSTRAINT [FK_OutOfScopeWork_ProductID]
GO
/****** Object:  ForeignKey [FK_DefectRepairRate_ComponentID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[DefectRepairRate]  WITH CHECK ADD  CONSTRAINT [FK_DefectRepairRate_ComponentID] FOREIGN KEY([ComponentID])
REFERENCES [dbo].[Component] ([ComponentID])
GO
ALTER TABLE [dbo].[DefectRepairRate] CHECK CONSTRAINT [FK_DefectRepairRate_ComponentID]
GO
/****** Object:  ForeignKey [FK_DefectRepairRate_IterationID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[DefectRepairRate]  WITH CHECK ADD  CONSTRAINT [FK_DefectRepairRate_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[DefectRepairRate] CHECK CONSTRAINT [FK_DefectRepairRate_IterationID]
GO
/****** Object:  ForeignKey [FK_DefectInjectionRate_ComponentID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[DefectInjectionRate]  WITH CHECK ADD  CONSTRAINT [FK_DefectInjectionRate_ComponentID] FOREIGN KEY([ComponentID])
REFERENCES [dbo].[Component] ([ComponentID])
GO
ALTER TABLE [dbo].[DefectInjectionRate] CHECK CONSTRAINT [FK_DefectInjectionRate_ComponentID]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DefectInjectionRate and component relationship.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DefectInjectionRate', @level2type=N'CONSTRAINT',@level2name=N'FK_DefectInjectionRate_ComponentID'
GO
/****** Object:  ForeignKey [FK_DefectInjectionRate_IterationID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[DefectInjectionRate]  WITH CHECK ADD  CONSTRAINT [FK_DefectInjectionRate_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[DefectInjectionRate] CHECK CONSTRAINT [FK_DefectInjectionRate_IterationID]
GO
/****** Object:  ForeignKey [FK_Coverage_ComponentID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[Coverage]  WITH NOCHECK ADD  CONSTRAINT [FK_Coverage_ComponentID] FOREIGN KEY([ComponentID])
REFERENCES [dbo].[Component] ([ComponentID])
GO
ALTER TABLE [dbo].[Coverage] NOCHECK CONSTRAINT [FK_Coverage_ComponentID]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Relationship between coverage and component.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Coverage', @level2type=N'CONSTRAINT',@level2name=N'FK_Coverage_ComponentID'
GO
/****** Object:  ForeignKey [FK_Coverage_IterationID]    Script Date: 04/04/2011 21:39:33 ******/
ALTER TABLE [dbo].[Coverage]  WITH CHECK ADD  CONSTRAINT [FK_Coverage_IterationID] FOREIGN KEY([IterationID])
REFERENCES [dbo].[Iteration] ([IterationID])
GO
ALTER TABLE [dbo].[Coverage] CHECK CONSTRAINT [FK_Coverage_IterationID]
GO
