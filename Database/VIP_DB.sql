
Create database VIP_DB
Go
use VIP_DB
go
create table [Customer] (
	ID int primary key identity(1,1),
	Username varchar(50) unique not null,
	[Password] varchar(50) not null,
	CustomerName varchar(50) not null,
	CustomerAddress varchar(100),
	CustomerPhone bigint unique not null
);
go
create table [Admin] (
	AdminId int primary key identity(1,1),
	UserName varchar(50) unique not null,
	[Password] varchar(50) not null
);
go
create table [Vehicle] (
	ID int primary key identity(1,1),
	VehicleName varchar(50) not null,
	VehicleOwnerName varchar(50) not null,
	VehicleModel varchar(50) not null,
	VehicleVersion varchar(50) not null,
	VehicleRate money not null,
	VehicleBodyNumber varchar(50) not null,
	VehicleEngineNumber varchar(50) not null,
	VehicleNumber int unique not null
);
go

create table [Policy] (
	ID int primary key identity(1,1),
	PolicyType varchar(50) not null,
	[Description] varchar(500) not null,
	-- Coverage: Dynamic content - display All coverage of each policy
	Coverage varchar(8000) not null,
	Annually money not null
); 
go

create table [Option] (
	ID int primary key identity(1,1),
	OptionType varchar(50) Check 
		(OptionType IN (
						'Hire car after accident',
						'Any repairer option'
		)) not null,
	[Description] varchar(500) not null,
	Annually money not null
);
go

create table [OptionDetails] (
	ID int primary key identity(1,1),
	Option1 int not null,
	Option2 int not null,
	IsOption1Included bit Check(IsOption1Included in (0,1)),
	IsOption2Included bit Check(IsOption2Included in (0,1)),
	CONSTRAINT FK_OptionDetails_OptionID1 FOREIGN KEY (Option1) REFERENCES [Option](ID),
	CONSTRAINT FK_OptionDetails_OptionID2 FOREIGN KEY (Option2) REFERENCES [Option](ID)
);
go

create table [Estimate] (
	ID int primary key identity(1,1),
	EstimateNo int unique not null,
	CustomerID int,
	VehicleName varchar(50) not null,
	VehicleModel varchar(50) not null,
	VehicleVersion varchar(50) not null,
	PolicyID int not null,
	OptionDetailsID int,
	PolicyDate date not null,
	PolicyDuration int not null,
	Premium money not null,
	CONSTRAINT FK_Estimate_Policy FOREIGN KEY (PolicyID) REFERENCES [Policy](ID),
	CONSTRAINT FK_Estimate_Customer FOREIGN KEY (CustomerID) REFERENCES [Customer](ID),
	CONSTRAINT FK_Estimate_OptionDetails FOREIGN KEY (OptionDetailsID) REFERENCES [OptionDetails](ID)
); 
go

create table [Certificate] (
	ID int primary key identity(1,1),
	PolicyNo int unique not null,
	EstimateID int unique not null,

	CustomerName varchar(50) not null,
	CustomerAddress varchar(100),
	CustomerPhone bigint not null,
	VehicleBodyNumber varchar(50) not null,
	VehicleEngineNumber varchar(50) not null,

	VehicleWarranty varchar(50) default 'Not Available' 
		Check (VehicleWarranty IN ('Not Available','Available','Pending')),
	Prove varchar(200)
	CONSTRAINT FK_Estimate_Certificate FOREIGN KEY (EstimateID) REFERENCES [Estimate](ID)
); 
go

create table [CustomerBill] (
	ID int primary key identity(1,1),
	BillNo int unique not null,
	CertificateID int not null,
	CustomerAddProve varchar(200),
	[Date] date default getdate(),
	Amount money,
	CONSTRAINT FK_Certificate_Bill FOREIGN KEY (CertificateID) REFERENCES [Certificate](ID)
); 
go

create table [Company_Expense] (
	ID int primary key identity(1,1),
	DateOfExpense varchar(50) not null,
	TypeOfExpense varchar(50) not null,
	AmountOfExpense money not null
); 
go

create table [Claim] (
	ID int primary key identity(1,1),
	ClaimNo int not null,
	CertificateID int not null,
	CustomerName varchar(50) not null,
	PlaceOfAccident varchar(100) not null,
	DateOfAccident varchar(50) not null,
	InsuredAmount money not null,
	ClaimableAmount money not null
	CONSTRAINT FK_Claim_Certificate FOREIGN KEY (CertificateID) REFERENCES [Certificate](ID)
); 
go

