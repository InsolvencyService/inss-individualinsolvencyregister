
CREATE TABLE [dbo].[findipauthbody](
	[AuthBodyCode] [varchar](5) NOT NULL,
	[AuthBodyName] [varchar](8000) NULL,
	[AuthBodyAddressLine1] [varchar](8000) NULL,
	[AuthBodyAddressLine2] [varchar](8000) NULL,
	[AuthBodyAddressLine3] [varchar](8000) NULL,
	[AuthBodyAddressLine4] [varchar](8000) NULL,
	[AuthBodyAddressLine5] [varchar](8000) NULL,
	[AuthBodyPostcode] [varchar](8000) NULL,
	[phone] [int] NULL,
	[fax] [int] NULL,
	[website] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[AuthBodyCode] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
