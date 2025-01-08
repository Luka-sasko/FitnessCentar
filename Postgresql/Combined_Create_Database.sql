-- Initial base script with data
CREATE TABLE IF NOT EXISTS "Role" (
    "Id" UUID PRIMARY KEY,
    "RoleName" TEXT,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- Kreiranje tabele 'Exercises'
CREATE TABLE IF NOT EXISTS "Exercises" (
    "Id" UUID PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Desc" TEXT,
    "Reps" INT,
    "Sets" INT,
    "RestPeriod" int,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);


-- Kreiranje tabele 'Discount'
CREATE TABLE IF NOT EXISTS "Discount" (
    "Id" UUID PRIMARY KEY,
    "StartDate" DATE NOT NULL,
	"EndDate" DATE NOT NULL,
    "Amount" INT NOT NULL,
    "Name" TEXT NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- Kreiranje tabele 'Meal'
CREATE TABLE IF NOT EXISTS "Meal" (
    "Id" UUID PRIMARY KEY,
    "Name" TEXT NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE
);

-- Kreiranje tabele 'Subscription'
CREATE TABLE IF NOT EXISTS "Subscription" (
    "Id" UUID PRIMARY KEY,
    "Price" MONEY,
    "Description" TEXT,
    "Name" TEXT,
    "StartDate" DATE,
    "Duration" INT,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	"DiscountId" UUID NOT NULL,
    CONSTRAINT "FK_Discount_Subscription_DiscountId" FOREIGN KEY ("DiscountId") REFERENCES "Discount" ("Id")
);
-- Kreiranje tabele 'User'
CREATE TABLE IF NOT EXISTS "User" (
    "Id" UUID PRIMARY KEY,
    "Password" CHAR(64),
	"Salt" CHAR(44) NOT NULL,
	"Firstname" TEXT NOT NULL,
	"Lastname" TEXT NOT NULL,
	"Email" TEXT NOT NULL,
    "Contact" TEXT NOT NULL,
    "Birthdate" DATE NOT NULL,
    "Weight" DECIMAL,
    "Height" DECIMAL,
    "CoachId" INT,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	"RoleId" UUID NOT NULL,
	"SubscriptionId" UUID not null,
    CONSTRAINT "FK_Role_User_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Role" ("Id"),
    CONSTRAINT "FK_Subscription_User_SubscriptionId" FOREIGN KEY ("SubscriptionId") REFERENCES "Subscription"("Id")
);

-- Kreiranje tabele 'MealPlan'
CREATE TABLE IF NOT EXISTS "MealPlan" (
    "Id" UUID PRIMARY KEY,
    "Name" TEXT NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	"UserId" UUID NOT NULL,
    CONSTRAINT "FK_User_MealPlan_UserId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id")
);

-- Kreiranje tabele 'WorkoutPlan'
CREATE TABLE IF NOT EXISTS "WorkoutPlan" (
    "Id" UUID PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	"UserId" UUID NOT NULL,
    CONSTRAINT "FK_User_WorkoutPlan_WorkoutPlanId" FOREIGN KEY ("UserId") REFERENCES "User" ("Id")
	
);


-- Kreiranje tabele 'WorkoutPlanExercises'
CREATE TABLE IF NOT EXISTS "WorkoutPlanExercise" (
    "Id" UUID PRIMARY KEY,
    "ExerciseNumber" INT,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	"WorkoutPlanId" UUID NOT NULL,
	"ExerciseId" UUID NOT NULL,
    CONSTRAINT "FK_WorkoutPlan_WorkoutPlanExercise_WorkoutPlanId" FOREIGN KEY ("WorkoutPlanId") REFERENCES "WorkoutPlan" ("Id"),
    CONSTRAINT "FK_Exercise_WorkoutPlanExercise_ExerciseId" FOREIGN KEY ("ExerciseId") REFERENCES "Exercises" ("Id")
);





-- Kreiranje tabele 'MealPlanMeal'
CREATE TABLE IF NOT EXISTS "MealPlanMeal" (
    "Id" UUID PRIMARY KEY,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	"MealPlanId" UUID NOT NULL,
	"MealId" UUID NOT NULL,
    CONSTRAINT "FK_MealPlan_MealPlanMeal_MealPlanId" FOREIGN KEY ("MealPlanId") REFERENCES "MealPlan" ("Id"),
    CONSTRAINT "FK_Meal_MealPlanMeal_MealId" FOREIGN KEY ("MealId") REFERENCES "Meal" ("Id")
);

-- Kreiranje tabele 'Food'
CREATE TABLE IF NOT EXISTS "Food" (
    "Id" UUID PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Weight" DECIMAL NOT NULL,
	"CreatedBy" UUID NOT NULL,
    "UpdatedBy" UUID NOT NULL,
    "DateCreated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DatedUpdated" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
	"IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
	"MealId" UUID NOT NULL,
    CONSTRAINT "FK_Meal_Food_MealId" FOREIGN KEY ("MealId") REFERENCES "Meal" ("Id")
);

-- Unos u tablicu 'Role'
INSERT INTO "Role" ("Id", "RoleName", "CreatedBy", "UpdatedBy") VALUES
('11111111-1111-1111-1111-111111111111', 'Admin', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'Discount'
INSERT INTO "Discount" ("Id", "StartDate", "EndDate", "Amount", "Name", "CreatedBy", "UpdatedBy") VALUES
('22222222-2222-2222-2222-222222222222', '2024-01-01', '2024-12-31', 20, 'New Year Discount', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'Subscription'
INSERT INTO "Subscription" ("Id", "Price", "Description", "Name", "StartDate", "Duration", "DiscountId", "CreatedBy", "UpdatedBy") VALUES
('33333333-3333-3333-3333-333333333333', 99.99, 'Annual subscription', 'Premium Plan', '2024-01-01', 12, '22222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'User'
INSERT INTO "User" ("Id", "Password", "Salt", "Firstname", "Lastname", "Email", "Contact", "Birthdate", "Weight", "Height", "CoachId", "RoleId", "SubscriptionId", "CreatedBy", "UpdatedBy") VALUES
('44444444-4444-4444-4444-444444444444', 'hash_password', 'salt_value', 'John', 'Doe', 'johndoe@example.com', '+123456789', '1990-01-01', 75.5, 180.0, NULL, '11111111-1111-1111-1111-111111111111', '33333333-3333-3333-3333-333333333333', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'Meal'
INSERT INTO "Meal" ("Id", "Name", "CreatedBy", "UpdatedBy") VALUES
('55555555-5555-5555-5555-555555555555', 'Healthy Breakfast', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'Food'
INSERT INTO "Food" ("Id", "Name", "Weight", "MealId", "CreatedBy", "UpdatedBy") VALUES
('66666666-6666-6666-6666-666666666666', 'Oatmeal', 200, '55555555-5555-5555-5555-555555555555', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'MealPlan'
INSERT INTO "MealPlan" ("Id", "Name", "UserId", "CreatedBy", "UpdatedBy") VALUES
('77777777-7777-7777-7777-777777777777', 'Weekly Plan', '44444444-4444-4444-4444-444444444444', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'MealPlanMeal'
INSERT INTO "MealPlanMeal" ("Id", "MealPlanId", "MealId", "CreatedBy", "UpdatedBy") VALUES
('88888888-8888-8888-8888-888888888888', '77777777-7777-7777-7777-777777777777', '55555555-5555-5555-5555-555555555555', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'Exercises'
INSERT INTO "Exercises" ("Id", "Name", "Desc", "Reps", "Sets", "RestPeriod", "CreatedBy", "UpdatedBy") VALUES
('99999999-9999-9999-9999-999999999999', 'Push Up', 'Upper body exercise', 15, 3, 60, '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'WorkoutPlan'
INSERT INTO "WorkoutPlan" ("Id", "Name", "Description", "UserId", "CreatedBy", "UpdatedBy") VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Beginner Plan', 'Basic workout routine', '44444444-4444-4444-4444-444444444444', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');

-- Unos u tablicu 'WorkoutPlanExercise'
INSERT INTO "WorkoutPlanExercise" ("Id", "ExerciseNumber", "WorkoutPlanId", "ExerciseId", "CreatedBy", "UpdatedBy") VALUES
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 1, 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', '99999999-9999-9999-9999-999999999999', '11111111-1111-1111-1111-111111111111', '11111111-1111-1111-1111-111111111111');


-- Alter table roles and insert statements
ALTER TABLE "Role"
DROP COLUMN "CreatedBy",
DROP COLUMN "UpdatedBy",
DROP COLUMN "DatedUpdated";

INSERT INTO "Role" ("Id", "RoleName", "DateCreated", "IsActive")
VALUES ('22222222-2222-2222-2222-222222222222', 'User', CURRENT_TIMESTAMP, 'True');

INSERT INTO "Role" ("Id", "RoleName", "DateCreated", "IsActive")
VALUES ('33333333-3333-3333-3333-333333333333', 'Coach', CURRENT_TIMESTAMP, 'True');

ALTER TABLE "User"
ALTER COLUMN "CoachId" TYPE UUID USING (NULL); 


-- Insert default subscription for create/register
INSERT INTO "Subscription" (
    "Id", 
    "Price", 
    "Description", 
    "Name", 
    "StartDate", 
    "Duration", 
    "CreatedBy", 
    "UpdatedBy", 
    "DiscountId"
) 
VALUES (
    '00000000-0000-0000-0000-000000000000', 
    '0',                              
    'Default on create! 0%',
	'Default0',
    '2024-12-01',       -- Datum početka
    365,   
    '123e4567-e89b-12d3-a456-426614174000', -- UUID osobe koja kreira zapis
    '123e4567-e89b-12d3-a456-426614174000', -- UUID osobe koja ažurira zapis
    '22222222-2222-2222-2222-222222222222'  -- UUID vezanog popusta
);