INSERT INTO Customer Values
('toann','toann', 'Nguyen Toan', 'PTH', 123123),
('toann1','toann1', 'Nguyen Toan1', 'PTH1', 1231232),
('admin','admin', 'Nguyen Admin', 'Admin address', 2222)
go
INSERT INTO Vehicle Values
('Honda', 'Owner', 'Accord', 'VTi-L', 29000.0000, 'VB5566778891MZ', 'EG-178899-INE', 1111),
('Honda', 'Owner', 'Accord', 'V6-L', 25000.0000, 'VB5566778872MZ', 'EG-278866-INE', 1112),
('Honda', 'Owner', 'City', 'VTi-L', 21000.0000, 'VB5566778893MZ', 'EG-378899-INE', 1113),
('Honda', 'Owner', 'City', 'VTi-LM (LE)', 20000.0000, 'VB5566778874MZ', 'EG-478866-INE', 1114),
('Honda', 'Owner', 'City', 'VTi-L GM', 21000.0000, 'VB5566778895MZ', 'EG-578899-INE', 1115),
('Honda', 'Owner', 'Civic', 'VTi-X', 22000.0000, 'VB5566778876MZ', 'EG-678866-INE', 1116),
('Honda', 'Owner', 'Civic', 'VTi-S', 21000.0000, 'VB5566778897MZ', 'EG-888899-INE', 1117),
('Honda', 'Owner', 'Civic', 'RS', 19800.0000, 'VB5566778878MZ', 'EG-878866-INE', 1118),
('Honda', 'Owner', 'CR-V', 'VTi-S (4x4)', 28000.0000, 'VB5566778889MZ', 'EG-978899-INE', 1119),
('Honda', 'Owner', 'CR-V', 'VTi-L7 (2WD)', 28900.0000, 'VB5566778879MZ', 'EG-108866-INE', 1200),
('Honda', 'Owner', 'CR-V', 'LE (4x2)', 21000.0000, 'VB5566778810MZ', 'EG-118899-INE', 1201),
('Honda', 'Owner', 'CR-V', 'VTi-LM', 20000.0000, 'VB5566778811MZ', 'EG-128866-INE', 1202),
('BMW', 'Owner', 'M5', 'LAUNCH EDITION F90', 21000.0000, 'VB5566778899MZ', 'EG-7788996-INE', 1203),
('BMW', 'Owner', 'M5', 'F10', 52700.0000, 'VB5566778877MZ', 'EG-778866678-INE', 1204),
('BMW', 'Owner', 'M5', 'LAUNCH EDITION F90', 119000.0000, 'VB5566778899MZ', 'EG-7788992-INE', 1205),
('BMW', 'Owner', 'M5', 'NIGHTHAWK F10', 92000.0000, 'VB5566778823MZ', 'EG-77886645-INE', 1206),
('BMW', 'Owner', 'M5', 'LAUNCH EDITION F90', 21000.0000, 'VB5566778832MZ', 'EG-77889912-INE', 1207),
('BMW', 'Owner', 'M5', 'SHADOW F10', 59000.0000, 'VB5566778811MZ', 'EG-778866223-INE', 1208)
go
INSERT INTO [Option] Values
('Hire car after accident', 'description of hire car', 86),
('Any repairer option', 'description of towing car', 206.36)
go
INSERT INTO [OptionDetails] Values
(1, 2, 0, 0),
(1, 2, 0, 1),
(1, 2, 1, 0),
(1, 2, 1, 1)
go

