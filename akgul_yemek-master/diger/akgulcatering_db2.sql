/****** Object:  Table [dbo].[details]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[details](
	[key_] [nvarchar](50) NOT NULL,
	[value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_details] PRIMARY KEY CLUSTERED 
(
	[key_] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[food]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[food](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[category_id] [int] NOT NULL,
	[unit_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [float] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_food] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[food_category]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[food_category](
	[name] [nvarchar](50) NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_food_category] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[food_units]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[food_units](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_food_units] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[gallery_categories]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gallery_categories](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_gallery_categories] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[gallery_images]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gallery_images](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[file_name] [nvarchar](50) NOT NULL,
	[title_id] [int] NOT NULL,
 CONSTRAINT [PK_gallery_images] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[gallery_titles]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gallery_titles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[keywords] [nvarchar](300) NOT NULL,
	[category_id] [int] NOT NULL,
 CONSTRAINT [PK_gallery_titles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[organization_and_food]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[organization_and_food](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[organization_information_id] [int] NOT NULL,
	[food_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [real] NOT NULL,
	[total_price] [real] NOT NULL,
	[people_count] [int] NOT NULL,
 CONSTRAINT [PK_organization_and_food] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[organization_and_service]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[organization_and_service](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[organization_information_id] [int] NOT NULL,
	[organization_service_id] [int] NOT NULL,
	[information] [nvarchar](500) NOT NULL,
	[price] [int] NOT NULL,
 CONSTRAINT [PK_organization_and_service] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[organization_information]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[organization_information](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[organization_category_id] [int] NOT NULL,
	[organization_name] [nvarchar](500) NOT NULL,
	[name_surname] [nvarchar](50) NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[telephone] [nvarchar](11) NOT NULL,
	[telephone2] [nvarchar](11) NOT NULL,
	[date] [datetime] NOT NULL,
	[time] [nvarchar](50) NOT NULL,
	[people_count] [int] NOT NULL,
	[referance] [nvarchar](50) NOT NULL,
	[down_payment] [int] NOT NULL,
	[organizators_adress] [nvarchar](300) NOT NULL,
	[organization_adress] [nvarchar](300) NOT NULL,
	[organization_status] [int] NOT NULL,
	[total_price] [int] NOT NULL,
 CONSTRAINT [PK_organization] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[organization_service]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[organization_service](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_organization_service] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[organization_type]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[organization_type](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[color] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_organization_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ready_menu]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ready_menu](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ready_menu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ready_menu_and_food]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ready_menu_and_food](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ready_menu_id] [int] NOT NULL,
	[food_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [real] NOT NULL,
 CONSTRAINT [PK_ready_menu_and_food] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[site_food_menu]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[site_food_menu](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[site_menu_id] [int] NOT NULL,
	[image_name] [nvarchar](50) NOT NULL,
	[description] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_site_food_menu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[site_menu]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[site_menu](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_site_menu] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[slider]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[slider](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[filename] [nvarchar](50) NOT NULL,
	[keywords] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_slider] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[user]    Script Date: 11.04.2018 03:28:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[telephone] [nvarchar](11) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[job] [nvarchar](50) NOT NULL,
	[type] [int] NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[telephone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[details] ([key_], [value]) VALUES (N'address', N'Köseköy Mahallesi Atatürk Caddesi No:16 Köseköy / Kartepe / KOCAELİ')
INSERT [dbo].[details] ([key_], [value]) VALUES (N'email', N'akgulcatering@gmail.com')
INSERT [dbo].[details] ([key_], [value]) VALUES (N'hakkimizda', N'<p>Şirketimiz taşıma yemek, yerinde &uuml;retim ve nişan, d&uuml;ğ&uuml;n, mevlit ve toplantı organizasyon faaliyetleri ile m&uuml;şterilerimize hizmet vermektedir. </p>

<p>&nbsp;</p>

<p>AKG&Uuml;L KARDEŞLER CATERİNG ORGANİZASYON A kalite &uuml;r&uuml;nlerle, hijyenik &uuml;retim ve kaliteli servis anlayışı ile hizmet vererek,kalitenin ve lezzetin adresi olmuştur. Akg&uuml;l kardeşler firması m&uuml;şterileri olarak sağlıklı, hijyenik ve A kalite sebze ve meyvelerden oluşan &uuml;r&uuml;nlerin pişirilmesini ve hazırlanmasını g&uuml;n&uuml;n her saatinde &uuml;retim tesisimizi ziyaret edebilirler. Ayrıca Hijyen anlayışına farklı bir bakış a&ccedil;ısı kazandıran şirketimiz, yetkililer tarafından haşere kontrolleri, &uuml;retim yerlerinin &ouml;ncesi ve sonrasındaki temizlik kontrolleri aksatılmadan yapılmaktadır.</p>

<p>&nbsp;</p>

<p>Catering sekt&ouml;r&uuml;ne adım attığımızda &ouml;n&uuml;m&uuml;ze &ouml;ncelikle, d&uuml;r&uuml;st olmak ve d&uuml;r&uuml;st olarak kalmak, eğitimli ve işinin ehli aş&ccedil;ı ve elamanlarla &uuml;retim yapılırken yemeğinize sevgiyle ağız tadınızın katılması gibi &ccedil;ok &ouml;nemli hedefler koyduk. Bunun &ouml;ncesinde gıda m&uuml;hendisleri tarafından kalori ve besin değerleri dikkate alınarak, hazırlanması d&uuml;ş&uuml;ncesi ve fikrimizden asla taviz vermediğimiz gibi iyinin de iyisinin olacağı d&uuml;ş&uuml;ncesiyle teknolojiyi takip etmeye &ccedil;alıştık b&uuml;t&uuml;n &ouml;zel g&uuml;nlerinizde hizmetinizdeyiz</p>

<p>bizi tercih ettiğiniz i&ccedil;in teşekk&uuml;rler</p>

<p>AKG&Uuml;L KARDEŞLER CATERİNG ORGANİZASYON</p>
')
INSERT [dbo].[details] ([key_], [value]) VALUES (N'hizmetlerimiz', N'<p>İ&ccedil;erik eklenecektir.</p>
')
INSERT [dbo].[details] ([key_], [value]) VALUES (N'insankaynaklari', N'<p>insan kaynaklari2</p>
')
INSERT [dbo].[details] ([key_], [value]) VALUES (N'sertifikalarimiz', N'<h2><strong>SERTİFİKALARIMIZ</strong></h2>

<p><em>1-)<img alt="" src="&lt;a href=&quot;https://hizliresim.com/lO5zvJ&quot;&gt;&lt;img src=&quot;https://i.hizliresim.com/lO5zvJ.jpg&quot;&gt;&lt;/a&gt;" />&nbsp;HELAL SERTİFİKASI</em></p>

<p>Helal &uuml;r&uuml;n; &ouml;ncelikle temiz ve g&uuml;venilir &uuml;r&uuml;n demektir. Bunun yanında hammadde girişinden son &uuml;r&uuml;n&uuml;n t&uuml;ketimine kadar olan t&uuml;m aşamalarda,&nbsp;İslami h&uuml;k&uuml;mlere uygun i&ccedil;erik, hammadde, faaliyet ve s&uuml;re&ccedil; ile &uuml;retilen &uuml;r&uuml;n demektir. Bu bağlamda&nbsp;İslam dinince yasaklanmamış &uuml;r&uuml;nler Helal olarak nitelendirilmektedir.</p>

<p>T&uuml;rkiyede, toplu yemek ve catering hizmetinde&nbsp;helal standartlarında &uuml;retim yapan ilk firmayız.</p>

<p>&nbsp;</p>

<img alt="Helal sertifikası" src="Files/Sertifikalar/001-helalsertifikasi.jpg"   class="col-md-12 col-xs-12"/>

<br />
<br />
<br />


<p><em>2-) TS-EN-ISO 2200-2004&nbsp;&nbsp;HACCP GIDA G&Uuml;VENLİĞİ Y&Ouml;NETİM SİSTEMİ</em>&nbsp;</p>

<p>ISO 22000 Standardı&#39;nın temel yaklaşımı, t&uuml;keticinin gıda kaynaklı hastalıklara maruz kalmaması i&ccedil;in geliştirilmiş, gıda zinciri i&ccedil;erisindeki t&uuml;m prosesleri altyapı, personel ve ekipman gibi t&uuml;m etkileyenleriyle birlikte kontrol altında tutan &ouml;nleyici bir sistemin kuruluşlarda uygulanmasıdır. Kuruluşlarda Gıda G&uuml;venliği Y&ouml;netim Sistemi uygulamaları &uuml;retim kontrol&uuml;, &uuml;r&uuml;n kontrol&uuml;, ekipman kontrol&uuml;; bakım ve genel hijyen uygulamaları; personel ve ziyaret&ccedil;i hijyeni; taşıma, depolama, &uuml;r&uuml;n bilgisi; eğitim, tedarik&ccedil;i se&ccedil;imi ve değerlendirmesi; eğitim, iletişim ve benzeri konuları kapsamaktadır.<br />
&nbsp;</p>


<img alt="ISO 2200-2004" src="Files/Sertifikalar/002-ISO22000.jpg"  class="col-md-12 col-xs-12"/>



<br />
<br />
<br />

<p><em>3-) TS-EN-ISO 9001-2015&nbsp;KALİTE Y&Ouml;NETİM SİSTEMİ</em></p>

<p>ISO 9001 Kalite Y&ouml;netim Sistemi, m&uuml;şteri beklentileri, ihtiya&ccedil;ları ve mevzuat şartlarını karşılama yolu ile m&uuml;şteri memnuniyetinin artırılmasını &ouml;ng&ouml;ren d&uuml;nyaca kabul g&ouml;rm&uuml;ş bir kalite y&ouml;netimi sistemi bi&ccedil;imidir. Kuruluşun organizasyonel yapısından m&uuml;şterilerinin memnuniyet seviyesine, toplanan verilerin analiz edilmesinden s&uuml;re&ccedil;lerin etkin y&ouml;netimine, i&ccedil; denetimlerden &uuml;r&uuml;n tasarımına, satın almadan satışa kadar pek &ccedil;ok noktada Kalite Y&ouml;netim Sistemi koşullarını belirler. ISO 9001 Standardı, esas olarak bir kontrol mekanizmasıdır. Bu standardın amacı, hata ve kusurları azaltmak, ortadan kaldırmak ve daha &ouml;nemlisi oluşabilecek hata ve kusurları &ouml;nlemektir. Standart, direk olarak &uuml;r&uuml;n ve hizmet kalitesiyle ilgili değil, y&ouml;netim sisteminin kalitesi ile ilgilidir. Buradaki temel varsayım, etkin bir Kalite Y&ouml;netim Sistemi oluşturulması ve uygulanması halinde m&uuml;şteri ihtiya&ccedil;larını karşılayacak kaliteli &uuml;r&uuml;n ve hizmetler &uuml;retileceğidir</p>


<img alt="ISO 9001" src="Files/Sertifikalar/003-ISO9001.jpg"    class="col-md-12 col-xs-12"/>


<br />
<br />
<br />


<img alt="Akgül Tarım İşletme Kayıt" src="Files/Sertifikalar/006-tarimisletmekayit.jpg"   class="col-md-12 col-xs-12"/>



<br />
<br />
<br />


<img alt="Akgül Marka Tescil" src="Files/Sertifikalar/007-markatescil.jpg"   class="col-md-12 col-xs-12"/>



<br />
<br />
<br />


<img alt="Akgül Kartepe Teşekkür" src="Files/Sertifikalar/010-kartepetesekkur.jpg"   class="col-md-12 col-xs-12"/>


<br />
<br />')
INSERT [dbo].[details] ([key_], [value]) VALUES (N'tel1', N'+90 530 425 26 98')
INSERT [dbo].[details] ([key_], [value]) VALUES (N'tel2', N'+90 262 606 01 40')
SET IDENTITY_INSERT [dbo].[food] ON 

INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (2, 1, 4, 1, 5, N'Yayla')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (3, 1, 4, 1, 6, N'Ezogelin')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (4, 3, 3, 2, 3, N'Tulumba')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (5, 3, 4, 1, 5, N'Kadayıf')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (10, 6, 1, 100, 2, N'Kavurma')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (13, 7, 3, 1, 1, N'Su')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (14, 7, 3, 1, 1, N'Ayran')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (15, 7, 3, 1, 2, N'Gazoz')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (16, 8, 4, 1, 2, N'Pirinç Pilavı')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (17, 8, 4, 1, 2, N'Bulgur Pilavı')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (18, 1, 4, 1, 1, N'Mercimek')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (19, 1, 4, 1, 3, N'Mantar')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (21, 10, 4, 1, 1, N'Patates-domates- Biber')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (24, 3, 3, 3, 1.5, N'Revani')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (25, 7, 3, 1, 1, N'Kola')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (26, 7, 3, 1, 1, N'Karışık İçecek')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (28, 11, 1, 150, 3, N'TAVUK (150 GR)')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (29, 11, 1, 100, 2, N'TAVUK (100 GR)')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (30, 6, 1, 150, 9, N'KAVURMA (150 GR)')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (31, 6, 1, 100, 6, N'KAVURMA (100GR)')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (34, 11, 1, 120, 3, N'TAVUK DÖNER (120 GR)')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (35, 11, 1, 150, 4, N'TAVUK DÖNER (150GR)')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (36, 6, 1, 120, 6, N'ET DÖNER (120GR)')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (37, 6, 1, 150, 7.5, N'ET DÖNER (150GR)')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (40, 12, 3, 1, 1, N'Köpük Tabak')
INSERT [dbo].[food] ([id], [category_id], [unit_id], [quantity], [price], [name]) VALUES (41, 12, 3, 1, 3, N'Porselen Tabak')
SET IDENTITY_INSERT [dbo].[food] OFF
SET IDENTITY_INSERT [dbo].[food_category] ON 

INSERT [dbo].[food_category] ([name], [id]) VALUES (N'Çorba', 1)
INSERT [dbo].[food_category] ([name], [id]) VALUES (N'Tatlı', 3)
INSERT [dbo].[food_category] ([name], [id]) VALUES (N'Et', 6)
INSERT [dbo].[food_category] ([name], [id]) VALUES (N'İçecek', 7)
INSERT [dbo].[food_category] ([name], [id]) VALUES (N'Pilav', 8)
INSERT [dbo].[food_category] ([name], [id]) VALUES (N'Pasta', 9)
INSERT [dbo].[food_category] ([name], [id]) VALUES (N'Garnitür', 10)
INSERT [dbo].[food_category] ([name], [id]) VALUES (N'Tavuk', 11)
INSERT [dbo].[food_category] ([name], [id]) VALUES (N'Tabak', 12)
SET IDENTITY_INSERT [dbo].[food_category] OFF
SET IDENTITY_INSERT [dbo].[food_units] ON 

INSERT [dbo].[food_units] ([id], [name]) VALUES (1, N'Gram')
INSERT [dbo].[food_units] ([id], [name]) VALUES (2, N'Kilo')
INSERT [dbo].[food_units] ([id], [name]) VALUES (3, N'Tane')
INSERT [dbo].[food_units] ([id], [name]) VALUES (4, N'Porsiyon')
SET IDENTITY_INSERT [dbo].[food_units] OFF
SET IDENTITY_INSERT [dbo].[gallery_categories] ON 

INSERT [dbo].[gallery_categories] ([id], [name]) VALUES (1, N'Organizasyonlarımız')
INSERT [dbo].[gallery_categories] ([id], [name]) VALUES (7, N'Sertifikalarımız')
INSERT [dbo].[gallery_categories] ([id], [name]) VALUES (8, N'Videolarımız')
INSERT [dbo].[gallery_categories] ([id], [name]) VALUES (10, N'Menülerimiz')
SET IDENTITY_INSERT [dbo].[gallery_categories] OFF
SET IDENTITY_INSERT [dbo].[gallery_images] ON 

INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (10, N'660WhatsApp Image 2017-09-23 at 17.30.49.jpeg', 6)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (12, N'906WhatsApp Image 2017-09-23 at 17.30.12(1).jpeg', 6)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (13, N'524WhatsApp Image 2017-09-23 at 17.30.55.jpeg', 6)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (52, N'1003001-HELAL SERTİFİKASI (1275 x 1754)--888.jpg', 19)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (53, N'2IdVy_doyBA', 20)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (57, N'b-jqL1aE1jI', 22)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (59, N'580IMG_20180311_132817 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (60, N'765IMG_20180311_132851 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (61, N'605IMG_20180311_132936 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (62, N'780IMG_20180311_132958 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (63, N'113IMG_20180311_133022 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (64, N'178IMG_20180311_133042 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (65, N'911IMG_20180311_133152 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (66, N'9IMG_20180311_133205 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (67, N'948IMG_20180311_133236 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (68, N'444IMG_20180311_133315 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (69, N'11IMG_20180311_133339 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (70, N'933IMG_20180311_133503 (Copy).jpg', 23)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (73, N'778IMG_20180311_133538 (Copy).jpg', 24)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (74, N'696IMG_20180311_133555 (Copy).jpg', 24)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (75, N'533IMG_20180311_133808 (Copy).jpg', 25)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (76, N'700IMG_20180311_133817 (Copy).jpg', 25)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (77, N'957IMG_20180311_133825 (Copy).jpg', 25)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (78, N'93ruyada-kuru-pasta-yemek.gif', 25)
INSERT [dbo].[gallery_images] ([id], [file_name], [title_id]) VALUES (79, N'254b (8).jpg', 25)
SET IDENTITY_INSERT [dbo].[gallery_images] OFF
SET IDENTITY_INSERT [dbo].[gallery_titles] ON 

INSERT [dbo].[gallery_titles] ([id], [title], [keywords], [category_id]) VALUES (6, N'Organizasyonlarımızdan Kareler', N'kocaeli,catering,yemek,toplu yemek,organizasyon,akgül,akgül yemek', 1)
INSERT [dbo].[gallery_titles] ([id], [title], [keywords], [category_id]) VALUES (19, N'Sertifikalarımız', N'Helal, hijyen,haccp, ıso, 22000, 9001', 7)
INSERT [dbo].[gallery_titles] ([id], [title], [keywords], [category_id]) VALUES (20, N'Tanıtım Videomuz', N'yemek, toplu yemek, catering, düğün, nişan, sünnet', 8)
INSERT [dbo].[gallery_titles] ([id], [title], [keywords], [category_id]) VALUES (22, N'Paketleme Makinemiz', N'Paket yemek, yemek, kocaeli paket yemek, ', 8)
INSERT [dbo].[gallery_titles] ([id], [title], [keywords], [category_id]) VALUES (23, N'Cemiyet Menülerimiz(Porselen)', N'TOPLU YEMEK, KAVURMA, PİLAV,TAVUK,HELAL', 10)
INSERT [dbo].[gallery_titles] ([id], [title], [keywords], [category_id]) VALUES (24, N'Cemiyet Münülerimiz(Köpük)', N'pilav, kavurma, helal, tavuk', 10)
INSERT [dbo].[gallery_titles] ([id], [title], [keywords], [category_id]) VALUES (25, N'Tatlı Çeşitlerimiz', N'Tulumba, yaş pasta, kuru pasta, irmik', 10)
SET IDENTITY_INSERT [dbo].[gallery_titles] OFF
SET IDENTITY_INSERT [dbo].[organization_and_food] ON 

INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (70, 23, 4, 1, 0.6, 240.000015, 400)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (71, 23, 30, 100, 5, 2000, 400)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (72, 23, 14, 1, 0.5, 200, 400)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (73, 23, 16, 1, 1, 400, 400)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (74, 23, 21, 1, 1, 400, 400)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (75, 25, 36, 100, 9, 5400, 600)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (76, 26, 36, 100, 9, 2700, 300)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (77, 27, 36, 100, 9, 2700, 300)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (78, 28, 4, 1, 0.6, 360, 600)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (79, 28, 16, 1, 1, 600, 600)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (80, 28, 26, 1, 1.5, 900, 600)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (81, 28, 21, 1, 1, 600, 600)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (82, 28, 36, 100, 6, 3600, 600)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (89, 31, 4, 1, 0.6, 60.0000038, 100)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (90, 31, 16, 1, 1, 100, 100)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (91, 31, 21, 1, 1, 100, 100)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (92, 31, 28, 150, 3, 300, 100)
INSERT [dbo].[organization_and_food] ([id], [organization_information_id], [food_id], [quantity], [price], [total_price], [people_count]) VALUES (93, 31, 13, 1, 1, 100, 100)
SET IDENTITY_INSERT [dbo].[organization_and_food] OFF
SET IDENTITY_INSERT [dbo].[organization_and_service] ON 

INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (46, 23, 3, N'Mehter Takımı
', 1000)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (47, 23, 2, N'10 kişi
4 aşçı
6 garson', 1080)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (48, 23, 4, N'Altın sarısı kuşak
Masal org. Taşıma ve sünnet tahtı', 1700)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (49, 23, 2, N'10 kişi
4 aşçı
6 garson', 1060)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (50, 23, 2, N'10 kişi
4 aşçı
6 garson', 1060)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (51, 23, 2, N'10 kişi
4 aşçı
6 garson', 1060)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (52, 23, 2, N'10 kişi
4 aşçı
6 garson', 1060)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (58, 24, 4, N'SÜNNET TAHTI
SÜSLEME
SANDALYE GİYDİRME 
NAKLİYE', 2850)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (59, 25, 3, N'NAKLİYE', 100)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (60, 25, 2, N'YEMEK SERVİS', 1000)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (61, 25, 4, N'MASA-SANDALYE 300 ADET 900 TL GİYDİRME 900 TL', 1800)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (62, 26, 4, N'KÖPÜK TABAK-SÜSLEME GİYDİRME
15 DİKDÖRTGEN MASA 150 SANDALYE', 1300)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (63, 27, 4, N'KÖPÜK TABAK
GİYDİRME TAKIM
', 1300)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (64, 28, 4, N'SALON GİYDİRME HEDİYE
TURKUAZ KUŞAK', 940)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (76, 31, 1, N'2 büyük kazan.', 100)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (77, 31, 2, N'Ali ataca', 100)
INSERT [dbo].[organization_and_service] ([id], [organization_information_id], [organization_service_id], [information], [price]) VALUES (78, 31, 4, N'Yemekler ha+++66
55555522222222222
44
555554565445
erwrwrwerw
feaff


2255252', 0)
SET IDENTITY_INSERT [dbo].[organization_and_service] OFF
SET IDENTITY_INSERT [dbo].[organization_information] ON 

INSERT [dbo].[organization_information] ([id], [organization_category_id], [organization_name], [name_surname], [email], [telephone], [telephone2], [date], [time], [people_count], [referance], [down_payment], [organizators_adress], [organization_adress], [organization_status], [total_price]) VALUES (23, 1, N'Necipfazıl', N'ERTUGRUL AK', N'belirtilmedi', N'5324941966', N'00000000000', CAST(0x0000A9020130DEE0 AS DateTime), N'18:30', 400, N'belirtilmedi', 0, N'belirtilmedi', N'Necip Fazıl Kısakürek Kültür Merkezi', 1, 0)
INSERT [dbo].[organization_information] ([id], [organization_category_id], [organization_name], [name_surname], [email], [telephone], [telephone2], [date], [time], [people_count], [referance], [down_payment], [organizators_adress], [organization_adress], [organization_status], [total_price]) VALUES (24, 1, N'ÇINARALTI-GİYDİRME', N'İSMAİL ABİ', N'belirtilmedi', N'5537023541', N'00000000000', CAST(0x0000A91000D63BC0 AS DateTime), N'13:00', 500, N'belirtilmedi', 0, N'belirtilmedi', N'belirtilmedi', 1, 0)
INSERT [dbo].[organization_information] ([id], [organization_category_id], [organization_name], [name_surname], [email], [telephone], [telephone2], [date], [time], [people_count], [referance], [down_payment], [organizators_adress], [organization_adress], [organization_status], [total_price]) VALUES (25, 1, N'ÇINARALTI', N'İSMAİL ABİ', N'belirtilmedi', N'5304252698', N'00000000000', CAST(0x0000A94F00D63BC0 AS DateTime), N'13:00', 600, N'belirtilmedi', 0, N'belirtilmedi', N'belirtilmedi', 1, 0)
INSERT [dbo].[organization_information] ([id], [organization_category_id], [organization_name], [name_surname], [email], [telephone], [telephone2], [date], [time], [people_count], [referance], [down_payment], [organizators_adress], [organization_adress], [organization_status], [total_price]) VALUES (26, 1, N'ÇINARALTI', N'İSMAİL ABİ', N'belirtilmedi', N'5304252698', N'00000000000', CAST(0x0000A917011826C0 AS DateTime), N'17:00', 300, N'belirtilmedi', 0, N'belirtilmedi', N'belirtilmedi', 1, 0)
INSERT [dbo].[organization_information] ([id], [organization_category_id], [organization_name], [name_surname], [email], [telephone], [telephone2], [date], [time], [people_count], [referance], [down_payment], [organizators_adress], [organization_adress], [organization_status], [total_price]) VALUES (27, 1, N'ÇINARALTI', N'İSMAİL ABİ', N'belirtilmedi', N'5537023541', N'00000000000', CAST(0x0000A90900D63BC0 AS DateTime), N'13:00', 300, N'belirtilmedi', 0, N'belirtilmedi', N'belirtilmedi', 1, 0)
INSERT [dbo].[organization_information] ([id], [organization_category_id], [organization_name], [name_surname], [email], [telephone], [telephone2], [date], [time], [people_count], [referance], [down_payment], [organizators_adress], [organization_adress], [organization_status], [total_price]) VALUES (28, 1, N'ZÜBEYDE HANIM', N'SERVET DEMİR', N'belirtilmedi', N'5362003487', N'5373083727', CAST(0x0000A8BC00D63BC0 AS DateTime), N'13:00', 600, N'belirtilmedi', 2000, N'ZÜBEYDE HANIM KÜLTÜR MERKEZİ', N'ZÜBEYDE HANIM KÜLTÜR MERKEZİ', 1, 0)
INSERT [dbo].[organization_information] ([id], [organization_category_id], [organization_name], [name_surname], [email], [telephone], [telephone2], [date], [time], [people_count], [referance], [down_payment], [organizators_adress], [organization_adress], [organization_status], [total_price]) VALUES (31, 1, N'Deneme', N'Emin', N'eyildizpro@gmail.com', N'05372403481', N'00000000000', CAST(0x0000A8C200FB4FF0 AS DateTime), N'15:15', 100, N'belirtilmedi', 0, N'belirtilmedi', N'belirtilmedi', 0, 0)
SET IDENTITY_INSERT [dbo].[organization_information] OFF
SET IDENTITY_INSERT [dbo].[organization_service] ON 

INSERT [dbo].[organization_service] ([id], [name]) VALUES (1, N'Emanet')
INSERT [dbo].[organization_service] ([id], [name]) VALUES (2, N'Personel')
INSERT [dbo].[organization_service] ([id], [name]) VALUES (3, N'Servis')
INSERT [dbo].[organization_service] ([id], [name]) VALUES (4, N'Aciklama')
SET IDENTITY_INSERT [dbo].[organization_service] OFF
SET IDENTITY_INSERT [dbo].[organization_type] ON 

INSERT [dbo].[organization_type] ([id], [name], [color]) VALUES (1, N'Düğün', N'RED')
INSERT [dbo].[organization_type] ([id], [name], [color]) VALUES (2, N'Sünnet', N'RED')
INSERT [dbo].[organization_type] ([id], [name], [color]) VALUES (3, N'Mevlid', N'RED')
INSERT [dbo].[organization_type] ([id], [name], [color]) VALUES (4, N'Cenaze ', N'RED')
SET IDENTITY_INSERT [dbo].[organization_type] OFF
SET IDENTITY_INSERT [dbo].[ready_menu] ON 

INSERT [dbo].[ready_menu] ([id], [name]) VALUES (1, N'MENÜ-1 TAVUK PİLAV')
INSERT [dbo].[ready_menu] ([id], [name]) VALUES (6, N'MENÜ-2 PİLAV ÜSTÜ TAVUK')
INSERT [dbo].[ready_menu] ([id], [name]) VALUES (7, N'MENÜ-3 ET PİLAV')
INSERT [dbo].[ready_menu] ([id], [name]) VALUES (8, N'MENÜ-4 PİLAV ÜSTÜ ET')
INSERT [dbo].[ready_menu] ([id], [name]) VALUES (9, N'MENÜ-5 ET DÖNER (120GR)')
INSERT [dbo].[ready_menu] ([id], [name]) VALUES (11, N'MENÜ-6 ET DÖNER (150GR)')
INSERT [dbo].[ready_menu] ([id], [name]) VALUES (12, N'MENÜ-7 TAVUK DÖNER (120GR)')
INSERT [dbo].[ready_menu] ([id], [name]) VALUES (13, N'MENÜ-8 TAVUK DÖNER (150GR)')
INSERT [dbo].[ready_menu] ([id], [name]) VALUES (14, N'TestMenusu')
SET IDENTITY_INSERT [dbo].[ready_menu] OFF
SET IDENTITY_INSERT [dbo].[ready_menu_and_food] ON 

INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (35, 6, 4, 1, 0.6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (36, 6, 29, 100, 2)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (37, 6, 21, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (38, 1, 4, 1, 0.6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (42, 1, 16, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (43, 1, 21, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (45, 1, 28, 150, 3)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (46, 6, 16, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (48, 7, 4, 1, 0.6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (49, 7, 30, 150, 9)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (50, 7, 14, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (51, 7, 16, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (52, 7, 21, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (53, 8, 4, 1, 0.6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (54, 8, 31, 100, 6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (55, 8, 14, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (56, 8, 16, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (57, 8, 21, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (59, 9, 4, 1, 0.6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (61, 9, 16, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (63, 9, 26, 1, 1.5)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (64, 9, 21, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (69, 9, 36, 120, 6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (70, 11, 4, 1, 0.6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (71, 11, 37, 150, 7.5)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (73, 11, 16, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (74, 11, 21, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (75, 11, 14, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (76, 12, 4, 1, 0.6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (77, 12, 14, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (78, 12, 16, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (79, 12, 21, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (80, 12, 34, 120, 3)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (81, 13, 4, 1, 0.6)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (82, 13, 14, 1, 0.5)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (83, 13, 16, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (84, 13, 21, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (85, 13, 35, 150, 4)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (86, 1, 13, 1, 1)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (87, 14, 3, 5, 1.5)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (88, 14, 4, 2, 1.5)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (89, 14, 31, 100, 4.5)
INSERT [dbo].[ready_menu_and_food] ([id], [ready_menu_id], [food_id], [quantity], [price]) VALUES (90, 13, 40, 1, 1)
SET IDENTITY_INSERT [dbo].[ready_menu_and_food] OFF
SET IDENTITY_INSERT [dbo].[site_food_menu] ON 

INSERT [dbo].[site_food_menu] ([id], [name], [site_menu_id], [image_name], [description]) VALUES (14, N'PİLAV ÜSTÜ KAVURMA', 9, N'585IMG_20180311_132651 (Copy).jpg', N'Menümüze Tulumba, Ayran, Garnitür ve Su dahildir. İçecek için karışık içecek (kola-fanta-ayran) seçeneği mevcuttur.')
INSERT [dbo].[site_food_menu] ([id], [name], [site_menu_id], [image_name], [description]) VALUES (17, N'KAVURMA PİLAV', 9, N'330IMG_20180311_133331 (Copy).jpg', N'Menümüze Tulumba, Ayran, Garnitür ve Su dahildir. İçecek için karışık içecek (kola-fanta-ayran) seçeneği mevcuttur.')
INSERT [dbo].[site_food_menu] ([id], [name], [site_menu_id], [image_name], [description]) VALUES (18, N'PİLAV ÜSTÜ TAVUK', 9, N'951IMG_20180311_133009_1 (Copy).jpg', N'Menümüze Tulumba, Ayran, Garnitür ve Su dahildir. İçecek için karışık içecek (kola-fanta-ayran) seçeneği mevcuttur.')
INSERT [dbo].[site_food_menu] ([id], [name], [site_menu_id], [image_name], [description]) VALUES (19, N'TAVUK PİLAV', 9, N'891IMG_20180311_132944 (Copy).jpg', N'Menümüze Tulumba, Ayran, Garnitür ve Su dahildir. İçecek için karışık içecek (kola-fanta-ayran) seçeneği mevcuttur.')
INSERT [dbo].[site_food_menu] ([id], [name], [site_menu_id], [image_name], [description]) VALUES (22, N'PİLAV ÜSTÜ KAVURMA-KÖPÜK TABAK', 9, N'801IMG_20180311_133538 (Copy).jpg', N'Menümüze Tulumba, Ayran ve Su dahildir. ')
INSERT [dbo].[site_food_menu] ([id], [name], [site_menu_id], [image_name], [description]) VALUES (23, N'PİLAV ÜSTÜ TAVUK-KÖPÜK TABAK', 9, N'727IMG_20180311_133555 (Copy).jpg', N'Menümüze Tulumba, Ayran ve Su dahildir. ')
INSERT [dbo].[site_food_menu] ([id], [name], [site_menu_id], [image_name], [description]) VALUES (24, N'DANA ROSTO', 10, N'991IMG_20180311_133032 (Copy).jpg', N'Menümüze Püre, Ayran, Garnitür ve Su dahildir. İçecek için karışık içecek (kola-fanta-ayran) seçeneği mevcuttur.')
INSERT [dbo].[site_food_menu] ([id], [name], [site_menu_id], [image_name], [description]) VALUES (25, N'TAVUK PİRZOLA', 10, N'662IMG_20180311_133050 (Copy).jpg', N'Menümüze Püre, Ayran, Garnitür ve Su dahildir. İçecek için karışık içecek (kola-fanta-ayran) seçeneği mevcuttur.')
SET IDENTITY_INSERT [dbo].[site_food_menu] OFF
SET IDENTITY_INSERT [dbo].[site_menu] ON 

INSERT [dbo].[site_menu] ([id], [name]) VALUES (9, N'Organizasyon Menülerimiz')
INSERT [dbo].[site_menu] ([id], [name]) VALUES (10, N'Özel Menülerimiz')
SET IDENTITY_INSERT [dbo].[site_menu] OFF
SET IDENTITY_INSERT [dbo].[slider] ON 

INSERT [dbo].[slider] ([id], [filename], [keywords]) VALUES (2, N'slider2.jpg', N'akgül yemek')
INSERT [dbo].[slider] ([id], [filename], [keywords]) VALUES (4, N'831WhatsApp Image 2018-02-06 at 19.32.49.jpeg', N'akgül yemek,akgül organizasyon,akgül')
INSERT [dbo].[slider] ([id], [filename], [keywords]) VALUES (6, N'676WhatsApp Image 2017-09-23 at 17.30.12.jpeg', N'akgül yemek,akgül organizasyon,akgül')
INSERT [dbo].[slider] ([id], [filename], [keywords]) VALUES (15, N'810lkjkllö.jpg', N'akgül yemek,akgül organizasyon,akgül')
INSERT [dbo].[slider] ([id], [filename], [keywords]) VALUES (16, N'356birthday-cake-celebration-353347 (Copy).jpg', N'akgül yemek,akgül organizasyon,akgül')
SET IDENTITY_INSERT [dbo].[slider] OFF
INSERT [dbo].[user] ([telephone], [name], [password], [job], [type]) VALUES (N'05074557917', N'Ali Gül', N'0123', N'Yonetici', 0)
INSERT [dbo].[user] ([telephone], [name], [password], [job], [type]) VALUES (N'05304252698', N'Hasan AKGÜL', N'0123', N'', 0)
INSERT [dbo].[user] ([telephone], [name], [password], [job], [type]) VALUES (N'05308711257', N'Ramazan D', N'124500', N'Aşçı', 0)
INSERT [dbo].[user] ([telephone], [name], [password], [job], [type]) VALUES (N'05313386426', N'Sinan Özden', N'sinan1453', N'Yönetici Asistanı', 0)
INSERT [dbo].[user] ([telephone], [name], [password], [job], [type]) VALUES (N'05367035122', N'MAZLUM BAYRAM SAPMAZ', N'1453', N'MUHASEBE', 0)
INSERT [dbo].[user] ([telephone], [name], [password], [job], [type]) VALUES (N'05372403481', N'Emin Yıldız', N'0123', N'Bulaşıkçı', 0)
INSERT [dbo].[user] ([telephone], [name], [password], [job], [type]) VALUES (N'05378524174', N'Ramazan Demir', N'0123', N'Aşçı', 0)
INSERT [dbo].[user] ([telephone], [name], [password], [job], [type]) VALUES (N'05422113452', N'SALİH DEMİR', N'3452', N'GIDA MÜHENDİSİ', 0)
ALTER TABLE [dbo].[gallery_titles] ADD  DEFAULT ('akgül yemek') FOR [keywords]
GO
ALTER TABLE [dbo].[food]  WITH CHECK ADD  CONSTRAINT [FK_food_food_category] FOREIGN KEY([category_id])
REFERENCES [dbo].[food_category] ([id])
GO
ALTER TABLE [dbo].[food] CHECK CONSTRAINT [FK_food_food_category]
GO
ALTER TABLE [dbo].[food]  WITH CHECK ADD  CONSTRAINT [FK_food_food_units] FOREIGN KEY([unit_id])
REFERENCES [dbo].[food_units] ([id])
GO
ALTER TABLE [dbo].[food] CHECK CONSTRAINT [FK_food_food_units]
GO
ALTER TABLE [dbo].[gallery_titles]  WITH CHECK ADD  CONSTRAINT [FK_GTitles_CategoryID_to_GCategory_ID] FOREIGN KEY([category_id])
REFERENCES [dbo].[gallery_categories] ([id])
GO
ALTER TABLE [dbo].[gallery_titles] CHECK CONSTRAINT [FK_GTitles_CategoryID_to_GCategory_ID]
GO
ALTER TABLE [dbo].[organization_and_food]  WITH CHECK ADD  CONSTRAINT [FK_organization_and_food_food] FOREIGN KEY([food_id])
REFERENCES [dbo].[food] ([id])
GO
ALTER TABLE [dbo].[organization_and_food] CHECK CONSTRAINT [FK_organization_and_food_food]
GO
ALTER TABLE [dbo].[organization_and_food]  WITH CHECK ADD  CONSTRAINT [FK_organization_and_food_organization_information] FOREIGN KEY([organization_information_id])
REFERENCES [dbo].[organization_information] ([id])
GO
ALTER TABLE [dbo].[organization_and_food] CHECK CONSTRAINT [FK_organization_and_food_organization_information]
GO
ALTER TABLE [dbo].[organization_and_service]  WITH CHECK ADD  CONSTRAINT [FK_organization_and_service_organization_information] FOREIGN KEY([organization_information_id])
REFERENCES [dbo].[organization_information] ([id])
GO
ALTER TABLE [dbo].[organization_and_service] CHECK CONSTRAINT [FK_organization_and_service_organization_information]
GO
ALTER TABLE [dbo].[organization_and_service]  WITH CHECK ADD  CONSTRAINT [FK_organization_and_service_organization_service] FOREIGN KEY([organization_service_id])
REFERENCES [dbo].[organization_service] ([id])
GO
ALTER TABLE [dbo].[organization_and_service] CHECK CONSTRAINT [FK_organization_and_service_organization_service]
GO
ALTER TABLE [dbo].[organization_information]  WITH CHECK ADD  CONSTRAINT [FK_organization_information_organization_type] FOREIGN KEY([organization_category_id])
REFERENCES [dbo].[organization_type] ([id])
GO
ALTER TABLE [dbo].[organization_information] CHECK CONSTRAINT [FK_organization_information_organization_type]
GO
ALTER TABLE [dbo].[ready_menu_and_food]  WITH CHECK ADD  CONSTRAINT [FK_ready_menu_and_food_food] FOREIGN KEY([food_id])
REFERENCES [dbo].[food] ([id])
GO
ALTER TABLE [dbo].[ready_menu_and_food] CHECK CONSTRAINT [FK_ready_menu_and_food_food]
GO
ALTER TABLE [dbo].[ready_menu_and_food]  WITH CHECK ADD  CONSTRAINT [FK_ready_menu_and_food_ready_menu] FOREIGN KEY([ready_menu_id])
REFERENCES [dbo].[ready_menu] ([id])
GO
ALTER TABLE [dbo].[ready_menu_and_food] CHECK CONSTRAINT [FK_ready_menu_and_food_ready_menu]
GO
ALTER TABLE [dbo].[site_food_menu]  WITH CHECK ADD  CONSTRAINT [FK_site_food_menu_site_menu] FOREIGN KEY([site_menu_id])
REFERENCES [dbo].[site_menu] ([id])
GO
ALTER TABLE [dbo].[site_food_menu] CHECK CONSTRAINT [FK_site_food_menu_site_menu]
GO
USE [master]
GO