INSERT INTO [Policy] Values
('Third Party Fire and Theft',
	'Get great value cover with RACV Third Party Fire and Theft Insurance. You’ll be covered for unintentional damage you cause to other people’s property. And you can rest at ease knowing you’re also covered for up to $10,000 if your car is stolen or damaged by a fire.',
	'<div class="tab-component" id="inclusions">
<h4>What does Third Party Fire and Theft Car Insurance cover? </h4>
<div class="tabs">
<ul class="nav nav-tabs yellow-tab">
<li class=""><a class="tab-links" id="inclusions0" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions0">Inclusions</a></li>
<li class="active in"><a class="tab-links" id="inclusions1" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions1">Exclusions</a></li>
</ul>
</div>
<div class="tab-content">
<div id="tabmenuinclusions0" class="tab-pane fade in u-hide">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child0_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div id="canvas_365535739_cop_392723712_canvas_par_rte" class="rte ">
<div>With an RACV Third Party Property Fire and Theft Insurance policy, you’ll be covered for a range of events. Here’s a summary of what’s included:<br>
<br>
</div>
</div><div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_365535739_cop_392723712_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small u-spacing-bottom-zero@xsmall u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--tick ">
<ul style="list-style-position: inside;">
<li><b>Liability cover up to $20 million</b> for unintentional damage to someone else’s vehicle or property.</li>
<li><b>Up to $10,000 in cover for loss or damage</b> to your vehicle caused by fire, theft or attempted theft.</li>
<li><b>Hire car after a theft or attempted theft</b> where your vehicle can’t be driven, for up to 21 days.<br>
</li>
</ul>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small  u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--tick ">
<ul style="list-style-position: inside;">
<li><b>Up to $5000 in limited cover</b> for damage to your vehicle if you’re not at fault in the collision, the other driver is uninsured and you can provide their details.<br>
</li>
<li><b>One tow</b> from an incident to a safe place or repairer.<br>
</li>
<li><b>Any licenced driver (including learners)</b> with permission to drive your vehicle will be covered. Additional excess may apply to inexperienced drivers or drivers under 25.</li>
</ul>
</div>
</div>
</div>
</div>
<div id="canvas_365535739_cop_1798917204_canvas_par_rte" class="rte ">
<p><sub><i>This is only a summary of the inclusions for RACV Third Party Fire and Theft Insurance. For more information, such as conditions, limits and exclusions, please read the <a href="/insurance/policy-documents/motor.html#pds" target="_self">Product Disclosure Statement</a>.</i></sub><br>
</p>
</div>
</div>
</div>
</section>
</div>
<div id="tabmenuinclusions1" class="tab-pane fade active">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child1_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_365535739_cop_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small u-spacing-bottom-zero@xsmall u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--cross ">
<ul style="list-style-position: inside;">
<li>Cover for repairs to your vehicle where you are at fault in the collision.</li>
<li>Running costs of a hire car, such as petrol or tolls.</li>
<li>Loss or damage to a hire car.<br>
</li>
</ul>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small  u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--cross ">
<ul style="list-style-position: inside;">
<li style="text-align: left;">Liability from the use of a hire car.</li>
<li style="text-align: left;">A hire car arranged by you, unless RACV Insurance&nbsp;approved it beforehand.<br>
</li>
</ul>
</div>
</div>
</div>
</div>
<div id="canvas_365535739_cop_1798917204_canvas_par_rte" class="rte ">
<p><sub><i>For more information on General Exclusions and items not included in your cover, refer to the <a href="/insurance/policy-documents/motor.html#pds">Product Disclosure Statement</a>.</i></sub></p>
</div>
</div>
</div>
</section>
</div>
</div>
</div>',
	500),
('Third-Party Property Damage',
	'Third Party Property Damage insurance is the most affordable, basic cover offered by RACV for unintentional damage you cause to another person’s vehicle or property, such as colliding with another car or hitting someone’s fence.',
	'<div class="tab-component" id="inclusions">
<h4>What does Third Party Property Damage Car Insurance cover? </h4>
<div class="tabs">
<ul class="nav nav-tabs yellow-tab">
<li class="active in"><a class="tab-links" id="inclusions0" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions0">Inclusions</a></li>
<li class=""><a class="tab-links" id="inclusions1" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions1">Exclusions</a></li>
</ul>
</div>
<div class="tab-content">
<div id="tabmenuinclusions0" class="tab-pane fade in active">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child0_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div id="canvas_365535739_cop_392723712_canvas_par_rte" class="rte ">
<div>With an RACV Third Party Property Damage car insurance policy, you’ll be covered for a range of events. Here’s a summary of what’s included:<br>
<br>
</div>
</div><div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_365535739_cop_392723712_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small u-spacing-bottom-zero@xsmall u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--tick ">
<ul style="list-style-position: inside;">
<li><b>Liability cover up to $20 million </b>for unintentional damage to someone else’s vehicle or property.</li>
<li><b>Up to $5000 in limited cover</b> for damage to your vehicle if you’re not at fault in the collision, the other driver is uninsured and you can provide their details.</li>
</ul>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small  u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--tick ">
<ul style="list-style-position: inside;">
<li><b>One tow</b> from an incident to a safe place or repairer.<br>
</li>
<li><b>Any licenced driver (including learners)</b> with permission to drive your vehicle will be covered. Additional excess may apply to inexperienced drivers or drivers under 25.</li>
</ul>
</div>
</div>
</div>
</div>
<div id="canvas_365535739_cop_1798917204_canvas_par_rte" class="rte ">
<p><sub><i>This is only a summary of the inclusions for RACV Third Party Property Damage Insurance. For more information, such as conditions, limits and exclusions, please read the <a href="/insurance/policy-documents/motor.html#pds" target="_self">Product Disclosure Statement</a>.</i></sub><br>
</p>
</div>
</div>
</div>
</section>
</div>
<div id="tabmenuinclusions1" class="tab-pane fade u-hide">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child1_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_365535739_cop_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small u-spacing-bottom-zero@xsmall u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--cross ">
<ul style="list-style-position: inside;">
<li>Cover for repairs to your vehicle where you are at fault in the collision.</li>
<li>Financial protection if your car is stolen or is damaged as a result of an attempted theft.</li>
<li>Hire car after an accident, theft or attempted theft.<br>
</li>
</ul>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small  u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--cross ">
<ul style="list-style-position: inside;">
<li style="text-align: left;">Cover for windscreen, sunroof and glass damage as a result of an incident.</li>
<li style="text-align: left;">New replacement vehicle if RACV Insurance considers your vehicle to be a total loss.<br>
</li>
</ul>
</div>
</div>
</div>
</div>
<div id="canvas_365535739_cop_1798917204_canvas_par_rte" class="rte ">
<p><sub><i>For more information on General Exclusions and items not included in your cover, refer to the <a href="/insurance/policy-documents/motor.html#pds">Product Disclosure Statement</a>.</i></sub></p>
</div>
</div>
</div>
</section>
</div>
</div>
</div>',
	490),
('Comprehensive',
'Comprehensive car insurance protects you from potentially hefty bills when an accident occurs. No matter who’s at fault, you and anyone permitted to drive your vehicle1 , will be covered for collision damage to your vehicle and other people’s property.

Go with the standard inclusions, or add optional extras to suit your needs and budget — like hire car cover, windscreen cover with no excess or pick your own licensed repairer.*',
'<div class="tab-component" id="inclusions">
<h4>What does RACV Comprehensive Car Insurance cover? </h4>
<div class="tabs">
<ul class="nav nav-tabs yellow-tab">
<li class="active in"><a class="tab-links" id="inclusions0" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions0">Inclusions</a></li>
<li class=""><a class="tab-links" id="inclusions1" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions1">Exclusions</a></li>
<li class=""><a class="tab-links" id="inclusions2" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions2">Optional extras</a></li>
</ul>
</div>
<div class="tab-content">
<div id="tabmenuinclusions0" class="tab-pane fade in active">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child0_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div id="canvas_365535739_cop_392723712_canvas_par_rte" class="rte ">
<div>With an RACV Comprehensive Car Insurance policy, you’ll be covered for a range of events. Here’s a summary of what’s included:<br>
<br>
</div>
</div><div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_365535739_cop_392723712_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn_copy"></form>
<div class="u-1/2 u-padding-base-LR@small u-spacing-bottom-zero@xsmall u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--tick ">
<ul style="list-style-position: inside;">
<li><b>Loss or damage to your vehicle</b> caused by an accident, flood, fire, malicious act, storm, theft or attempted theft.</li>
<li><b>Hire car after a not-at-fault collision</b> if your car is unsafe to drive or is in for repairs. The at-fault driver’s details must be provided.<br>
</li>
<li><b>Liability cover up to $20 million</b> for damage to someone else’s vehicle or property.<br>
</li>
</ul>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn_copy"></form>
<div class="u-1/2 u-padding-base-LR@small  u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--tick ">
<ul style="list-style-position: inside;">
<li><b>Up to $500 for personal items</b> if they’re stolen with your vehicle or damaged in an incident. Some exclusions apply.<br>
</li>
<li><b>New replacement vehicle</b>&nbsp;where available if your vehicle is up to 2 years old and RACV Insurance considers it to be a total loss and agrees to pay your claim.<br>
</li>
<li><b>Any licenced driver (including learners) </b>with permission to drive your vehicle will be covered. Additional excess may apply to inexperienced drivers or drivers under 25.</li>
</ul>
</div>
</div>
</div>
</div>
<div id="canvas_365535739_cop_1798917204_canvas_par_rte" class="rte ">
<p><sub><i>This is only a summary of the inclusions for RACV Comprehensive&nbsp;Insurance. For more information, such as conditions, limits and exclusions, please read the&nbsp;<a href="/insurance/policy-documents/motor.html#pds" target="_self">Product Disclosure Statement</a>.&nbsp;</i></sub></p>
</div>
</div>
</div>
</section>
</div>
<div id="tabmenuinclusions1" class="tab-pane fade u-hide">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child1_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_365535739_cop_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small u-spacing-bottom-zero@xsmall u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--cross ">
<ul style="list-style-position: inside;">
<li>Running costs of a hire car, such as petrol or tolls.</li>
<li>Loss or damage to a hire car.<br>
</li>
</ul>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small  u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--cross ">
<ul style="list-style-position: inside;">
<li style="text-align: left;">Liability from the use of a hire car.</li>
<li style="text-align: left;">A hire car arranged by you, unless RACV Insurance&nbsp;approved it beforehand.<br>
</li>
</ul>
</div>
</div>
</div>
</div>
<div id="canvas_365535739_cop_607911654_canvas_par_rte" class="rte ">
<p><sub><i>For more information on General Exclusions and items not included in your cover, refer to the <a href="/insurance/policy-documents/motor.html#pds">Product Disclosure Statement</a>.&nbsp;</i></sub></p>
</div>
</div>
</div>
</section>
</div>
<div id="tabmenuinclusions2" class="tab-pane fade u-hide">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child2_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div id="canvas_1819562518_canvas_par_rte" class="rte ">
<p>For added peace of mind, you can choose to boost your cover with optional extras. If you do this, you’ll need to pay more on your premium.<br>
</p>
</div><div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_1819562518_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2  u-spacing-bottom-small@xsmall u-container-form-builder">
<div id="gridcolumn_0_rte" class="rte ">
<p><b>Hire Car regardless of who is at fault</b><br>
Under Comprehensive Insurance, you’re covered for a hire car for not-at-fault collisions if you can provide the details of the at-fault driver, as well as theft or attempted theft of your vehicle. By adding this option, you’ll be covered for a hire car regardless of who is at fault.<br>
</p>
<p><b>Windscreen cover with no excess</b><br>
If you add this option, you won’t need to pay any excess when only your vehicle’s windscreen, sunroof or window glass is damaged as a result of an incident.</p>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2   u-container-form-builder">
<div id="gridcolumn_0_rte" class="rte ">
<p><b>Any repairer<br>
</b>Under Comprehensive Insurance, RACV Insurance will arrange for an <a href="https://www.racv.com.au/on-the-road/insurance/car-insurance/repairer-map.html" style="background-color: rgb(255,255,255);">RACV Partner Repairer</a> to&nbsp;fix your vehicle.&nbsp;If you choose this option, you can pick any licensed repairer&nbsp;to fix your vehicle.</p>
</div>
</div>
</div>
</div>
<div id="canvas_589014929_canvas_par_rte" class="rte ">
<p><sub><i>For associated conditions, see the <a href="/insurance/policy-documents/motor.html#pds">Product Disclosure Statement</a>.</i></sub></p>
</div>
</div>
</div>
</section>
</div>
</div>
</div>',
2000),
('Complete Care®', 
	'Complete Care® Car Insurance covers all the things you’d expect from comprehensive cover, plus extra inclusions like emergency accommodation, pet assistance, and no excess for windscreen, sunroof and window glass repairs. Drive with confidence when you choose the highest level of RACV Motor Insurance available.',
'<div class="tab-component" id="inclusions">
<h4>What does Complete Care® Car Insurance cover? </h4>
<div class="tabs">
<ul class="nav nav-tabs yellow-tab">
<li class="active in"><a class="tab-links" id="inclusions0" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions0">Inclusions</a></li>
<li class=""><a class="tab-links" id="inclusions1" data-toggle="tab" href="javascript:void(0);" data-tabid="#tabmenuinclusions1">Exclusions</a></li>
</ul>
</div>
<div class="tab-content">
<div id="tabmenuinclusions0" class="tab-pane fade in active">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child0_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div id="canvas_365535739_cop_392723712_canvas_par_rte" class="rte ">
<div>With an RACV Complete Care® Car Insurance policy, you’ll be covered for a range of events. Here’s a summary of what’s included:<br>
</div>
</div><div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_365535739_cop_392723712_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small u-spacing-bottom-zero@xsmall u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--tick ">
<ul style="list-style-position: inside;">
<li><b>Loss or damage to your vehicle</b> caused by an accident, flood, fire, malicious act, storm, theft or attempted theft.</li>
<li><b>Hire car after an incident or theft</b> until your vehicle is repaired and returned to you.</li>
<li><b>Pet assistance</b> in the event you’re involved in an incident more that 100km from home and need to stay in pet-friendly accommodation.</li>
<li><b>Cover for windscreen, sunroof and window glass damage</b> as a result of an incident, without any excess payment.<br>
</li>
</ul>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small  u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--tick ">
<ul style="list-style-position: inside;">
<li><b>Vehicle towing, pickup and return</b> if you’re involved in an incident.</li>
<li><b>Liability cover up to $20 million</b> for unintentional collision damage to someone else’s vehicle or property.</li>
<li><b>New replacement vehicle</b>&nbsp;if RACV Insurance considers your vehicle to be a total loss and agrees to pay your claim. Conditions apply.<br>
</li>
<li><b>Any licenced driver (including learners) </b>with permission to drive your vehicle will be covered. Additional excess may apply to inexperienced drivers or drivers under 25.</li>
</ul>
</div>
</div>
</div>
</div>
<div id="canvas_365535739_cop_1798917204_canvas_par_rte" class="rte ">
<p><sub><i>This is only a summary of the inclusions&nbsp;for&nbsp;Complete Care® Motor Insurance. For more information, such as conditions, limits and exclusions, please read the&nbsp;<a href="/insurance/policy-documents/motor.html#pds" target="_self">Product Disclosure Statement</a>.</i></sub></p>
</div>
</div>
</div>
</section>
</div>
<div id="tabmenuinclusions1" class="tab-pane fade u-hide">
<section class="u-spacing-top-small none u-spacing-bottom-small none " id="tabbedcontent_child1_canvas">
<div class="o-grid">
<div class="o-content-wrap">
<div class="gridColumn parbase section">
<div class="form_row clearfix" id="canvas_365535739_cop_canvas_par_gridcolumn">
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small u-spacing-bottom-zero@xsmall u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--cross ">
<ul style="list-style-position: inside;">
<li>Accommodation rates beyond the cost of the room (such as mini bar costs)</li>
<li>Veterinary costs if your pet is injured.</li>
<li>Running costs of a hire car, such as petrol or tolls.<br>
</li>
</ul>
</div>
</div>
<form><input type="hidden" disabled="" name="gridcolumn"></form>
<div class="u-1/2 u-padding-base-LR@small  u-container-form-builder">
<div id="canvas_365535739_canvas_par_rte" class="rte c-list c-list--cross ">
<ul style="list-style-position: inside;">
<li style="text-align: left;">Loss or damage to a hire car.</li>
<li style="text-align: left;">Liability from the use of a hire car.</li>
<li style="text-align: left;">A hire car arranged by you, unless RACV Insurance approved it beforehand.<br>
</li>
</ul>
</div>
</div>
</div>
</div>
<div id="canvas_365535739_cop_1798917204_canvas_par_rte" class="rte ">
<p><sub><i>For more information on General Exclusions and items not included in your cover, refer to the <a href="/insurance/policy-documents/motor.html#pds">Product Disclosure Statement</a>.</i></sub></p>
</div>
</div>
</div>
</section>
</div>
</div>
</div>',
2800)
go
INSERT INTO [Estimate] Values
(3333, 1, 'Honda', 'City', 'VTi-L', 1, 1,'2022-12-24', 12, 1200),
(3334, 2, 'BMW', 'M5', 'NIGHTHAWK F10', 3, 1,'2023-01-14', 12, 2200)
go

INSERT INTO [Certificate] Values
(5555, 1, 'OwnerName', 'OwnerAddress', 903368689,'VB5566778893MZ', 'EG-378899-INE', null, null),
(5556, 2, 'OwnerName 2', 'OwnerAddress 2', 903379789, 'VB5566778823MZ', 'EG-77886645-INE', null, null)
go

INSERT INTO [CustomerBill] Values
(221218444, 1, null, null, 900),
(221218445, 2, null, null, 3000)
go
